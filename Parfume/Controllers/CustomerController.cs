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

namespace Parfume.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ParfumeContext _context;
        private readonly IGeneralNotifactionService _generalNotifaction;
        private readonly IUserService _userService;
        public CustomerController(ParfumeContext context, IGeneralNotifactionService generalNotifaction, IUserService userService)
        {
            _context = context;
            _generalNotifaction = generalNotifaction;
            _userService = userService;
        }
        public IActionResult AllCustomer()
        {
            var model = _context.Customers.Where(c => c.IsBlock == false && c.IsActive == true).Include(c => c.Orders).ToList();
            return View(model);
        }
        public IActionResult AllCustomerBirthday()
        {
            List<Customer> customers = new List<Customer>();
            var mouth = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            foreach (var item in _context.Customers.Where(c=>c.Birthday!=null))
            {
                if (item.Birthday?.Day==day && item.Birthday?.Month==mouth)
                {
                    customers.Add(item);
                }
            }
            return View(customers);
        }
        public IActionResult GeneralCustomer()
        {
            var model = _context.Customers.Include(c => c.Orders).ToList();
            return View(model);
        }

        public IActionResult GeneralActiveCustomer()
        {

            var model = _userService.GetCustomerWithDebt(1);
            
            return View(model);
        }
        public IActionResult GeneralPassiveCustomer()
        {
            var model = _userService.GetCustomerWithDebt(2);

            return View(model);
        }
        public IActionResult CreateCustomer()
        {

            return View();
        }
        [HttpPost]
        public JsonResult CreateCustomer(string name, string surname, string fatherName, string baseNumber,
            string fincode, string address, string workAddress, string InstagramAddress, string firstName, string firstNumber, string secondName,
            string secondNumber, string thirdName, string thirdNumber, string WhoIsOkey ,string dateBirth)
        {

            if (_context.Customers.Any(c => c.Fincode.ToUpper() == fincode.Trim().ToUpper() && c.IsBlock))
            {
                return Json(new { status = "error", message = "Bu şəxs qara siyahıdadır!" });
            }
            if (_context.Customers.Any(c => c.Fincode.ToUpper() == fincode.Trim().ToUpper() && c.IsBlock == true))
            {
                return Json(new { status = "error", message = "Bu şəxs   siyahıda var!" });
            }
            var format = "dd/MM/yyyy";
            CultureInfo provider = CultureInfo.InvariantCulture;
             
            DateTime? BirthDate = null;
            if (dateBirth != null)
            {
                BirthDate = DateTime.ParseExact(dateBirth, format, provider);
            }
            var customer = new Customer()
            {
                Address = address,
                BaseNumber = baseNumber,
                FatherName = fatherName,
                Fincode = fincode.Trim().ToUpper(),
                InstagramAddress = InstagramAddress,
                Name = name,
                Surname = surname,
                WorkAddress = workAddress,
                FirstNumber = firstNumber,
                FirstNumberWho = firstName,
                SecondNumberWho = secondName,
                SecondNumber = secondNumber,
                ThirdNumberWho = thirdName,
                ThirdNumber = thirdNumber,
                WhoIsOkey = WhoIsOkey,
                Birthday=BirthDate

            };
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
        }

        public IActionResult BlockCustomer()
        {
            var model = _context.Customers.Where(c => c.IsBlock == true).Include(c => c.Orders).ToList();
            return View(model);
        }

        public JsonResult AddBlock(string CustomerId, string NoteBlock)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                var customerId = Convert.ToInt32(CustomerId);
                if (_context.Customers.Any(c => c.Id == customerId))
                {
                    var customer = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault();
                    customer.IsBlock = true;
                    customer.BlockNote = NoteBlock;
                    customer.BlockDate = DateTime.Now;
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
        public async Task<bool> SendNotification()
        {
            var today = DateTime.Now.AddDays(-1);
            var orders = _context.Orders.Where(c => c.PaymentDate < today).Include(c => c.Customer).ToList();
            var notification = new List<NotificationModel>();
            foreach (var item in orders)
            {
                notification.Add(new NotificationModel()
                {
                    CustomerId = item.CustomerId,
                    NotificationText = $"{item.Customer.Name} {item.Customer.Surname} sabah odenisin vaxtidi!",
                    OrderId = item.Id,
                    Url = $"Home/Pay?orderId={item.Id}"
                });
            }

            await _generalNotifaction.SendWebAsync(notification);
            return true;
        }

        public IActionResult CustomerDetails(int customerId)
        {
            var model = _context.Customers.Where(c => c.Id == customerId).Include(c => c.Orders).ThenInclude(c => c.User).FirstOrDefault();
            return View(model);
        }

        public IActionResult CustomerEdit(int customerId)
        {
            
            var model = _context.Customers.Where(c => c.Id == customerId).Include(c=>c.Card).Include(c => c.Orders).ThenInclude(c => c.User).FirstOrDefault();
            return View(model);
        }

        public IActionResult CustomerChange(int customerId)
        {
            var model = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public JsonResult CustomerChange(int CustomerId, string name, string surname, string fatherName, string baseNumber, string fincode, string firstName, string firstNumber, string secondName,
            string secondNumber, string thirdName, string thirdNumber, string address,
            string workAddress, string InstagramAddress, string WhoIsOkey , string dateBirth)
        {
            try
            {
                var format = "dd/MM/yyyy";
                CultureInfo provider = CultureInfo.InvariantCulture;

                DateTime? BirthDate = null;
                if (dateBirth != null)
                {
                    BirthDate = DateTime.ParseExact(dateBirth, format, provider);
                }
                if (_context.Customers.Any(c => c.Id == CustomerId))
                {
                    var customer = _context.Customers.Where(c => c.Id == CustomerId).FirstOrDefault();
                    customer.Name = name;
                    customer.Surname = surname;
                    customer.FatherName = fatherName;
                    customer.Fincode = fincode;
                    customer.BaseNumber = baseNumber;
                    customer.WorkAddress = workAddress;
                    customer.InstagramAddress = InstagramAddress;
                    customer.Address = address;
                    customer.FirstNumberWho = firstName;
                    customer.FirstNumber = firstNumber;
                    customer.SecondNumberWho = secondName;
                    customer.SecondNumber = secondNumber;
                    customer.ThirdNumberWho = thirdName;
                    customer.ThirdNumber = thirdNumber;
                    customer.WhoIsOkey = WhoIsOkey;
                    customer.Birthday = BirthDate;
                    customer.CreateDate = DateTime.Now;
                    _context.Customers.Update(customer);
                    _context.SaveChanges();
                }
                return Json(new { status = "success", message = "Uğurla yerinə yetirildi " });
            }
            catch (Exception)
            {

                return Json(new { status = "error", message = "İstifadəci tapilmadi" });
            }

        }
        public JsonResult AddNote(string CustomerId, string noteCustomer, string dateBirth )
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            try
            {
                var customerId = Convert.ToInt32(CustomerId);
                var format = "dd/MM/yyyy";
                CultureInfo provider = CultureInfo.InvariantCulture;

                DateTime? BirthDate = null;
                if (dateBirth != null)
                {
                    BirthDate = DateTime.ParseExact(dateBirth, format, provider);
                }
                if (_context.Customers.Any(c => c.Id == customerId))
                {
                    var customer = _context.Customers.Where(c => c.Id == customerId).FirstOrDefault();
                    customer.Note = noteCustomer;
                    customer.Birthday = BirthDate;
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

       


        public JsonResult Users(string search, int page, string blackList,
                         bool selfAccess = false, bool fullAccess = false, bool isDelegation = false, bool selfInner = false)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            string fincode;
            fincode = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;


            if (!String.IsNullOrEmpty(search))
            {
                search = search.Trim().Replace(" ", "");
            }
            List<int> _blackList = new List<int>();
            if (!string.IsNullOrEmpty(blackList))
            {
                _blackList = blackList.Split(',').Select(Int32.Parse).ToList();
            }


            (IEnumerable<CustomerModel> employees, int rowCount) employeesAndRowCount = _userService.GetCustomer(fincode, page, 100, search);

            IEnumerable<Customer> users =
                 employeesAndRowCount.employees
                 .Select(e => new Customer
                 {
                     Id = e.Id,
                     Fincode = e.Fincode,
                     Name = e.Name,
                     Surname = e.Surname,
                     Address = e.Address,
                     BaseNumber = e.BaseNumber,
                     FatherName = e.FatherName,
                     FirstNumber = e.FirstNumber,
                     FirstNumberWho = e.FirstNumberWho,
                     InstagramAddress = e.InstagramAddress,
                     SecondNumber = e.SecondNumber,
                     SecondNumberWho = e.SecondNumberWho,
                     ThirdNumber = e.ThirdNumber,
                     ThirdNumberWho = e.ThirdNumberWho,
                     WorkAddress = e.WorkAddress,
                     WhoIsOkey = e.WhoIsOkey ,
                      CardId= e.CardId=="" ? 0: Convert.ToInt32( e.CardId)
                 }).ToList();

            List<Select2Result> results = users.Select(u =>
            new Select2Result
            {
                id = u.Id.ToString(),
                text = u.Name + " /" + u.Fincode,
                Name = u.Name,
                Surname = u.Surname,
                FatherName = u.FatherName,
                Fincode = u.Fincode,
                Address = u.Address,
                WorkAddress = u.WorkAddress,
                InstagramAddress = u.InstagramAddress,
                BaseNumber = u.BaseNumber,
                FirstNumber = u.FirstNumber,
                FirstNumberWho = u.FirstNumberWho,
                SecondNumber = u.SecondNumber,
                SecondNumberWho = u.SecondNumberWho,
                ThirdNumber = u.ThirdNumber,
                ThirdNumberWho = u.ThirdNumberWho,
                WhoIsOkey = u.WhoIsOkey,
                CardId=u.CardId
                
            }).ToList();
            return Json(new { results = results, count_filtered = employeesAndRowCount.rowCount });
        }

        public JsonResult UsersWithPhone(string search, int page, string blackList,
                         bool selfAccess = false, bool fullAccess = false, bool isDelegation = false, bool selfInner = false)
        {
            int UserId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
            string fincode;
            fincode = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;


            if (!String.IsNullOrEmpty(search))
            {
                search = search.Trim().Replace(" ", "");
            }
            List<int> _blackList = new List<int>();
            if (!string.IsNullOrEmpty(blackList))
            {
                _blackList = blackList.Split(',').Select(Int32.Parse).ToList();
            }


            (IEnumerable<CustomerModel> employees, int rowCount) employeesAndRowCount = _userService.GetCustomerWithPhone(fincode, page, 100, search);

            IEnumerable<Customer> users =
                 employeesAndRowCount.employees
                 .Select(e => new Customer
                 {
                     Id = e.Id,
                     Fincode = e.Fincode,
                     Name = e.Name,
                     Surname = e.Surname,
                     Address = e.Address,
                     BaseNumber = e.BaseNumber,
                     FatherName = e.FatherName,
                     FirstNumber = e.FirstNumber,
                     FirstNumberWho = e.FirstNumberWho,
                     InstagramAddress = e.InstagramAddress,
                     SecondNumber = e.SecondNumber,
                     SecondNumberWho = e.SecondNumberWho,
                     ThirdNumber = e.ThirdNumber,
                     ThirdNumberWho = e.ThirdNumberWho,
                     WorkAddress = e.WorkAddress,
                     WhoIsOkey = e.WhoIsOkey
                 }).ToList();

            List<Select2Result> results = users.Select(u =>
            new Select2Result
            {
                id = u.Id.ToString(),
                text = u.Name + " /" + u.Fincode +" /" + u.BaseNumber,
                Name = u.Name,
                Surname = u.Surname,
                FatherName = u.FatherName,
                Fincode = u.Fincode,
                Address = u.Address,
                WorkAddress = u.WorkAddress,
                InstagramAddress = u.InstagramAddress,
                BaseNumber = u.BaseNumber,
                FirstNumber = u.FirstNumber,
                FirstNumberWho = u.FirstNumberWho,
                SecondNumber = u.SecondNumber,
                SecondNumberWho = u.SecondNumberWho,
                ThirdNumber = u.ThirdNumber,
                ThirdNumberWho = u.ThirdNumberWho,
                WhoIsOkey = u.WhoIsOkey
            }).ToList();
            return Json(new { results = results, count_filtered = employeesAndRowCount.rowCount });
        }

    }
}
