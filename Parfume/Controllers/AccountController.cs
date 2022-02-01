using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Tend.Models;

namespace Parfume.Controllers
{
    public class AccountController : Controller
    {
        private readonly ParfumeContext _context;
        public AccountController(ParfumeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult Login( )
        {
            
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            System.Net.IPAddress remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            string browserInfo = Request.Headers["User-Agent"].ToString();
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yanlışdır.");
                    return View(model);
                }
                var user = _context.Users.Where(c => c.Fincode.ToUpper() == model.Fincode.ToUpper() && c.Password.Trim() == model.Password.Trim()).Include(c=>c.Role).FirstOrDefault();
                if (user == null)
                {
                    ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yanlışdır.");
                    return View(model);
                }
                return await SignInUser(user);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yanlışdır.");
                return View(model);
            }
        }

        private async Task<IActionResult> SignInUser(User AuthentificatinModel)
        {
            System.Net.IPAddress remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            string browserInfo = Request.Headers["User-Agent"].ToString();
            try
            {
                List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.GivenName,AuthentificatinModel.FatherName),
                new Claim(ClaimTypes.Surname,AuthentificatinModel.Surname),
                new Claim(ClaimTypes.Name,AuthentificatinModel.Name),
                new Claim(ClaimTypes.NameIdentifier,AuthentificatinModel.Fincode),
                new Claim(ClaimTypes.Role,AuthentificatinModel.Role.Name.ToString()),
                new Claim(ClaimTypes.PrimarySid,AuthentificatinModel.Id.ToString())
            };
                 
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);
                Thread.CurrentPrincipal = principal;

                _context.Logs.Add(new Log()
                {
                    BrowserInfo = browserInfo,
                    RemoteIpAddress = remoteIpAddress.ToString(),
                    Fincode = AuthentificatinModel.Fincode,
                    UserId= AuthentificatinModel.Id,
                    Success = true,
                    Type=1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
               
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "İstifadəçi adı və ya şifrə yanlışdır.");
                _context.Logs.Add(new Log()
                {
                    BrowserInfo = browserInfo,
                    RemoteIpAddress = remoteIpAddress.ToString(),
                    Error = "İstifadəçi adı və ya şifrə yanlışdır.",
                    Fincode = AuthentificatinModel.Fincode,
                    Success = false,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();

                return RedirectToAction("Login");
            }
        }

        public async Task<IActionResult> LogOut()
        {
            string userFincode = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            System.Net.IPAddress remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            string browserInfo = Request.Headers["User-Agent"].ToString();
            _context.Logs.Add(new Log()
            {
                BrowserInfo = browserInfo,
                RemoteIpAddress = remoteIpAddress.ToString(),
                Fincode = userFincode,
                Success = true,
                Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
            });
            _context.SaveChanges();

            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult AccessDenied(LoginViewModel model)
        {
            return Json("access denied");
        }

        [Authorize]
        [HttpPost]
        public bool SubscribePushNotification(PushSubscribtionModel pushSubscribtion)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            if (pushSubscribtion != null)
            {
                _context.userWebPushCredentials.Add(new UserWebPushCredentials
                {
                    UserId = userId,
                    Auth = pushSubscribtion.Auth,
                    P256dh = pushSubscribtion.P256dh,
                    PushEndPoint = pushSubscribtion.PushEndPoint,
                     CreateDate=DateTime.Now
                });
            }
            return _context.SaveChanges() > 0;
        }

        public IActionResult ChangePassword( )
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            var userDb = _context.Users.Where(c => c.Id == UserId).FirstOrDefault();
            return View(userDb);
        }
        [HttpPost]
        public JsonResult ChangePassword(string oldPassword,string newPassword)
        {
            try
            {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
               var userDb= _context.Users.Where(c => c.Id == UserId).FirstOrDefault();

                if (userDb.Password==oldPassword)
                {
                    if (newPassword==" " ||newPassword==null)
                    {
                        return Json(new { status = "error", message = "Parol boş və ya boşluq ola bilməz!" });
                    }
                    userDb.Password = newPassword;
                    _context.Users.Update(userDb);
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi!" });
                }
                return Json(new { status = "error", message = "Köhnə parol səhvdi!" });
            }
            catch (Exception)
            {

                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }
    }
}
