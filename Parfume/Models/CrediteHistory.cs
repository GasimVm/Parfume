using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class CrediteHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order  Order { get; set; }
        public string Note { get; set; }
        public double CachMany { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public DateTime CreateDate { get; set; }

        public int? PaymentHistoryId { get; set; }
        public virtual PaymentHistory  PaymentHistory { get; set; }


    }
}
