using Microsoft.EntityFrameworkCore;
using Parfume.DAL;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Service
{
    public class GeneralNotifactionService :IGeneralNotifactionService
    {
        private DbContextOptionsBuilder<ParfumeContext> _optionsBuilder;
        private IPushNotificationService _pushNotificationService;
         

        public GeneralNotifactionService(IPushNotificationService pushNotificationService )
        {
            _optionsBuilder = new DbContextOptionsBuilder<ParfumeContext>();
            _pushNotificationService = pushNotificationService;
            
        }

        public async Task SendWebAsync(List<NotificationModel> notifications)
        {
            Payload payload;
            List<(int userId, Payload payload)> data = new List<(int userId, Payload payload)>();

            _optionsBuilder.UseSqlServer(AppConfig.ConnectionString);
            using (ParfumeContext dbContext = new ParfumeContext(_optionsBuilder.Options))
            {
                foreach (var notification in notifications)
                {
                    payload = new Payload
                    {
                        title = $"#{notification.OrderId}",
                        message = notification.NotificationText,
                        url = notification.Url
                         
                    };

                    data.Add((
                             notification.OrderId,
                             payload
                            ));
                }
            }
            _pushNotificationService.Push(data);
        }
    }
}
