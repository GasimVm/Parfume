using Parfume.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Service
{
   public interface IUserService
    {
        (IEnumerable<CustomerModel> customers, int rowCount) GetCustomer(string fincode, int page, int length, string search);
    }
}
