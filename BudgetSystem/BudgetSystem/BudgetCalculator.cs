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
            var currentDate = start;
            var middleEnd = new DateTime(end.Year, end.Month, 1).AddMonths(1);
            while (currentDate < middleEnd)
            {
                var budget = budgets.FirstOrDefault(a => a.YearMonth == currentDate.ToString("yyyyMM"));
                if (budget != null)
                {
                    var overlappingDays = OverlappingDays(start, end, budget);

                    middleAmount += overlappingDays * budget.DailyAmount();
                }

                currentDate = currentDate.AddMonths(1);
            }

            return middleAmount;
        }

        private static decimal GetMonthAmount(DateTime start, List<Budget> budgets)
        {
            var startMonthData = budgets.FirstOrDefault(a => a.YearMonth == start.ToString("yyyyMM"));

            return startMonthData?.Amount ?? 0;
        }

        private static int OverlappingDays(DateTime start, DateTime end, Budget budget)
        {
            int overlappingDays;
            if (budget.YearMonth == start.ToString("yyyyMM"))
            {
                overlappingDays = budget.Days() - start.Day + 1;
            }
            else if (budget.YearMonth == end.ToString("yyyyMM"))
            {
                overlappingDays = end.Day;
            }
            else
            {
                overlappingDays = budget.Days();
            }

            return overlappingDays;
        }
    }
}