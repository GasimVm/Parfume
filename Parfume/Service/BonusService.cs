using Parfume.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Parfume.Service
{
    public class BonusService : IBonusService
    {
        private readonly ParfumeContext _context;
        public BonusService(ParfumeContext context)
        {
            _context = context;
        }
        public bool AddBonus(int customerId, double TotalPrice, int orderId)
        {
            var bonus = _context.Bonus.Where(c => c.CustomerId == customerId).First();
            var amountBonus = (TotalPrice * bonus.Precent /100 );
            bonus.Amount += amountBonus;
            bonus.Customer.BonusAmount += amountBonus;
            _context.Bonus.Update(bonus);
            _context.Customers.Update(bonus.Customer);
            _context.SaveChanges();
            _context.BonusHistories.Add(new Models.BonusHistory()
            {
                CustomerId = customerId,
                Amount = amountBonus,
                BonusId = bonus.Id,
                IsIncome = true,
                OrderId = orderId
            });
            return _context.SaveChanges() > 0;
        }

        public bool CheckBonus(int customerId, double bonusAmount)
        {
            var customerDb = _context.Customers.Where(c => c.Id == customerId).First();
           if(customerDb.BonusAmount<bonusAmount || customerDb.BonusAmount==bonusAmount)
            {
                return true;
            }
            return false;

        }

        public bool RemoveBonus(int customerId, double bonusAmount, int orderId)
        {
            var customerDb = _context.Customers.Where(c => c.Id == customerId).First();
            customerDb.BonusAmount -= bonusAmount;
            _context.Customers.Update(customerDb);
            _context.BonusHistories.Add(new Models.BonusHistory()
            {
                CustomerId = customerId,
                Amount = bonusAmount,
                IsIncome = false,
                OrderId = orderId
            });
            return _context.SaveChanges() > 0;
        }
    }
}
