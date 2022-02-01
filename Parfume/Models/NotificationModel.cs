using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class NotificationModel
    {
        public string NotificationText { get; set; }
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public string Url { get; set; }
        public int? Status { get; set; }
        public string Date { get; set; }
        public int OrderId { get; set; }
        
    }
}
