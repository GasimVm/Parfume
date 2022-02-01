using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Service
{
  public  interface IGeneralNotifactionService
    {
        Task SendWebAsync(List<NotificationModel> notifications);
    }
}
