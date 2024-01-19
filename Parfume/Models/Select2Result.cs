using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class Select2Result
    {
        public string id { get; set; }
        public string text { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Fincode { get; set; }
        public string Address { get; set; }
        public string WorkAddress { get; set; }
        public string InstagramAddress { get; set; }

        public string BaseNumber { get; set; }
        public string FirstNumber { get; set; }
        public string FirstNumberWho { get; set; }

        public string SecondNumber { get; set; }
        public string SecondNumberWho { get; set; }
        public string ThirdNumber { get; set; }
        public string ThirdNumberWho { get; set; }
        public string WhoIsOkey { get; set; }
        public double? BonusAmount { get; set; }
        public int? CardId { get; set; }
        public int? ReferencesId { get; set; }
        public double? Amount { get; set; }
    }
}
