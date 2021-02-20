using System;

namespace BudgetSystem
{
    public class BudgetCalculator
    {
        private readonly IBudgetRepo _repo;

        public BudgetCalculator(IBudgetRepo repo)
        {
            _repo = repo;
        }

        public decimal query(DateTime start, DateTime end)
        {
            var budgets = _repo.GetAll();
            return budgets[0].Amount;
        }
    }
}