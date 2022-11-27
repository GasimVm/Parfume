using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parfume.Controllers
{
    [Authorize]
    public class CardController : Controller
    {
        private readonly ParfumeContext _context;
        public CardController(ParfumeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.Cards.Where(c=>c.Active).Include(c => c.Customers).ToList();
            return View(model);
        }

        public IActionResult CardAddCusmoter()
        {
             
               var model = _context.Customers.Where(c => c.IsActive && c.CardId==null).ToList();

            return View(model);
        }
        public IActionResult CardCustomerDetails(int cardId)
        {
               var model = _context.Customers.Where(c => c.IsActive && c.CardId == cardId).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddCard(int customerId)
        {
            ViewBag.Cards = _context.Cards.Where(c => c.Active && c.Limit < 4000);
            var model = _context.Customers.Where(c => c.Id == customerId).First();
            return View(model);
        }

        public IActionResult CreateCard( )
        {
             
            return View();
        }
        [HttpPost]
        public JsonResult CreateCard(string CardName)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                if (_context.Cards.Any(c => c.Name != CardName)  )
                {
                    _context.Cards.Add(new Card()
                    {
                         Active=true,
                          Limit=0,
                          Name=CardName
                    });
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
                }
                return Json(new { status = "error", message = "İstifadəci tapilmadi" });


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
        public JsonResult AddCard(string CustomerId, int cardId)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                var customerId = Convert.ToInt32(CustomerId);
                var orders = _context.Orders.Where(c => c.CustomerId == customerId);
                int limitCard = 0;
                foreach (var item in orders)
                {
                    item.CardId = cardId;
                    limitCard += (int)item.MonthPrice;
                }
                _context.Orders.UpdateRange(orders);
                _context.SaveChanges();
                if (_context.Customers.Any(c => c.Id == customerId) && _context.Cards.Any(c => c.Id == cardId))
                {
                    var customer = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault();
                    var card = _context.Cards.Where(c => c.Id == cardId).FirstOrDefault();
                    card.Limit = card.Limit + limitCard;
                    customer.CardId = cardId;
                    _context.Cards.Update(card);
                    _context.Customers.Update(customer);
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
                }
                return Json(new { status = "error", message = "İstifadəci tapilmadi" });


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

        public JsonResult DeleteCard(int CardId)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                if (_context.Cards.Any(c => c.Id == CardId))
                {
                    var cardDb = _context.Cards.Where(c => c.Id == CardId).First();
                    cardDb.Active = false;
                    _context.Cards.Update(cardDb);
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
                }
                return Json(new { status = "error", message = "Kart tapilmadi" });


            }
            catch (Exception ex)
            {

                _context.Logs.Add(new Log()
                {
                    Error = ex.Message ?? "Delete card error.",
                    UserId = UserId,
                    Success = false,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }
    }
}
