using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tend.Models
{
    public class LoginViewModel
    {
        [StringLength(50), Required(ErrorMessage = "İstifadəçi fincodu məcburidir.")]
        public string Fincode { get; set; }

        [StringLength(50), Required(ErrorMessage = "Şifrə məcburidir.")]
        public string Password { get; set; }

    }
}
