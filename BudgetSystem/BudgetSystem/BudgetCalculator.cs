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

            if (Math.Abs(start.Month - end.Month) >= 2)
            {
                decimal totalInterval = 0;
                var tempDate = start.AddMonths(1);
                while (tempDate > start && tempDate < end)
                {
                    totalInterval += GetMonthAmount(tempDate, budgets);
                    tempDate = tempDate.AddMonths(1);
                }

                return StartAmount(start, budgets) + totalInterval + EndAmount(end, budgets);
            }
            else
            {
                return StartAmount(start, budgets) + EndAmount(end, budgets);
            }
        }

        private static decimal EndAmount(DateTime end, List<Budget> budgets)
        {
            var endMonth_TotalDays = DateTime.DaysInMonth(end.Year, end.Month);

            var endAmount = GetMonthAmount(end, budgets);
            return end.Day * endAmount / endMonth_TotalDays;
        }

        private static decimal GetMonthAmount(DateTime start, List<Budget> budgets)
        {
            var startMonthData = budgets.FirstOrDefault(a => a.YearMonth == start.ToString("yyyyMM"));

            return startMonthData?.Amount ?? 0;
        }

        private static decimal StartAmount(DateTime start, List<Budget> budgets)
        {
            var startAmount = GetMonthAmount(start, budgets);
            var startMonth_TotalDays = DateTime.DaysInMonth(start.Year, start.Month);
            var strDays = startMonth_TotalDays - start.Day + 1;
            return strDays * startAmount / startMonth_TotalDays;
        }
    }
}