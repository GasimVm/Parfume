using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Service
{
  public  interface IBonusService
    {
        public bool AddBonus(int customerId, double TotalPrice,int orderId);
        public bool CheckBonus(int customerId ,double bonusAmount);
        public bool RemoveBonus(int customerId ,double bonusAmount, int orderId);
    }
}
