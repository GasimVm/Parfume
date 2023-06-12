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
            var bonus = _context.Bonus.Where(c => c.CustomerId == customerId).First();
            var customerBonus = bonus.Customer.BonusAmount;
            if (customerBonus < bonusAmount  )
            {
                return true;
            }
            return false;

        }

        public bool RemoveBonus(int customerId, double bonusAmount, int orderId)
        {
            var bonus = _context.Bonus.Where(c => c.CustomerId == customerId).First();
            bonus.Amount -= bonusAmount;
            bonus.Customer.BonusAmount -= bonusAmount;
            _context.Bonus.Update(bonus);
            _context.SaveChanges();
            _context.Customers.Update(bonus.Customer);
            _context.BonusHistories.Add(new Models.BonusHistory()
            {
                CustomerId = customerId,
                Amount = bonusAmount,
                BonusId = bonus.Id,
                IsIncome = false,
                OrderId = orderId
            });
            return _context.SaveChanges() > 0;
        }
    }
}
