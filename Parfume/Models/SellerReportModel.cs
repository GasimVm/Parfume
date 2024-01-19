using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class SellerReportModel
    {
        public List<Order> Orders { get; set; }
        public int Count { get; set; }
    }
}
