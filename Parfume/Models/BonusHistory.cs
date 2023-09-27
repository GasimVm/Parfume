using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class BonusHistory
    {
        public int Id { get; set; }
        public double? Amount { get; set; }
        public bool IsIncome { get; set; } // true --  gelir bonus false cixir
        public DateTime CreateDate { get; set; }
        public int? BonusId { get; set; }
        public virtual Bonus Bonus { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
