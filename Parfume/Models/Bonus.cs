using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class Bonus
    {
        public Bonus()
        {
            BonusHistories = new HashSet<BonusHistory>();
        }
        public int Id { get; set; }
        public double? Amount { get; set; }
        public int Precent { get; set; }
        public int? CardNumber { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<BonusHistory> BonusHistories { get; set; }

    }
}
