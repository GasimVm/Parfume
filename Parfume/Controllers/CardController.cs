using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
