#region

using System;
using System.Collections.Generic;
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
            if (start > end)
            {
                return 0;
            }

            var budgets = _repo.GetAll();
            if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
            {
                var interval = (end - start).Days + 1;
                var startAmount = GetMonthAmount(start, budgets);
                return interval * startAmount / DateTime.DaysInMonth(end.Year, end.Month);
            }

            decimal middleAmount = 0;
            var period = new Period(start, end);
            foreach (var budget in budgets)
            {
                middleAmount += budget.OverlappingAmount(period);
            }

            return middleAmount;
        }

        private static decimal GetMonthAmount(DateTime start, List<Budget> budgets)
        {
            var startMonthData = budgets.FirstOrDefault(a => a.YearMonth == start.ToString("yyyyMM"));

            return startMonthData?.Amount ?? 0;
        }
    }
}