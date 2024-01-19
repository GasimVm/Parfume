using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class SellerByOrderHistory
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int? SellerId { get; set; }
        public virtual Seller Seller { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
         
    }
}
