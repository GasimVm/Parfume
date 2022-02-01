using Parfume.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using Parfume.Service;
using System.Threading.Tasks;
using Parfume.Models;
using Microsoft.EntityFrameworkCore;

namespace Parfume.Service
{

    public class SendNotification : ISendNotification
    {
        private readonly ParfumeContext _context;
        private readonly IGeneralNotifactionService _generalNotifaction;
        public SendNotification(ParfumeContext context, IGeneralNotifactionService generalNotifaction)
        {
            _context = context;
            _generalNotifaction = generalNotifaction;
        }
        public async Task Print()
        {
            var today = DateTime.Now.AddDays(-1);
            var orders = _context.Orders.Where(c => c.PaymentDate < today && c.Status==2 && c.IsCredite && c.StatusNotification==1).Include(c => c.Customer).ToList();
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
                
                item.StatusNotification = 2;
                _context.Orders.Update(item);
                _context.SaveChanges();
            }

            await _generalNotifaction.SendWebAsync(notification);
        }
    }
}
