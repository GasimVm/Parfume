using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Parfume.DAL;
using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebPush;

namespace Parfume.Service
{
    public class PushNotificationService :IPushNotificationService
    {
        private VapidKeys _vapidKeys { get; }
        private DbContextOptionsBuilder<ParfumeContext> _optionsBuilder;

        public PushNotificationService(IOptions<VapidKeys> vapidKeys)
        {
            _vapidKeys = vapidKeys.Value;
            _optionsBuilder = new DbContextOptionsBuilder<ParfumeContext>();
        }

        public void Push(List<(int userId, Payload payload)> data)
        {
            _optionsBuilder.UseSqlServer(AppConfig.ConnectionString);
            using (ParfumeContext dbContext = new ParfumeContext(_optionsBuilder.Options))
            {
                VapidDetails vapidDetails = new VapidDetails("mailto:gasimvm@code.edu.az", _vapidKeys.PublicKey, _vapidKeys.PrivateKey);
                string _payload = "";
                PushSubscription subscription;
                WebPushClient webPushClient = new WebPushClient();
                List<UserWebPushCredentials> notValidCredentials = new List<UserWebPushCredentials>();
                foreach ((int userId, Payload payload) datum in data)
                {
                    IQueryable<UserWebPushCredentials> userSubscriptions = dbContext.userWebPushCredentials.Where(u => u.UserId>0);
                    foreach (UserWebPushCredentials userCredential in userSubscriptions)
                    {
                        subscription = new PushSubscription(userCredential.PushEndPoint, userCredential.P256dh, userCredential.Auth);
                        _payload = JsonConvert.SerializeObject(datum.payload);
                        try
                        {
                            if (!String.IsNullOrEmpty(_payload))
                            {
                                webPushClient.SendNotification(subscription, _payload, vapidDetails);
                            }
                          
                        }
                        catch (Exception exc)
                        {
                            if (exc.Message.Equals("Subscription no longer valid"))
                            {
                                notValidCredentials.Add(userCredential);
                            }
                        }
                    }
                }
                dbContext.RemoveRange(notValidCredentials);
                dbContext.SaveChanges();
            }
        }
    }
}
