using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Models
{
    public class PushSubscribtionModel
    {
        public string PushEndPoint { get; set; }

        public string P256dh { get; set; }

        public string Auth { get; set; }
    }
}
