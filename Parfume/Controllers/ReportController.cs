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
            double? neededMany = 0;//fix ayliq  ile ayliq odenilen pulun ferqi
          
            if (String.IsNullOrEmpty(dateRange))
            {
                dateRange = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (dateRange.Contains("Invalid date - Invalid date") || dateRange.Contains("Hamısı"))
            {
                dateRange = DateTime.Now.AddYears(-100).ToString("dd/MM/yyyy") + "-" + DateTime.Now.AddYears(10).ToString("dd/MM/yyyy");
               // dateRange = SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy") + "-" + SqlDateTime.MaxValue.Value.ToString("dd/MM/yyyy");
            }
              if (!String.IsNullOrEmpty(dateRange))
            {
                startDateTime = DateTime.ParseExact(dateRange.Split('-')[0].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                endDateTime = DateTime.ParseExact(dateRange.Split('-')[1].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            var order = _context.Orders.Where(c => c.CreateDate >= startDateTime && c.CreateDate <= endDateTime).ToList();
            var paymentHistory = _context.PaymentHistories.Where(c => c.PayDate >= startDateTime && c.PayDate <= endDateTime).ToList();

            foreach (var item in paymentHistory)
            {
                if (item.Status)
                {
                    CrediteMany += item.PayPrice;
                    difference += Convert.ToDouble(item.PayPrice - item.MonthPrice);
                }
                else
                {
                    Important += item.MonthPrice;
                }
                neededMany += item.MonthPrice;
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
             
            model.Debt = Important;
            model.Balans = Balans;
            model.CachMany = CachMany;
            model.CrediteMany = CrediteMany;
            model.Important = neededMany;
            return Json(new { status = "success", data = model });
        }

        public IActionResult ReportCreate()
        {
            return View();
        }

        public IActionResult ReportCreateNew()
        {
            ViewBag.Time = SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy");
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


        [HttpPost]
        public IActionResult ReportCreateNew(string dateRange)
        {

            try
            {
                DateTime startDateTime = DateTime.MinValue;
                DateTime endDateTime = DateTime.Now;
                var model = new HistoryPayVM();
                var cachOrder = 0;//nagd satis sayi
                var crediteOrder = 0; // kreditin sayi
                double? Debt = 0;// borc
                double cachMany = 0;// nagd satisdan gelen pulu
                                    // GeneralMany faktiki  pul olmalidi umumi satisdan
                double? CrediteMany = 0;// kredit ve ilkin odenis
                double? SaleMany = 0;// umumi  pul hamsi
                double? TotalCredite = 0;// umumi  pul hamsi
                int income = 0;
                int neededMany = 0;
                double? GeneralBalans = 0;
                double? FirstBalans = 0;
                int expense = 0;
                double bonus = 0;
                var order = new List<Order>();
                var crediteHistory = new List<CrediteHistory>(); ;
                var PaymentHistory = new List<PaymentHistory>();
                var ayliqOdenisler = new List<Order>();
                var PaymentHistoryPay = new List<PaymentHistory>(); 
                var ExpenseHistory = new List<Expense>(); 

                if (dateRange.Contains("Invalid date - Invalid date") || dateRange.Contains("Hamısı"))
                {
                      order = _context.Orders.ToList();
                      crediteHistory = _context.CrediteHistories.ToList();
                      PaymentHistory = _context.PaymentHistories.Where(c =>c.Status == false).ToList();
                     ayliqOdenisler = _context.Orders.Where(c => c.Debt != 0).ToList();
                      PaymentHistoryPay = _context.PaymentHistories.Where(c =>c.Status == true).ToList();
                    ExpenseHistory = _context.Expenses.ToList();
                }
                else
                {
                    if (String.IsNullOrEmpty(dateRange))
                    {
                        dateRange = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    if (!String.IsNullOrEmpty(dateRange))
                    {


                        _context.SaveChanges();
                        startDateTime = DateTime.ParseExact(dateRange.Split('-')[0].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        endDateTime = DateTime.ParseExact(dateRange.Split('-')[1].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                      order = _context.Orders.Where(c => c.CreateDate.Date >= startDateTime.Date && c.CreateDate.Date <= endDateTime.Date).ToList();
                      crediteHistory = _context.CrediteHistories.Where(c => c.CreateDate.Date >= startDateTime.Date && c.CreateDate.Date <= endDateTime.Date).ToList();
                      PaymentHistory = _context.PaymentHistories.Where(c => c.PaymentDate.Value.Date >= startDateTime.Date && c.PaymentDate.Value.Date <= endDateTime.Date && c.Status == false && c.Order.Debt!=0).ToList();
                      PaymentHistoryPay = _context.PaymentHistories.Where(c => c.PayDate.Value.Date >= startDateTime.Date && c.PayDate.Value.Date <= endDateTime.Date && c.Status == true).ToList();
                     ExpenseHistory = _context.Expenses.Where(c => c.CreateDate.Date >= startDateTime.Date && c.CreateDate.Date <= endDateTime.Date).ToList();
                    ayliqOdenisler = _context.Orders.Where(c => c.PaymentDate.Value.Date >= startDateTime.Date && c.PaymentDate.Value.Date <= endDateTime.Date && c.Debt != 0 ).ToList();
                }

                foreach (var item in order)
                {
                    FirstBalans += item.FirstPrice ?? 0;
                    bonus += item.BonusPrice ?? 0;
                    if (!item.IsCredite)
                    {
                        cachOrder+=item.Amount;
                        cachMany += item.TotalPrice;
                        Debt += item.Debt ?? 0;
                    }
                    else
                    {
                        crediteOrder+=item.Amount;
                        Debt += item.Debt ?? 0;
                        
                    }
                    

                    SaleMany += item.TotalPrice;
                    if (item.Cost > 0)
                        income += (Convert.ToInt32(item.Price) - Convert.ToInt32(item.Cost)) * item.Amount;
                }
                foreach (var item in PaymentHistoryPay)
                {
                    CrediteMany += item.PayPrice;
                }
                foreach (var item in ayliqOdenisler)
                {
                    neededMany += (int)item.MonthPrice;
                }

                foreach (var item in ExpenseHistory)
                {
                    expense += item.Money;
                }
                GeneralBalans = CrediteMany + cachMany + FirstBalans;
                model.Debt = Debt;
                //model.Debt = SaleMany - GeneralBalans;
                model.CachOrder = cachOrder;
                model.SaleMany = SaleMany;
                model.CachMany = cachMany;
                model.CrediteOrder = crediteOrder;
                model.GeneralMany = SaleMany - cachMany;
                model.OrderCount = cachOrder + crediteOrder;
                model.CrediteMany = CrediteMany;
                model.Income = income;
                model.NeededMany = neededMany;
                model.GeneralBalans = GeneralBalans;
                model.FirstBalans = FirstBalans;
                model.Expense = expense;
                model.Bonus = bonus;

                return Json(new { status = "success", data = model });
            }
            catch (Exception ex)
            {

                _context.Logs.Add(new Log()
                {
                    BrowserInfo = "",
                    RemoteIpAddress = ex.Message.ToString(),
                    Error = ex.InnerException.Message.ToString(),
                    Fincode = "",
                    Success = false,
                    Type = 1,
                    Url = "ReportNew"
                }); 
                _context.SaveChanges();
                return Json(new { status = "error" });
            }
           
        }

        public IActionResult ReportState()
        {
            ViewBag.Time = SqlDateTime.MinValue.Value.ToString("dd/MM/yyyy");
            return View();
        }
        [HttpPost]
        public IActionResult ReportState(string dateRange)
        {
            try
            {
                DateTime startDateTime = DateTime.MinValue;
                DateTime endDateTime =DateTime.Now.AddYears(1);
                var model = new HistoryPayVM();
                var cachOrder = 0;//nagd satis sayi
                var crediteOrder = 0; // kreditin sayi
                double? Debt = 0;// borc
                double cachMany = 0;// nagd satisdan gelen pulu
                                    // GeneralMany faktiki  pul olmalidi umumi satisdan
                double? CrediteMany = 0;// kredit ve ilkin odenis
                double? SaleMany = 0;// umumi  pul hamsi
                double? TotalCredite = 0;// umumi  pul hamsi
                int income = 0;
                int neededMany = 0;
                double? GeneralBalans = 0;
                double? FirstBalans = 0;
                int expense = 0;
                double bonus = 0;
                var order = new List<Order>();
                var crediteHistory = new List<CrediteHistory>(); ;
                var PaymentHistory = new List<PaymentHistory>();
                var ayliqOdenisler = new List<Order>();
                var PaymentHistoryPay = new List<PaymentHistory>();
                var ExpenseHistory = new List<Expense>();

                 

                if (!String.IsNullOrEmpty(dateRange))
                {
                    _context.SaveChanges();
                    endDateTime = DateTime.ParseExact(dateRange.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                
                order = _context.Orders.Where(c => c.CreateDate.Date <= endDateTime.Date).ToList();
                crediteHistory = _context.CrediteHistories.Where(c => c.CreateDate.Date <= endDateTime.Date).ToList();
                PaymentHistory = _context.PaymentHistories.Where(c => c.PaymentDate.Value.Date <= endDateTime.Date && c.Status == false && c.Order.Debt != 0).ToList();
                PaymentHistoryPay = _context.PaymentHistories.Where(c => c.PayDate.Value.Date <= endDateTime.Date && c.Status == true).ToList();
                ExpenseHistory = _context.Expenses.Where(c => c.CreateDate.Date <= endDateTime.Date).ToList();
                ayliqOdenisler = _context.Orders.Where(c => c.PaymentDate.Value.Date <= endDateTime.Date && c.Debt != 0).ToList();

                foreach (var item in order)
                {
                    FirstBalans += item.FirstPrice ?? 0;
                    bonus += item.BonusPrice ?? 0;
                    if (!item.IsCredite)
                    {
                        cachOrder += item.Amount;
                        cachMany += item.TotalPrice;
                        Debt += item.Debt ?? 0;
                    }
                    else
                    {
                        crediteOrder += item.Amount;
                        Debt += item.Debt ?? 0;

                    }


                    SaleMany += item.TotalPrice;
                    if (item.Cost > 0)
                        income += (Convert.ToInt32(item.Price) - Convert.ToInt32(item.Cost)) * item.Amount;
                }
                foreach (var item in PaymentHistoryPay)
                {
                    CrediteMany += item.PayPrice;
                }
                foreach (var item in ayliqOdenisler)
                {
                    neededMany += (int)item.MonthPrice;
                }

                foreach (var item in ExpenseHistory)
                {
                    expense += item.Money;
                }
                GeneralBalans = CrediteMany + cachMany + FirstBalans;
                model.Debt = Debt;
                //model.Debt = SaleMany - GeneralBalans;
                model.CachOrder = cachOrder;
                model.SaleMany = SaleMany;
                model.CachMany = cachMany;
                model.CrediteOrder = crediteOrder;
                model.GeneralMany = SaleMany - cachMany;
                model.OrderCount = cachOrder + crediteOrder;
                model.CrediteMany = CrediteMany;
                model.Income = income;
                model.NeededMany = neededMany;
                model.GeneralBalans = GeneralBalans;
                model.FirstBalans = FirstBalans;
                model.Expense = expense;
                model.Bonus = bonus;

                return Json(new { status = "success", data = model });
            }
            catch (Exception ex)
            {

                _context.Logs.Add(new Log()
                {
                    BrowserInfo = "",
                    RemoteIpAddress = ex.Message.ToString(),
                    Error = ex.InnerException.Message.ToString(),
                    Fincode = "",
                    Success = false,
                    Type = 1,
                    Url = "ReportNew"
                });
                _context.SaveChanges();
                return Json(new { status = "error" });
            }
        }

    }
}
