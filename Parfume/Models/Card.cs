using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class Card
    {

        public Card()
        {
            Customers = new HashSet<Customer>();
            Orders = new HashSet<Order>();
            PaymentHistories = new HashSet<PaymentHistory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Limit { get; set; }
        public bool Active { get; set; } = true;

        public DateTime CreateDate { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PaymentHistory>   PaymentHistories { get; set; }
    }
}
