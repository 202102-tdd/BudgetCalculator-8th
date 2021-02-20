#region

using System;
using System.Linq;

#endregion

namespace BudgetSystem
{
    public class BudgetCalculator
    {
        private readonly IBudgetRepo _repo;

        public BudgetCalculator(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public decimal Query(DateTime start, DateTime end)
        {
            var period = new Period(start, end);
            return _repo.GetAll().Sum(b => b.OverlappingAmount(period));
        }
    }
}