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
            var amountBonus = (TotalPrice * bonus.Precent / 100);
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

        public bool AddBonusCardHistory(int bonusCardId, double bonusAmount, int orderId, int customerId)
        {
            _context.BonusCardHistories.Add(new Models.BonusCardHistory()
            {
                Amount = bonusAmount,
                BonusCardId = bonusCardId,
                OrderId = orderId,
                CustomerId = customerId
            });
            return _context.SaveChanges() > 0;
        }

        public bool CheckBonus(int customerId, double bonusAmount)
        {
            var customerDb = _context.Customers.Where(c => c.Id == customerId).First();
            if (customerDb.BonusAmount < bonusAmount || customerDb.BonusAmount == bonusAmount)
            {
                return true;
            }
            return false;

        }

        public bool CheckBonusCardAmount(int bonusCardId, double? bonusAmount)
        {
            var bonusCard = _context.BonusCards.Where(c => c.Id == bonusCardId && c.IsActive).First();
            if (bonusCard.Balans >= bonusAmount)
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

        public bool RemoveBonusCard(double? bonusAmount, int bonusCardId)
        {
            var bonusCard = _context.BonusCards.Where(c => c.Id == bonusCardId).First();
            if (bonusAmount==bonusCard.Balans)
            {
                bonusCard.Balans = 0;
                bonusCard.IsActive = false;
                _context.BonusCards.Update(bonusCard);
               return _context.SaveChanges()>0;
            }
            else
            {
                bonusCard.Balans = bonusCard.Balans-bonusAmount;
                _context.BonusCards.Update(bonusCard);
                return _context.SaveChanges() > 0;
            }

            return false;
        }
    }
}
