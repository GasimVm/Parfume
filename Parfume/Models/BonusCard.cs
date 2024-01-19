using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class BonusCard
    {
        public BonusCard()
        {
            BonusCardHistories = new HashSet<BonusCardHistory>();
        }
        public int Id { get; set; }
        public double? Balans  { get; set; }
        public string CardNumber { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int BonusCardTypeId { get; set; }
        public virtual BonusCardType BonusCardType { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<BonusCardHistory> BonusCardHistories { get; set; }

    }
}
