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
        (IEnumerable<CustomerModel> customers, int rowCount) GetCustomerWithPhone(string fincode, int page, int length, string search);

        public IEnumerable<CustomerModel> GetCustomerWithDebt(   int type);
    }


}
