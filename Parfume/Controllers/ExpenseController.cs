using Microsoft.AspNetCore.Mvc;
using Parfume.DAL;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parfume.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ParfumeContext _context;
        public ExpenseController(ParfumeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(string name, int money, string date)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                var format = "dd/MM/yyyy";
                CultureInfo provider = CultureInfo.InvariantCulture;
                var CreateDate = DateTime.Now;
                if (date != null)
                {
                    CreateDate = DateTime.ParseExact(date, format, provider);

                }
                _context.Expenses.Add(new Expense()
                {
                    Name = name,
                    Money = money,
                    CreateDate = CreateDate
                });
                _context.SaveChanges();

                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception ex)
            {

                _context.Logs.Add(new Log()
                {
                    Error = ex.Message ?? "İstifadəçi adı və ya şifrə yanlışdır.",
                    UserId = UserId,
                    Success = false,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
            
        }

        public IActionResult History()
        {
           var model= _context.Expenses.ToList();
            return View(model);
        }
        public JsonResult ExpenseDelete(int ExpenseId)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                if (_context.Expenses.Any(c => c.Id == ExpenseId))
                {
                    var cardDb = _context.Expenses.Where(c => c.Id == ExpenseId).First();
                     
                    _context.Expenses.Remove(cardDb);
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
                }
                return Json(new { status = "error", message = "Kart tapilmadi" });


            }
            catch (Exception ex)
            {

                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }
    }
}
