using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class User
    {
        public User()
        {
            CrediteHistories = new HashSet<CrediteHistory>();
            Orders = new HashSet<Order>();
            Logs = new HashSet<Log>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string Fincode { get; set; }
        public string Password { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? CreateDate { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

       
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<CrediteHistory>  CrediteHistories { get; set; }
        public virtual ICollection<Log> Logs { get; set; }



    }
}
