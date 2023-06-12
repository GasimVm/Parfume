using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using Parfume.Models;
using Parfume.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Controllers
{
    public class BonusController : Controller
    {
        private readonly ParfumeContext _context;
        private readonly IUserService _userService;

        public BonusController(ParfumeContext context , IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public IActionResult Index()
        {
            var model = _context.Customers.Where(c => c.IsBlock == false && c.IsActive == true ).Include(c => c.Orders).ToList();
             
            return View(model);
        }
        public IActionResult CreateVip()
        {
            var model = _context.Customers.Where(c => c.IsBlock == false && c.IsActive == true && c.IsVIP != true).Include(c => c.Orders).ToList();
            return View(model);
        }
        public IActionResult AddVipCustomer(int customerId)
        {
            ViewBag.CustomerId = customerId;
            return View(customerId);
        }
        [HttpPost]
        public JsonResult CreateVip(int customerId,int degree)
        {
            try
            {
                if (!_context.Customers.Any(c => c.IsBlock == false && c.IsActive == true && c.Id == customerId))
                {
                    return Json(new { status = "error", message = "Bele aktiv  müştəri tapilmadi" });
                }
                    var customer = _context.Customers.Where(c => c.IsBlock == false && c.IsActive == true && c.Id == customerId).First();
                customer.IsVIP = true;
                customer.BonusDegree = degree;
                customer.BonusAmount = 0;
                customer.ReferencesId = customer.Id;
                _context.Customers.Update(customer);
                _context.SaveChanges();

                _context.Bonus.Add(new Bonus()
                {
                    CustomerId = customerId,
                    Precent = degree,
                     Amount=0

                });
                _context.SaveChanges();
                _context.Logs.Add(new Log()
                {
                    Error = "Add new vip customer ",
                    BrowserInfo = "customer  id=" + customerId.ToString(),
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception)
            {

                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
           
        }

        public IActionResult History()
        {
          var model=  _context.BonusHistories.Include(c=>c.Customer).Include(c=>c.Order).ToList();
            return View(model); 
        }
        public IActionResult ReferencesCustomers(int customerId)
        {
            var model = _context.Customers.Where(c=>c.ReferencesId==customerId).ToList();
            return View(model);
        }
    }
}
