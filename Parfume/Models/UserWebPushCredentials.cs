using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class UserWebPushCredentials
    {
        [Column("ID")]
        public int UserWebPushCredentialsId { get; set; }

        [Column("USER_ID")]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Column("PUSH_END_POINT")]
        public string PushEndPoint { get; set; }

        [Column("P256DH")]
        public string P256dh { get; set; }

        [Column("AUTH")]
        public string Auth { get; set; }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
    }
}
