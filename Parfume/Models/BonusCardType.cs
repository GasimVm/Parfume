using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class BonusCardType
    {
        public BonusCardType()
        {
            BonusCard = new HashSet<BonusCard>();
        }
        public int Id { get; set; }
        public int Amount { get; set; }

        public virtual ICollection<BonusCard> BonusCard { get; set; }
    }
}
