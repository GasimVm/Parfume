using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parfume.DAL;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly ParfumeContext _context;

        public ReportController(ParfumeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReportPayment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReportPayment(string dateRange)
        {
            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = DateTime.Now;
            var model = new ReportPaymentVM();
            double? CachMany = 0;// nagd satisdan gelen
            double? CrediteMany = 0;// ayliq odenisden gelen pullar
            double? Balans = 0;//butun pullar hem kreditinin ayliq odenisinden ve nagd satisdan gelen
            double? Important = 0;// odenmeli pullar
            double? difference = 0;//fix ayliq  ile ayliq odenilen pulun ferqi
          
            if (String.IsNullOrEmpty(dateRange))
            {
                dateRange = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (dateRange.Contains("Invalid date - Invalid date") || dateRange.Contains("Hamısı"))
            {
                dateRange = SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy") + "-" + SqlDateTime.MaxValue.Value.ToString("dd/MM/yyyy");
            }
              if (!String.IsNullOrEmpty(dateRange))
            {
                startDateTime = DateTime.ParseExact(dateRange.Split('-')[0].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                endDateTime = DateTime.ParseExact(dateRange.Split('-')[1].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            var order = _context.Orders.Where(c => c.CreateDate > startDateTime && c.CreateDate < endDateTime).ToList();
            var paymentHistory = _context.PaymentHistories.Where(c => c.PayDate > startDateTime && c.PayDate < endDateTime).ToList();

            foreach (var item in paymentHistory)
            {
                if (item.Status)
                {
                    CrediteMany += item.PayPrice;
                    difference += Convert.ToDouble(item.PayPrice - item.MonthPrice);
                }
                Important += item.MonthPrice;
            }


            foreach (var item in order)
            {
                if (!item.IsCredite)
                {
                    CachMany += item.TotalPrice;
                }
                 
            }
            //foreach (var item in crediteHistory)
            //{
            //    CrediteMany += item.CachMany;
            //}
            Balans = CachMany + CrediteMany;
             
            model.Debt = Important- CrediteMany;
            model.Balans = Balans;
            model.CachMany = CachMany;
            model.CrediteMany = CrediteMany;
            model.Important = Important+ difference+ CachMany;
            return Json(new { status = "success", data = model });
        }

        public IActionResult ReportCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReportCreate(string dateRange)
        {
            DateTime startDateTime = DateTime.MinValue;
            DateTime endDateTime = DateTime.Now;
            if (String.IsNullOrEmpty(dateRange))
            {
                dateRange = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (dateRange.Contains("Invalid date - Invalid date") || dateRange.Contains("Hamısı"))
            {
                dateRange = SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy") + "-" + SqlDateTime.MaxValue.Value.ToString("dd/MM/yyyy");
            }
              if (!String.IsNullOrEmpty(dateRange))
            {
                startDateTime = DateTime.ParseExact(dateRange.Split('-')[0].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                endDateTime = DateTime.ParseExact(dateRange.Split('-')[1].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            var order = _context.Orders.Where(c => c.CreateDate > startDateTime && c.CreateDate < endDateTime).ToList();
            var crediteHistory = _context.CrediteHistories.Where(c => c.CreateDate > startDateTime && c.CreateDate < endDateTime).ToList();
            var model = new HistoryPayVM();
            var cachOrder = 0;//nagd satis sayi
            var crediteOrder = 0; // kreditin sayi
            double? Debt = 0;// borc
            double cachMany = 0;// nagd satisdan gelen pulu
            // GeneralMany faktiki  pul olmalidi umumi satisdan
            double? CrediteMany = 0;// kredit ve ilkin odenis
            double? SaleMany = 0;// umumi  pul hamsi
            int income = 0;

            foreach (var item in order)
            {
                if (!item.IsCredite)
                {
                    cachOrder++;
                    cachMany += item.TotalPrice;
                   
                }
                else
                {
                    crediteOrder++;
                    Debt += item.Debt;
                }
                SaleMany += item.TotalPrice;
                if(item.Cost>0)
                income += Convert.ToInt32(item.Price) - Convert.ToInt32(item.Cost);
            }
            foreach (var item in crediteHistory)
            {
                CrediteMany += item.CachMany;
            }
            model.Debt = Debt;
            model.CachOrder = cachOrder;
            model.SaleMany = SaleMany;
            model.CachMany = cachMany;
            model.CrediteOrder = crediteOrder;
            model.GeneralMany = SaleMany-Debt;
            model.OrderCount = cachOrder + crediteOrder;
            model.CrediteMany = CrediteMany;
            model.Income = income;

            return Json(new { status = "success", data = model });
        }
    }
}
