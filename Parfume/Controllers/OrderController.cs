using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using Parfume.Models;
using Parfume.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Parfume.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ParfumeContext _context;
        private readonly ICreatePdfService _createPdfService;
        private readonly IBonusService _bonusService;
        public OrderController(ParfumeContext context, ICreatePdfService createPdfService,IBonusService bonusService)
        {
            _context = context;
            _createPdfService = createPdfService;
            _bonusService = bonusService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CreateOrder(string name, string surname, string duration, string fatherName, string baseNumber, string fincode, string firstName, string firstNumber, string secondName,
            string secondNumber, string thirdName, string thirdNumber, string quantity, string price, string firstPrice, string amount, double monthlyPayment, string productName, string totalPrice, string address,
            string workAddress, string InstagramAddress, string CustomerId, string dateCreate, string WhoIsOkey,
            int cost,string dateBirth,int cardId,int referencesId,string bonusPrice,string hasBonus)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

            try
            {
                var ctsId = 0;
                var IsBonus = Convert.ToBoolean(hasBonus);
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
                var bonusPriceCovert = Convert.ToDouble(bonusPrice);
                 
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
                        CardId=cardId,
                        BonusAmount=0
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
                //customerId
                var order = new Order();
                if (bonusPriceCovert == 0)
                {
                      order = new Order()
                    {
                        Amount = Convert.ToInt32(amount),
                        CustomerId = customerId,
                        Debt = debt,
                        OldDebt = debt,
                        IsCredite = true,
                        MonthPrice = monthlyPayment,
                        Name = productName,
                        Quantity = quantity,
                        Duration = Convert.ToInt32(duration),
                        PaymentDate = CreateDate.AddMonths(1),
                        Price = Convert.ToDouble(price),
                        Cost = cost,
                        HasBonus= IsBonus,
                        FirstPrice = Convert.ToDouble(firstPrice),
                        ProductId = productId,
                        TotalPrice = Convert.ToDouble(totalPrice),
                        UserId = UserId,
                        CreateDate = CreateDate,
                        Status = 2,
                        StatusNotification = 1,
                        CardId = cardId ,

                    };
                    _context.Orders.Add(order).GetDatabaseValues();
                    _context.SaveChanges();
                }
                else
                {
                    if (_bonusService.CheckBonus(customerId,bonusPriceCovert))
                    {
                        return Json(new { status = "error", message = " Sizin kifayət qədər bonus balansiniz yoxdur! " });
                    }
                    else
                    {
                        
                        order = new Order()
                        {
                            Amount = Convert.ToInt32(amount),
                            CustomerId = customerId,
                            Debt = debt,
                            OldDebt = debt,
                            IsCredite = true,
                            MonthPrice = monthlyPayment,
                            Name = productName,
                            Quantity = quantity,
                            HasBonus = IsBonus,
                            Duration = Convert.ToInt32(duration),
                            PaymentDate = CreateDate.AddMonths(1),
                            Price = Convert.ToDouble(price),
                            Cost = cost,
                            FirstPrice = Convert.ToDouble(firstPrice),
                            BonusPrice = Convert.ToDouble(bonusPrice),
                            ProductId = productId,
                            TotalPrice = Convert.ToDouble(totalPrice),
                            UserId = UserId,
                            CreateDate = CreateDate,
                            Status = 2,
                            StatusNotification = 1,
                            CardId = cardId,

                        };
                        _context.Orders.Add(order).GetDatabaseValues();
                        _context.SaveChanges();
                        _bonusService.RemoveBonus(customerId, bonusPriceCovert, order.Id);
                    }
                }
               

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
                if (IsBonus)
                {
                    var customerDbase = _context.Customers.FirstOrDefault(c => c.Id == customerId);
                    customerDbase.BonusAmount = (customerDbase.BonusAmount ?? 0) + (Convert.ToDouble(totalPrice) / 100) * 2;
                    _context.Customers.Update(customerDbase);
                    _context.SaveChanges();
                }
               
                if (referencesId!=0 && false)
                {
                    var customerDbRef = _context.Customers.Where(c => c.Id == customerId).First();
                    customerDbRef.ReferencesId = referencesId;
                    _context.Customers.Update(customerDbRef);
                    _bonusService.AddBonus(referencesId, order.TotalPrice, order.Id);
                }

                _context.Logs.Add(new Log()
                {
                    Error =   $"Sifaris ugurla yerine yetrildi.Məhsulun adı{order.Product.Name} alanin fini: {order.Customer.Fincode}, sifarisin nomresi{order.Id} ,bonus eelave olunsun {hasBonus}   ",
                    UserId = UserId,
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
        public JsonResult CreateOrderCash(string name, string surname, string fatherName, string baseNumber, string fincode,
             string quantity, string price, string amount, string productName, string totalPrice,
             string InstagramAddress, string CustomerId, string dateCreate, string cost, string dateBirth,
             int referencesId, string bonusPrice,string hasBonus)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);

            try
            {
                fincode = fincode ?? "";
                var IsBonus = Convert.ToBoolean(hasBonus);
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
                var bonusPriceCovert = Convert.ToDouble(bonusPrice);

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
                        BonusAmount = 0,
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
                var order = new Order();
                if (bonusPriceCovert == 0)
                {
                    order = new Order()
                    {
                        Amount = Convert.ToInt32(amount),
                        CustomerId = customerId,
                        IsCredite = false,
                        Name = productName,
                        Quantity = quantity,
                        HasBonus = IsBonus,
                        Price = Convert.ToDouble(price),
                        Cost = Convert.ToInt32(cost),
                        ProductId = productId,
                        TotalPrice = Convert.ToDouble(totalPrice),
                        UserId = UserId,
                        CreateDate = CreateDate,
                        Status = 2,
                        StatusNotification = 1,
                         

                    };
                    _context.Orders.Add(order).GetDatabaseValues();
                    _context.SaveChanges();
                }
                else
                {
                    if (_bonusService.CheckBonus(customerId, bonusPriceCovert))
                    {
                        return Json(new { status = "error", message = " Sizin kifayət qədər bonus balansiniz yoxdur! " });
                    }
                    else
                    {
                        order = new Order()
                        {
                            Amount = Convert.ToInt32(amount),
                            CustomerId = customerId,
                            IsCredite = false,
                            Name = productName,
                            Quantity = quantity,
                            HasBonus = IsBonus,
                            Price = Convert.ToDouble(price),
                            Cost = Convert.ToInt32(cost),
                            ProductId = productId,
                            TotalPrice = Convert.ToDouble(totalPrice),
                            UserId = UserId,
                            CreateDate = CreateDate,
                            Status = 2,
                            StatusNotification = 1,
                        };
                        _context.Orders.Add(order).GetDatabaseValues();
                        _context.SaveChanges();
                        _bonusService.RemoveBonus(customerId, bonusPriceCovert, order.Id);
                    }
                }
                if (referencesId != 0 && false)
                {
                    var customerDbRef = _context.Customers.Where(c => c.Id == customerId).First();
                    customerDbRef.ReferencesId = referencesId;
                    _context.Customers.Update(customerDbRef);
                   
                    _bonusService.AddBonus(referencesId, order.TotalPrice, order.Id);
                   
                }
                if (IsBonus)
                {
                    var customerDbase = _context.Customers.FirstOrDefault(c => c.Id == customerId);
                    customerDbase.BonusAmount = (customerDbase.BonusAmount ?? 0) + (Convert.ToDouble(totalPrice) / 100) * 2;
                    _context.Customers.Update(customerDbase);
                    _context.SaveChanges();
                }
                _context.Logs.Add(new Log()
                {
                    Error = $"Sifaris ugurla yerine yetrildi.Məhsulun adı{order.Product.Name} alanin fini {order.Customer.Fincode}, sifarisin nomresi{order.Id} ,bonus elave olsun {hasBonus}   ",
                    UserId = UserId,
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception  )
            {
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
                if (order.Debt ==0 || order.Debt<0)
                {
                   
                    order.Status = 1;
                    order.StatusNotification = 2;
                    
                    _context.Orders.Update(order);
                    _context.SaveChanges();

                     if (_context.Cards.Any(c => c.Id == order.CardId) )
                {
                    var cardDb = _context.Cards.Where(c => c.Id == order.CardId).First();
                    cardDb.Limit = cardDb.Limit - (int)order.MonthPrice;
                    _context.Cards.Update(cardDb);
                    _context.SaveChanges();
                }
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
                    var pymentH = new PaymentHistory()
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
                    };
                    _context.PaymentHistories.Add(pymentH);
                    _context.SaveChanges();
                    var crediteHistory = new CrediteHistory()
                    {
                        CachMany = Price,
                        UserId = UserId,
                        Note = note,
                        OrderId = orderId,
                        PaymentHistoryId = pymentH.Id
                    };
                    _context.CrediteHistories.Add(crediteHistory);
                    _context.SaveChanges();
                     
                }

                _context.Logs.Add(new Log()
                {
                    Error = $"Ugurlu, odenis miqdar {price}, musterinin fini {order.Customer.Fincode},sifarisin nomresi {order.Id}",
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
                var order = _context.Orders.Where(c => c.Id == orderId).Include(c=>c.PaymentHistories).FirstOrDefault();
                order.PaymentDate = order.PaymentDate?.AddDays(changeDay);
                var history = order.PaymentHistories;
                foreach (var item in history.Where(c=>c.Status==false))
                {
                    item.PaymentDate = item.PaymentDate?.AddDays(changeDay);
                    item.PayDate = item.PayDate?.AddDays(changeDay);
                }
                _context.PaymentHistories.UpdateRange(history);
                _context.SaveChanges();
                order.StatusNotification = 1;
                _context.Orders.Update(order);
                _context.SaveChanges();

                _context.Logs.Add(new Log()
                {
                    Error = $"Ayliqin vaxtini deyismek gunlerin sayi:{changeDay},musterinin fini {order.Customer.Fincode}, sifarisin nomresi:{order.Id} ",
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception  )
            {
                
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
                    Error = "Maya deyerin deyisdirmesi  yeni qiymet" + changeCost.ToString()+" kohne qiymet:"+ orderCost,
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception  )
            {
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
                var order = _context.Orders.Where(c => c.Id == orderId).Include(c=>c.Customer).First();
                var payHistory = _context.PaymentHistories.Where(c => c.Id == payHistoryId).First();
                var oldMany = payHistory.PayPrice;
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
                    Error = $"odenisin miqdarinini deyismek yeni miqdar: {newMany},kohne miqdar {oldMany},musterinin fini {order.Customer.Fincode}, sifarisin nomresi {order.Id}",
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 1,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();


                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception  )
            {
                 
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
                var order = _context.Orders.Where(c => c.Id == orderId).Include(c=>c.Customer).First();
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
                    Error = $"odenisin Silmek,musterinin fini {order.Customer.Fincode}, sifarisin nomresi {order.Id}",
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 3,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();

                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception  )
            {
                 
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
                var bonushistory = _context.BonusHistories.Where(c => c.OrderId == orderId);
                var order = _context.Orders.Where(c => c.Id == orderId).Include(c=>c.Customer).FirstOrDefault();
                if (order.CardId!=null && order.MonthPrice!=null)
                {
                    var card = _context.Cards.Where(c => c.Id == order.CardId).First();
                    card.Limit -= Convert.ToInt32(order.MonthPrice);
                    _context.Cards.Update(card);
                    _context.SaveChanges();
                }
                if (order.Customer.BonusAmount>0 && order.HasBonus==true)
                {
                    var d = Convert.ToDecimal((order.Customer.BonusAmount - order.TotalPrice / 50));
                    order.Customer.BonusAmount = (double?)Math.Round(d,2);
                    
                }
                _context.CrediteHistories.RemoveRange(creadit);
                _context.SaveChanges();
                _context.BonusHistories.RemoveRange(bonushistory);
                _context.SaveChanges();
                _context.PaymentHistories.RemoveRange(Paymenthistory);
                _context.SaveChanges();
                _context.Orders.Remove(order);
                _context.SaveChanges();

                _context.Logs.Add(new Log()
                {
                    Error = $"sifarisi Silmek,musterinin fini {order.Customer.Fincode}, sifarisin nomresi {order.Id}",
                    UserId = UserId,
                    BrowserInfo = "order id=" + OrderId.ToString(),
                    Success = true,
                    Type = 3,
                    Url = ControllerContext.ActionDescriptor.ControllerName + "/" + ControllerContext.ActionDescriptor.ActionName
                });
                _context.SaveChanges();
                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception  )
            {
                 
                return Json(new { status = "error", message = "Xəta baş verdi" });
            }
        }

        public IActionResult Cashbox()
        {
           var model= _context.Users.Where(c => c.RoleId != 2).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult CashboxState(string dateRange,int userId)
        {
            try
            {
                DateTime startDateTime = DateTime.MinValue;
                DateTime endDateTime = DateTime.Now;
                var users = _context.Users.Where(c => c.RoleId != 2).ToList();
                double cachOrder = 0;//pullun miqdari
                var crediteHistory = new List<CrediteHistory>(); ;
                var model = new CashModel();
                if (!String.IsNullOrEmpty(dateRange))
                {
                    endDateTime = DateTime.ParseExact(dateRange.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                crediteHistory = _context.CrediteHistories.Where(c => c.CreateDate.Date == endDateTime.Date && c.UserId==userId)
                    .Include(c=>c.Order).ThenInclude(c=>c.Customer)
                    .Include(c => c.Order)
                    .ThenInclude(c=>c.Card)
                    .OrderBy(c => c.CreateDate).ToList();
                foreach (var item in crediteHistory)
                {
                    cachOrder += item.CachMany;
                }
                 model.CrediteHistories=crediteHistory;
                model.Money = cachOrder;
                ViewBag.Cashmoney = cachOrder;

                return PartialView("_PartialTableCashbox", model);
               
            }
            catch (Exception  )
            {
                return Json(new { status = "error" });
            }
        }
    }
}
