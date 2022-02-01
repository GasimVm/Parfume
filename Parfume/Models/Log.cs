using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class Log
    {
         
        public int Id { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        [StringLength(300)]
        public string Fincode { get; set; }
        public string RemoteIpAddress { get; set; }
        public string BrowserInfo { get; set; }
        public string Note { get; set; }

        public string Error { get; set; }
        public String Url { get; set; }
        public int Type { get; set; }
        public bool Success { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
