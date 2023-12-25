using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class Order
    {
        public Order()
        {
             CrediteHistories = new HashSet<CrediteHistory>();
            PaymentHistories=new HashSet<PaymentHistory>();
            BonusHistories = new HashSet<BonusHistory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public Double Price { get; set; }
        public Double? MonthPrice { get; set; }
        public Double? Debt { get; set; }
        public Double? OldDebt { get; set; }
        public Double? FirstPrice { get; set; }
        // pay by bonus
        public Double? BonusPrice { get; set; }
        public Double TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int? Duration { get; set; }
        public string Quantity { get; set; }
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int Cost { get; set; }
        public bool IsCredite { get; set; }
        public bool? HasBonus { get; set; }
        
        public DateTime CreateDate   { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? CreateOn { get; set; }

        // 2-borclu 1-borcu bitib
        public int Status { get; set; }
        // 1-gonderilir 2-gonderilmir
        public int StatusNotification { get; set; }
        public int? CardId { get; set; }
        public virtual Card Card { get; set; }
        public virtual ICollection<CrediteHistory>  CrediteHistories { get; set; }
        public virtual ICollection<PaymentHistory>   PaymentHistories { get; set; }
        public virtual ICollection<BonusHistory>    BonusHistories { get; set; }

    }
}
