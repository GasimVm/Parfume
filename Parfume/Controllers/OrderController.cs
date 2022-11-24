using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parfume.DAL;
using Parfume.Models;
using Parfume.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parfume.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ParfumeContext _context;
        private readonly ICreatePdfService _createPdfService;
        public OrderController(ParfumeContext context, ICreatePdfService createPdfService)
        {
            _context = context;
            _createPdfService = createPdfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreateOrder(string name, string surname, string duration, string fatherName, string baseNumber, string fincode, string firstName, string firstNumber, string secondName,
            string secondNumber, string thirdName, string thirdNumber, string quantity, string price, string firstPrice, string amount, double monthlyPayment, string productName, string totalPrice, string address,
            string workAddress, string InstagramAddress, string CustomerId, string dateCreate, string WhoIsOkey, int cost,string dateBirth,int cardId)
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

                if (_context.Customers.Any(c => c.Fincode.ToUpper() == fincode.Trim().ToUpper() && c.IsBlock))
                {
                    return Json(new { status = "error", message = "Bu şəxs qara siyahıdadır!" });
                }

                var format = "dd/MM/yyyy";
                CultureInfo provider = CultureInfo.InvariantCulture;
                var debt = Convert.ToDouble(totalPrice) - Convert.ToDouble(firstPrice);
                monthlyPayment = Math.Round(debt/Convert.ToDouble(duration));
                var CreateDate = DateTime.Now;
                var BirthDate = DateTime.Now;
                if (dateCreate != null)
                {
                    CreateDate = DateTime.ParseExact(dateCreate, format, provider);
                }
                if (dateBirth!=null)
                {
                    BirthDate = DateTime.ParseExact(dateBirth, format, provider);
                }
                var customerId = 0;
                var productId = 0;
                if (_context.Customers.Any(c => c.Id == ctsId))
                {
                    var customerDb = _context.Customers.Where(c => c.Id == ctsId).FirstOrDefault();
                    customerDb.Address = address;
                    customerDb.BaseNumber = baseNumber;
                    customerDb.FatherName = fatherName;
                    customerDb.Fincode = fincode.Trim().ToUpper();
                    customerDb.FirstNumber = firstNumber;
                    customerDb.FirstNumberWho = firstName;
                    customerDb.InstagramAddress = InstagramAddress;
                    customerDb.Name = name;
                    customerDb.WhoIsOkey = WhoIsOkey;
                    customerDb.SecondNumber = secondNumber;
                    customerDb.SecondNumberWho = secondName;
                    customerDb.Surname = surname;
                    customerDb.ThirdNumber = thirdNumber;
                    customerDb.ThirdNumberWho = thirdName;
                    customerDb.WorkAddress = workAddress;
                    customerDb.Birthday = BirthDate;
                    customerDb.CardId = cardId;
                    _context.Customers.Update(customerDb);
                    _context.SaveChanges();
                    customerId = ctsId;
                }
                else if (!_context.Customers.Any(c => c.Fincode.ToUpper() == fincode.Trim().ToUpper() && c.IsActive && !c.IsBlock))
                {
                    var customer = new Customer()
                    {
                        Address = address,
                        BaseNumber = baseNumber,
                        FatherName = fatherName,
                        Fincode = fincode.Trim().ToUpper(),
                        FirstNumber = firstNumber,
                        FirstNumberWho = firstName,
                        InstagramAddress = InstagramAddress,
                        Name = name,
                        WhoIsOkey = WhoIsOkey,
                        SecondNumber = secondNumber,
                        SecondNumberWho = secondName,
                        Surname = surname,
                        ThirdNumber = thirdNumber,
                        ThirdNumberWho = thirdName,
                        WorkAddress = workAddress,
                        Birthday = BirthDate,
                        CardId=cardId
                    };
                    _context.Customers.Add(customer).GetDatabaseValues();
                    _context.SaveChanges();
                    customerId = customer.Id;
                }
                else
                {
                    customerId = _context.Customers.Where(c => c.Fincode.ToUpper() == fincode.Trim().ToUpper() && c.IsActive && !c.IsBlock).FirstOrDefault().Id;
                }


                if (!_context.Products.Any(c => c.Name == productName))
                {
                    var product = new Product()
                    {
                        Name = productName,
                        Quantity = quantity

                    };
                    _context.Products.Add(product).GetDatabaseValues();
                    _context.SaveChanges();
                    productId = product.Id;
                }
                else
                {
                    productId = _context.Products.Where(c => c.Name == productName).FirstOrDefault().Id;
                }
                 
                var order = new Order()
                { 
                    Amount = Convert.ToInt32(amount),
                    CustomerId = customerId,
                    Debt = debt,
                    IsCredite = true,
                    MonthPrice = monthlyPayment  ,
                    Name = productName,
                    Quantity = quantity,
                    Duration = Convert.ToInt32(duration),
                    PaymentDate = CreateDate.AddMonths(1),
                    Price = Convert.ToDouble(price),
                    Cost = cost,
                    FirstPrice = Convert.ToDouble(firstPrice),
                    ProductId = productId,
                    TotalPrice = Convert.ToDouble(totalPrice),
                    UserId = UserId,
                    CreateDate = CreateDate,
                    Status = 2,
                    StatusNotification = 1,
                    CardId=cardId
                };
                
                _context.Orders.Add(order).GetDatabaseValues();
                _context.SaveChanges();

                var paymentHistory = new List<PaymentHistory>();
                for (int i = 1; i <= Convert.ToInt32(duration); i++)
                {
                    if (i== Convert.ToInt32(duration))
                    {
                        paymentHistory.Add(new PaymentHistory()
                        {
                            CustomerId = customerId,
                            OrderId = order.Id,
                            MonthPrice = debt- monthlyPayment*(i-1),
                            Status = false,
                            Queue = i,
                            Debt = debt,
                            PaymentDate = CreateDate.AddMonths(i),
                            PayDate = CreateDate.AddMonths(i)

                        });
                    }
                    else
                    {
                        paymentHistory.Add(new PaymentHistory()
                        {
                            CustomerId = customerId,
                            OrderId = order.Id,
                            MonthPrice = monthlyPayment  ,
                            Status = false,
                            Queue = i,
                            Debt = debt,
                            PaymentDate = CreateDate.AddMonths(i),
                            PayDate = CreateDate.AddMonths(i)

                        });
                    }
                   

                }
                _context.PaymentHistories.AddRange(paymentHistory);
                _context.SaveChanges();

                //var html = _createPdfService.CreateHTML(order.Id);
                //var css = _createPdfService.CreateCSS();
                //_createPdfService.CreatePdf(css,html, order.Id);
                var cardDb = _context.Cards.Where(c => c.Id == cardId).First();
                cardDb.Limit += (int)monthlyPayment;
                _context.Cards.Update(cardDb);
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

        [HttpPost]
        public JsonResult CreateOrderCash(string name, string surname, string fatherName, string baseNumber, string fincode,
             string quantity, string price, string amount, string productName, string totalPrice,
             string InstagramAddress, string CustomerId, string dateCreate, string cost, string dateBirth)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

            try
            {
                fincode = fincode ?? "";

                if (fincode.Contains('/'))
                {
                    fincode = fincode.Split('/')[1].ToString();
                }
                var customerId = 0;
                var productId = 0;
                var ctsId = 0;
                var CustomerFin = "";
                var format = "dd/MM/yyyy";
                CultureInfo provider = CultureInfo.InvariantCulture;
                var CreateDate = DateTime.Now;
                DateTime? BirthDate =null;
                if (dateCreate != null)
                {
                    CreateDate = DateTime.ParseExact(dateCreate, format, provider);

                }
                if (dateBirth!=null)
                {
                    BirthDate = DateTime.ParseExact(dateBirth, format, provider);

                }
                if (!CustomerId.Contains("new") && CustomerId != "null")
                {
                    ctsId = Int32.Parse(CustomerId);
                }
                if (fincode != null && fincode != "")
                {
                    CustomerFin = fincode.Trim().ToUpper();
                }
                if (_context.Customers.Any(c => c.Id == ctsId))
                {
                    var customerDb = _context.Customers.Where(c => c.Id == ctsId).FirstOrDefault();
                    customerDb.BaseNumber = baseNumber;
                    customerDb.FatherName = fatherName;
                    customerDb.Fincode = CustomerFin;
                    customerDb.InstagramAddress = InstagramAddress;
                    customerDb.Name = name;
                    customerDb.Surname = surname;
                    customerDb.Birthday = BirthDate;
                    _context.Customers.Update(customerDb);
                    _context.SaveChanges();
                    customerId = ctsId;
                }
                else if (!_context.Customers.Any(c => c.Fincode.ToUpper() == CustomerFin && c.IsActive && !c.IsBlock) || CustomerFin == "")
                {
                    var customer = new Customer()
                    {
                        BaseNumber = baseNumber,
                        FatherName = fatherName,
                        Fincode = CustomerFin,
                        InstagramAddress = InstagramAddress,
                        Name = name,
                        Surname = surname,
                        Birthday = BirthDate
                    };
                    _context.Customers.Add(customer).GetDatabaseValues();
                    _context.SaveChanges();
                    customerId = customer.Id;
                }
                else
                {
                    customerId = _context.Customers.Where(c => c.Fincode.ToUpper() == CustomerFin && c.IsActive && !c.IsBlock).FirstOrDefault().Id;
                }

                if (!_context.Products.Any(c => c.Name == productName))
                {
                    var product = new Product()
                    {
                        Name = productName,
                        Quantity = quantity

                    };
                    _context.Products.Add(product).GetDatabaseValues();
                    _context.SaveChanges();
                    productId = product.Id;
                }
                else
                {
                    productId = _context.Products.Where(c => c.Name == productName).FirstOrDefault().Id;
                }
                var order = new Order()
                {
                    Amount = Convert.ToInt32(amount),
                    CustomerId = customerId,
                    Debt = 0,
                    IsCredite = false,
                    Name = productName,
                    Quantity = quantity,
                    Price = Convert.ToDouble(price),
                    ProductId = productId,
                    Cost = Convert.ToInt32(cost),
                    TotalPrice = Convert.ToDouble(totalPrice),
                    CreateDate = CreateDate,
                    UserId = UserId,
                    Status = 1
                };

                _context.Orders.Add(order);
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
        [HttpPost]
        public JsonResult PayOrder(string OrderId, string price, string note, string dateCreate)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

            try
            {
                var format = "dd/MM/yyyy";
                CultureInfo provider = CultureInfo.InvariantCulture;
                var CreateDate = DateTime.Now;
                if (dateCreate != null)
                {
                    CreateDate = DateTime.ParseExact(dateCreate, format, provider);

                }
                var orderId = Convert.ToInt32(OrderId);
                var Price = Convert.ToDouble(price);

                var order = _context.Orders.Where(c => c.Id == orderId).FirstOrDefault();
                order.Debt = order.Debt - Price;
                order.StatusNotification = 1;
                order.PaymentDate = order.PaymentDate?.AddMonths(1);
                _context.Orders.Update(order);
                _context.SaveChanges();
                if (order.Debt == 0)
                {
                   var cardDb= _context.Cards.Where(c => c.Id == order.CardId).First();
                    cardDb.Limit= cardDb.Limit - (int)order.MonthPrice;
                    order.Status = 1;
                    order.StatusNotification = 2;
                    _context.Cards.Update(cardDb);
                    _context.SaveChanges();
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                }
                if (_context.PaymentHistories.Any(c => c.OrderId == orderId && c.Status == false))
                {
                    var paymentHistory = _context.PaymentHistories.Where(c => c.OrderId == orderId && c.Status == false).FirstOrDefault();
                    paymentHistory.Status = true;
                    paymentHistory.Note = note;
                    paymentHistory.PayPrice = Price;
                    paymentHistory.PayDate = CreateDate;
                    paymentHistory.Debt = Convert.ToDouble(order.Debt);
                    _context.PaymentHistories.Update(paymentHistory);

                    var crediteHistory = new CrediteHistory()
                    {
                        CachMany = Price,
                        UserId = UserId,
                        Note = note,
                        OrderId = orderId,
                        PaymentHistoryId = paymentHistory.Id
                    };
                    _context.CrediteHistories.Add(crediteHistory);
                    _context.SaveChanges();
                }
                else
                {
                    var paymentHistory = _context.PaymentHistories.Where(c => c.OrderId == orderId).FirstOrDefault();
                    _context.PaymentHistories.Add(new PaymentHistory()
                    {
                        CustomerId = paymentHistory.CustomerId,
                        MonthPrice = paymentHistory.MonthPrice,
                        PayPrice = Price,
                        Note = note,
                        Status = true,
                        Queue = paymentHistory.Queue + 1,
                        Debt = Convert.ToDouble(order.Debt),
                        PaymentDate = CreateDate,
                        PayDate = CreateDate,
                        OrderId = orderId
                    });
                    _context.SaveChanges();
                }

                _context.Logs.Add(new Log()
                {
                    Error = "Ugurlu odenis miqdar" + price,
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
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
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = false,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }

        [HttpPost]
        public JsonResult ChangePaymentDate(string OrderId, int changeDay)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

            try
            {
                var orderId = Convert.ToInt32(OrderId);
                var order = _context.Orders.Where(c => c.Id == orderId).FirstOrDefault();
                order.PaymentDate = order.PaymentDate?.AddDays(changeDay);
                order.StatusNotification = 1;
                _context.Orders.Update(order);
                _context.SaveChanges();

                _context.Logs.Add(new Log()
                {
                    Error = "ChangePaymentDate day number" + changeDay.ToString(),
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
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
        
             [HttpPost]
        public JsonResult ChangeCost(string OrderId, int changeCost)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

            try
            {
                var orderId = Convert.ToInt32(OrderId);
                var order = _context.Orders.Where(c => c.Id == orderId).FirstOrDefault();
                var orderCost = order.Cost;
                order.Cost = changeCost;
              
                _context.Orders.Update(order);
                _context.SaveChanges();

                _context.Logs.Add(new Log()
                {
                    Error = "new cost" + changeCost.ToString()+" old cost:"+ orderCost,
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception ex)
            {
                _context.Logs.Add(new Log()
                {
                    Error = ex.Message ?? "new cost",
                    UserId = UserId,
                    Success = false,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }
        public ActionResult ChangePaymentPay(int payHistoryId)
        {
            var payHistory = _context.PaymentHistories.Where(c => c.Id == payHistoryId).First();
            ViewBag.Many = payHistory.PayPrice;
            ViewBag.PayHistoryId = payHistoryId;
            ViewBag.OrderId = payHistory.OrderId;
            return View();
        }

        [HttpPost]
        public JsonResult ChangePaymentPay(string OrderId, int newMany, int payHistoryId)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                var orderId = Convert.ToInt32(OrderId);
                var order = _context.Orders.Where(c => c.Id == orderId).First();
                var payHistory = _context.PaymentHistories.Where(c => c.Id == payHistoryId).First();

                if (_context.CrediteHistories.Any(c => c.PaymentHistoryId == payHistoryId))
                {
                    var crediteHistory = _context.CrediteHistories.Where(c => c.PaymentHistoryId == payHistoryId).First();
                    crediteHistory.CachMany = newMany;
                    _context.CrediteHistories.Update(crediteHistory);
                    _context.SaveChanges();
                }
                order.Debt += (payHistory.PayPrice - newMany);
                _context.Orders.Update(order);
                _context.SaveChanges();
                payHistory.PayPrice = newMany;
                payHistory.Debt = (double)order.Debt;
                _context.PaymentHistories.Update(payHistory);
                _context.SaveChanges();
                _context.Logs.Add(new Log()
                {
                    Error = "odenisin miqdarinini deyismek yeni miqdar" + newMany.ToString(),
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
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

        [HttpPost]
        public JsonResult DeletePaymentPay(string OrderId, int payHistoryId)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                var orderId = Convert.ToInt32(OrderId);
                var order = _context.Orders.Where(c => c.Id == orderId).First();
                var payHistory = _context.PaymentHistories.Where(c => c.Id == payHistoryId).First();
                var newMany = payHistory.PayPrice;
                if (_context.CrediteHistories.Any(c => c.PaymentHistoryId == payHistoryId))
                {
                    var crediteHistory = _context.CrediteHistories.Where(c => c.PaymentHistoryId == payHistoryId).First();
                    _context.CrediteHistories.Remove(crediteHistory);
                    _context.SaveChanges();
                }
                order.Debt += payHistory.PayPrice;
                order.Status = 2;
                order.PaymentDate = order.PaymentDate?.AddMonths(-1);
                _context.Orders.Update(order);
                _context.SaveChanges();
                payHistory.PayPrice = null;
                payHistory.Note = null;
                payHistory.PayDate = payHistory.PaymentDate;
                payHistory.Status = false;
                payHistory.Debt = (double)order.Debt;
                _context.PaymentHistories.Update(payHistory);
                _context.SaveChanges();

                _context.Logs.Add(new Log()
                {
                    Error = "odenisin Silmek  payHistoryId=" + payHistoryId.ToString(),
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 3,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult RemoveOrder(string OrderId)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

            try
            {
                var orderId = Convert.ToInt32(OrderId);
                var Paymenthistory = _context.PaymentHistories.Where(c => c.OrderId == orderId);
                var creadit = _context.CrediteHistories.Where(c => c.OrderId == orderId);
                var order = _context.Orders.Where(c => c.Id == orderId).FirstOrDefault();
                _context.CrediteHistories.RemoveRange(creadit);
                _context.SaveChanges();
                _context.PaymentHistories.RemoveRange(Paymenthistory);
                _context.SaveChanges();
                _context.Orders.Remove(order);
                _context.SaveChanges();

                _context.Logs.Add(new Log()
                {
                    Error = "sifarisi Silmek  OrderId=" + OrderId,
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 3,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
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


    }
}
