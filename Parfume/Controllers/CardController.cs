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
        public IActionResult CardInfo(int cardId)
        {

            var model = _context.Cards.Where(c =>c.Id == cardId).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public JsonResult ChangeCard(string cardName, int cardId)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                var cardOldName = "";
                if (  _context.Cards.Any(c => c.Id == cardId))
                {
                    var card = _context.Cards.Where(c => c.Id == cardId).FirstOrDefault();
                    cardOldName = card.Name;
                    card.Name = cardName;
                    _context.Cards.Update(card);
                    _context.SaveChanges();

                    _context.Logs.Add(new Log()
                    {
                        Error = $"ugurlu cardin  adi deyisdi kohne ad:{cardOldName}  cardId:{cardId}",
                        UserId = UserId,
                        Success = true,
                        Type = 1,
                        Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                    });
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
                }
              
                return Json(new { status = "error", message = "Kard tapilmadi" });


            }
            catch (Exception ex)
            {
                _context.Logs.Add(new Log()
                {
                    BrowserInfo = ex.InnerException?.ToString(),
                    Error = $" Xeta bas verdi kart adini deyismek  ,   cardId:{cardId}",
                    UserId = UserId,
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
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
                var t = _context.Cards.Any(c => c.Name != CardName);
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
                var orders = _context.Orders.Where(c => c.CustomerId == customerId && c.Status==2 && c.IsCredite==true);
                int limitCard = 0;
                foreach (var item in orders)
                {
                    item.CardId = cardId;
                    limitCard += Convert.ToInt32(item.MonthPrice);
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

                    _context.Logs.Add(new Log()
                    {
                        Error =  $"ugurlu card add limit:{limitCard} , custmoreId={customerId} cardId:{cardId}",
                        UserId = UserId,
                        Success = true,
                        Type = 1,
                        Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                    });
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
                }
                return Json(new { status = "error", message = "İstifadəci tapilmadi" });
               

            }
            catch (Exception ex )
            {
                _context.Logs.Add(new Log()
                {
                    BrowserInfo=ex.InnerException?.ToString(),
                    Error = $" Xeta bas verdi kart elave edende, custmoreId={CustomerId} cardId:{cardId}",
                    UserId = UserId,
                    Success = true,
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

                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }
        [HttpPost]
        public JsonResult ChangeCardCustomer(int customerId, int newCardId)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                if (_context.Customers.Any(c => c.Id == customerId))
                {
                    var customer = _context.Customers.Where(c => c.Id == customerId).First();
                    var cardId = customer.CardId;
                    var cardDb = _context.Cards.Where(c => c.Id == cardId).First();
                    var NewcardDb = _context.Cards.Where(c => c.Id == newCardId).First();
                    var orders = _context.Orders.Where(c => c.CustomerId == customerId && c.Debt > 0);
                    var limit = 0;
                    customer.CardId = newCardId;
                    _context.Customers.Update(customer);
                    foreach (var item in orders )
                    {
                        item.CardId = newCardId;
                        limit +=(int) item.MonthPrice;
                         
                    }
                    _context.Orders.UpdateRange(orders);

                    
                     
                    cardDb.Limit -= limit;
                    NewcardDb.Limit += limit;
                    _context.Cards.Update(cardDb);
                    _context.Cards.Update(NewcardDb);
                    _context.SaveChanges();

                    _context.Logs.Add(new Log()
                    {
                        Error =   $"Change card success. oldKard:{cardId}, newKart:{newCardId} "+DateTime.Now.ToString(),
                        UserId = UserId,
                        Success = true,
                        Type = 1,
                        Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                    });
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
                }
                return Json(new { status = "error", message = "Kart tapilmadi" });


            }
            catch (Exception  ex)
            {
                _context.Logs.Add(new Log()
                {
                    BrowserInfo = ex.InnerException?.ToString(),
                    Error = $" Xeta bas verdi kart elave edende, custmoreId={customerId} newCardId:{newCardId}",
                    UserId = UserId,
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }

        [HttpGet]
        public IActionResult ChangeCard()
        {
            var model = _context.Customers.Where(c => c.IsActive && c.CardId != null ).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult ChangeCustomerCard(int customerId) 
        {
            ViewBag.CustomerId = customerId;
            var model = _context.Cards.Where(c => c.Limit<4000 && c.Active).ToList();

            return View(model);
}
    }
}
