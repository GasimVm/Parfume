using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class Seller
    {
        public Seller()
        {
            SellerByOrders = new HashSet<SellerByOrderHistory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<SellerByOrderHistory> SellerByOrders { get; set; }


    }
}
