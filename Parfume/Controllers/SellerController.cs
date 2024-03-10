using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Controllers
{

    public class SellerController : Controller
    {
        private readonly ParfumeContext _context;
        public SellerController(ParfumeContext context)
        {
            _context = context;
        }
         
        public IActionResult Index()
        {
           var model= _context.Sellers
                .Select(c=> new SellerModel
                { 
                 Name=c.Name,
                 Count=c.SellerByOrders.Count
                }).ToList();

            return View(model);
        }

        public IActionResult Report()
        {
            var model = _context.Sellers.Where(c =>c.IsActive).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Report(string dateRange,int sellerId)
        {
            try
            {
                DateTime startDateTime = DateTime.MinValue;
                DateTime endDateTime = DateTime.Now;
                
                var crediteHistory = new List<Order>();
                if (dateRange.Contains("Invalid date - Invalid date") || dateRange.Contains("Hamısı"))
                {
                    crediteHistory = _context.SellerByOrderHistories.Where(c => c.SellerId==sellerId)
                   .Include(c => c.Order)
                   .Include(c => c.Order).ThenInclude(c => c.Customer)
                   .Include(c => c.Order).ThenInclude(c => c.Product)
                   .OrderBy(c => c.CreateDate)
                   .Select(c => new Order
                   {
                       Amount = c.Order.Amount,
                       Product = c.Order.Product,
                       Customer = c.Order.Customer,
                       TotalPrice = c.Order.TotalPrice,
                       Price = c.Order.Price,
                       Name = c.Order.Name,
                       Id = c.Order.Id,
                       Debt = c.Order.Debt,
                       CreateDate = c.Order.CreateDate
                   })
                   .ToList();
                }
                else
                {
                    if (String.IsNullOrEmpty(dateRange))
                    {
                        dateRange = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy") + "-" + DateTime.Now.ToString("dd/MM/yyyy");
                    }

                    if (!String.IsNullOrEmpty(dateRange))
                    { 
                        startDateTime = DateTime.ParseExact(dateRange.Split('-')[0].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        endDateTime = DateTime.ParseExact(dateRange.Split('-')[1].Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                   var crediteHistory1 = _context.SellerByOrderHistories.Where(c => c.CreateDate.Date >= startDateTime.Date).FirstOrDefault();

                   crediteHistory = _context.SellerByOrderHistories.Where(c =>c.Order.CreateDate.Date >=  startDateTime.Date &&  c.Order.CreateDate <= endDateTime.Date && c.SellerId==sellerId)
                    .Include(c => c.Order)
                    .Include(c => c.Order).ThenInclude(c => c.Customer)
                    .Include(c => c.Order).ThenInclude(c => c.Product)
                    .OrderBy(c => c.CreateDate)
                    .Select(c => new Order
                    {
                        Amount = c.Order.Amount,
                        Product = c.Order.Product,
                        Customer = c.Order.Customer,
                        TotalPrice = c.Order.TotalPrice,
                        Price = c.Order.Price,
                        Name = c.Order.Name,
                        Id = c.Order.Id,
                        Debt = c.Order.Debt,
                        CreateDate = c.Order.CreateDate
                    })
                    .ToList();
                     
                }
                var model = new SellerReportModel();
                model.Orders = crediteHistory;
                foreach (var item in crediteHistory)
                {
                    model.Count += item.Amount;
                }
                return PartialView("_PartialTableSeller", model);

            }
            catch (Exception ex)
            {
                return Json(new { status = "error" });
            }

        }
    }
}
