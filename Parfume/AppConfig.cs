using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume
{
    public class AppConfig
    {
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["PARFUME"].ConnectionString;
    }
}
