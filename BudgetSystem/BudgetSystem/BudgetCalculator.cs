#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace BudgetSystem
{
    public class Period
    {
        public Period(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public int OverlappingDays(Budget budget)
        {
            var overlappingStart = Start > budget.FirstDay() ? Start : budget.FirstDay();
            var overlappingEnd = End < budget.LastDay() ? End : budget.LastDay();

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }

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
                    var overlappingDays = new Period(start, end).OverlappingDays(budget);

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
    }
}