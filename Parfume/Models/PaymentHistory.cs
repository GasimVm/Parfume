using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class PaymentHistory
    {
        public PaymentHistory()
        {
            CrediteHistories = new HashSet<CrediteHistory>();
        }
        public int Id { get; set; }

        public Double? MonthPrice { get; set; }
        public Double? PayPrice { get; set; }
        public string Note { get; set; }

        public int Queue { get; set; }
        public Double Debt { get; set; }
        public DateTime? PaymentDate { get; set; }
        // odenis etdiyi tarix
        public DateTime? PayDate { get; set; }

        // false-borclu true-oduyub
        public bool Status { get; set; }
         
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int? CardId { get; set; }
        public virtual Card Card { get; set; }
        public virtual ICollection<CrediteHistory>  CrediteHistories { get; set; }


    }
}
