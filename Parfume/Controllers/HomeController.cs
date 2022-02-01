using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parfume.DAL;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ParfumeContext _context;

        public HomeController(ParfumeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HomePage()
        {
            return View();
        }
        public IActionResult SaleCredite()
        {
            return View();
        }
        public IActionResult SaleCash()
        {
            return View();
        }
        public IActionResult Pay(int orderId)
        {
            var model=_context.Orders.Where(c => c.Id == orderId).Include(c=>c.PaymentHistories).Include(c=>c.Customer).FirstOrDefault();
            return View(model);
        }

        public IActionResult HistoryCredite()
        {
           var model= _context.Orders.Where(c=>c.IsCredite && c.Debt==0).Include(c => c.Customer ).Include(c => c.User).ToList();
            return View(model);
        }
        public IActionResult History()
        {
            var model = _context.Orders.Where(c=>c.IsCredite==false).Include(c => c.Customer).Include(c => c.User).ToList();
            return View(model);
        }
        public IActionResult DebtHistory()
        {
            var model = _context.Orders.Where(c => c.Debt > 0).Include(c => c.Customer).Include(c => c.User).ToList();
            return View(model);
        }
        public IActionResult WhoPays()
        {
            var model = _context.Orders.Where(c=>c.Debt>0 && c.Status==2).Include(c => c.Customer).Include(c => c.User).OrderBy(c=>c.PaymentDate).ToList();
            return View(model);
        }
        public IActionResult HistoryDetails(int orderId)
        {
            var model = _context.Orders.Where(c => c.Id == orderId).Include(c => c.Customer).Include(c=>c.PaymentHistories).Include(c=>c.User).FirstOrDefault();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult HistoryPaymentDate(string dateRange)
        {
            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = DateTime.Now;
            if (String.IsNullOrEmpty(dateRange))
            {
                dateRange = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (dateRange.Contains("Invalid date - Invalid date") || dateRange.Contains("Hamısı"))
            {
                dateRange = SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
            }
           else if (!String.IsNullOrEmpty(dateRange))
            {
                startDateTime = DateTime.ParseExact(dateRange.Split('-')[0].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                endDateTime = DateTime.ParseExact(dateRange.Split('-')[1].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            var order = _context.Orders.Where(c=>c.PaymentDate> startDateTime  && c.PaymentDate< endDateTime).ToList();
            var crediteHistory = _context.CrediteHistories.Where(c => c.CreateDate > startDateTime && c.CreateDate < endDateTime).ToList();
            var model = new HistoryPayVM();
            var cachOrder = 0;
            var crediteOrder = 0;
            double? Debt = 0;
            double cachMany = 0;
            double? generalMany = 0;
            double? CrediteMany = 0;
            double? SaleMany = 0;
            double? importantMany = 0;

            foreach (var item in order)
            {
                if (!item.IsCredite)
                {
                    cachOrder++;
                    cachMany += item.TotalPrice;
                    generalMany += item.TotalPrice;
                }
                else
                {
                    crediteOrder++;
                    Debt += item.Debt;
                    generalMany += (item.TotalPrice - item.Debt);
                     
                }
                SaleMany += item.TotalPrice;
                importantMany += item.MonthPrice;
            }
            foreach (var item in crediteHistory)
            {
                CrediteMany += item.CachMany;
            }
            model.Debt = Debt;
            model.CachOrder = cachOrder;
            model.SaleMany = SaleMany;
            model.ImportantMany = importantMany + CrediteMany;
            model.CachMany = cachMany;
            model.CrediteOrder = crediteOrder;
            model.GeneralMany = generalMany;
            model.OrderCount = cachOrder + crediteOrder;
            model.CrediteMany = CrediteMany;
        
            return Json(new { status = "success",  data= model });
        }

        public IActionResult HistoryPayment()
        {
             
            var order = _context.Orders.ToList();
           
            var model = new HistoryPayVM();
            var cachOrder = 0;
            var crediteOrder = 0;
            double? Debt = 0;
            double cachMany = 0;
            double? generalMany = 0;
            double? CrediteMany = 0;
            double? SaleMany = 0;

            foreach (var item in order)
            {
                if (!item.IsCredite)
                {
                    cachOrder++;
                    cachMany += item.TotalPrice;
                    generalMany += item.TotalPrice;
                }
                else
                {
                    crediteOrder++;
                    Debt += item.Debt;
                    generalMany += (item.TotalPrice - item.Debt);
                    CrediteMany += (item.TotalPrice - item.Debt);
                }
                SaleMany += item.TotalPrice;
            }

            model.Debt = Debt;
            model.CachOrder = cachOrder;
            model.SaleMany = SaleMany;
            model.CachMany = cachMany;
            model.CrediteOrder = crediteOrder;
            model.GeneralMany = generalMany;
            model.OrderCount = cachOrder + crediteOrder;
            model.CrediteMany = CrediteMany;

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult HistoryCreateDate(string dateRange)
        {
            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = DateTime.Now;
            if (String.IsNullOrEmpty(dateRange))
            {
                dateRange = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (dateRange.Contains("Invalid date - Invalid date") || dateRange.Contains("Hamısı"))
            {
                dateRange = SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (!String.IsNullOrEmpty(dateRange))
            {
                startDateTime = DateTime.ParseExact(dateRange.Split('-')[0].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                endDateTime = DateTime.ParseExact(dateRange.Split('-')[1].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            var order = _context.Orders.Where(c => c.PaymentDate > startDateTime && c.PaymentDate < endDateTime).ToList();

            var model = new HistoryPayVM();
            var cachOrder = 0;
            var crediteOrder = 0;
            double? Debt = 0;
            double cachMany = 0;
            double? generalMany = 0;
            double? CrediteMany = 0;
            double? SaleMany = 0;

            foreach (var item in order)
            {
                if (!item.IsCredite)
                {
                    cachOrder++;
                    cachMany += item.TotalPrice;
                    generalMany += item.TotalPrice;
                }
                else
                {
                    crediteOrder++;
                    Debt += item.Debt;
                    generalMany += (item.TotalPrice - item.Debt);
                    CrediteMany += (item.TotalPrice - item.Debt);
                }
                SaleMany += item.TotalPrice;
            }

            model.Debt = Debt;
            model.CachOrder = cachOrder;
            model.SaleMany = SaleMany;
            model.CachMany = cachMany;
            model.CrediteOrder = crediteOrder;
            model.GeneralMany = generalMany;
            model.OrderCount = cachOrder + crediteOrder;
            model.CrediteMany = CrediteMany;
            return View(model);
        }
    }
}
