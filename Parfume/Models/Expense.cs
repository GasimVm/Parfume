using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
