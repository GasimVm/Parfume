using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class SaleModel
    {
        public List<Card> Cards { get; set; }
        public List<Customer> Customers { get; set; }
        public List<Seller> Sellers { get; set; }
    }
}
