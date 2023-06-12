using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            Bonus = new HashSet<Bonus>();
            PaymentHistories = new HashSet<PaymentHistory>();
            BonusHistories = new HashSet<BonusHistory>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName  { get; set; }
        public string Fincode  { get; set; }
        public string Address  { get; set; }
        public string WorkAddress  { get; set; }
        public string InstagramAddress { get; set; }
        public string BaseNumber  { get; set; }
        public string FirstNumber  { get; set; }
        public string FirstNumberWho { get; set; }
        public string SecondNumber { get; set; }
        public string SecondNumberWho { get; set; }
        public string ThirdNumber { get; set; }
        public string ThirdNumberWho { get; set; }
        public bool IsBlock { get; set; }
        public DateTime? BlockDate { get; set; }
        public DateTime? Birthday { get; set; }
        public string BlockNote { get; set; }
        public string WhoIsOkey { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public bool? IsVIP { get; set; }
        public double? BonusAmount { get; set; }
        public int? BonusDegree { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CardId { get; set; }
        public virtual Card Card { get; set; }

        public int? ReferencesId { get; set; }
        public virtual Customer ReferencesCustomer { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Bonus> Bonus { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
        public virtual ICollection<BonusHistory> BonusHistories { get; set; }



    }
}
