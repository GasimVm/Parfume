using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class BonusCardHistory
    {
        public int Id { get; set; }
        public double? Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public int? BonusCardId { get; set; }
        public virtual BonusCard BonusCard { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
