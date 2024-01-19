using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using Parfume.Models;
using Parfume.Service;

namespace Parfume.Controllers
{
    [Authorize]
    public class BonusCardController : Controller
    {
        private readonly ParfumeContext _context;
        private readonly IUserService _userService;
        public BonusCardController(ParfumeContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public IActionResult Index()
        {
            var model = _context.BonusCards.Where(c => c.IsActive).Include(c=>c.BonusCardType).Include(c => c.Customer).ToList();
            return View(model);
        }
        public IActionResult UseBonusCard()
        {
            var model = _context.BonusCards.Where(c => c.IsActive!=true).Include(c => c.BonusCardType).Include(c => c.Customer).ToList();
            return View(model);
        }
        public IActionResult History()
        {
            var model = _context.BonusCardHistories
                .Include(c => c.BonusCard)
                .Include(c=>c.Order)
                .ThenInclude(c=>c.Product)
                .Include(c => c.Customer).ToList();
            return View(model);
        }
        public IActionResult CreateBonusCard()
        {
            var model = _context.BonusCardTypes.ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult CreateBonusCard(string cardNumber, string CustomerId,string name,string surname,string fatherName,
            string baseNumber,string fincode,int cardTypeId,int balans)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                var ctsId = 0;
                 
                if (fincode.Contains('/'))
                {
                    fincode = fincode.Split('/')[1].ToString();
                }
                if (!CustomerId.Contains("new"))
                {
                    ctsId = Int32.Parse(CustomerId);
                }
                var customerId = 0;
                if (_context.Customers.Any(c => c.Id == ctsId))
                {
                    var customerDb = _context.Customers.Where(c => c.Id == ctsId).FirstOrDefault();
                    customerId = ctsId;
                }
                else if (!_context.Customers.Any(c => c.Fincode.ToUpper() == fincode.Trim().ToUpper() && c.IsActive && !c.IsBlock))
                {
                    var customer = new Customer()
                    {
                         
                        BaseNumber = baseNumber,
                        FatherName = fatherName,
                        Fincode = fincode.Trim().ToUpper(),
                        Name = name,
                        Surname = surname,
                        BonusAmount = 0
                    };
                    _context.Customers.Add(customer).GetDatabaseValues();
                    _context.SaveChanges();
                    customerId = customer.Id;
                }
                else
                {
                    customerId = _context.Customers.Where(c => c.Fincode.ToUpper() == fincode.Trim().ToUpper() && c.IsActive && !c.IsBlock).FirstOrDefault().Id;
                }

                if (!_context.BonusCards.Any(c => c.CardNumber.Equals(cardNumber)  ))
                {
                    _context.BonusCards.Add(new BonusCard()
                    {
                         CustomerId= customerId,
                          BonusCardTypeId=cardTypeId,
                          CardNumber=cardNumber,
                          Balans=balans
                    });
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
                }
                return Json(new { status = "error", message = "Kart nömrəsi istifadə olunub!" });


            }
            catch (Exception ex)
            {

                _context.Logs.Add(new Log()
                {
                    Error = ex.Message ?? "Create card error.",
                    UserId = UserId,
                    Success = false,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }

        public JsonResult DeleteBonusCard(int id)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                if (_context.BonusCards.Any(c => c.Id == id))
                {
                    var cardDb = _context.BonusCards.Where(c => c.Id == id).First();
                    cardDb.IsActive = false;
                    _context.BonusCards.Update(cardDb);
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

        public IActionResult HistoryByOrder(int id)
        {
            var model = _context.Orders.Where(c => c.BonusCardId == id).ToList();
            return View(model);
        }

        public JsonResult BonusCardFilter(string search, int page, string blackList,
                         bool selfAccess = false, bool fullAccess = false, bool isDelegation = false, bool selfInner = false)
        {
            try
            {
                int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
                 

                if (!String.IsNullOrEmpty(search))
                {
                    search = search.Trim().Replace(" ", "");
                }
                List<int> _blackList = new List<int>();
                if (!string.IsNullOrEmpty(blackList))
                {
                    _blackList = blackList.Split(',').Select(Int32.Parse).ToList();
                }


                (IEnumerable<BonusCardModel> bonusCards, int rowCount) employeesAndRowCount = _userService.GetBonusCard(page, 100, search);

                IEnumerable<BonusCardModel> bonusCards =
                     employeesAndRowCount.bonusCards
                     .Select(e => new BonusCardModel
                     {
                         Id = e.Id,
                         Amount=e.Amount
                     }).ToList();

                List<Select2Result> results = bonusCards.Select(u =>
                new Select2Result
                {
                    id = u.Id.ToString(),
                    Amount = u.Amount
                      
                }).ToList();
                return Json(new { results = results, count_filtered = employeesAndRowCount.rowCount });
            }
            catch (Exception ex)
            {

                _context.Logs.Add(new Log()
                {
                    Error = $"bonus card get error: {ex.Message} ",
                    Success = false,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                throw;
            }

        }

    }
}
