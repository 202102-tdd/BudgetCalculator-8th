using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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
            if (start>end)
            {
                return 0;
            }
            var budgets = _repo.GetAll();
            if (start.Month == end.Month)
            {
            
                var interval = (end - start).Days + 1;
                var startAmount1 = GetMonthAmount(start, budgets);
                return interval * startAmount1 / DateTime.DaysInMonth(end.Year, end.Month);
            }

            if (Math.Abs(start.Month-end.Month)>=2)
            {
                decimal toalInterval = 0;
                var tempDate = start.AddMonths(1);
                while (tempDate > start && tempDate < end)
                {

                    toalInterval += GetMonthAmount(tempDate, budgets);
                    tempDate= tempDate.AddMonths(1);
                }

                return StartAmount(start, budgets)+ toalInterval + EndAmount(end, budgets);
            }
            else
            {
                return StartAmount(start, budgets) + EndAmount(end, budgets);
            }
        }

        private static decimal StartAmount(DateTime start, List<Budget> budgets)
        {
            var startAmount = GetMonthAmount(start, budgets);
            var startMonth_TotalDays = DateTime.DaysInMonth(start.Year, start.Month);
            var strDays = startMonth_TotalDays - start.Day + 1;
            var amount1 = (strDays * startAmount / startMonth_TotalDays);
            return amount1;
        }

        private static decimal EndAmount(DateTime end, List<Budget> budgets)
        {
            var endMonth_TotalDays = DateTime.DaysInMonth(end.Year, end.Month);

            var endAmount = GetMonthAmount(end, budgets);
            var endDays = end.Day;
            var amount2 = (endDays * endAmount / endMonth_TotalDays);
            return amount2;
        }
      

        private static decimal GetMonthAmount(DateTime start, List<Budget> budgets)
        {
            var startMonthData = budgets.FirstOrDefault(a => a.YearMonth == start.ToString("yyyyMM"));

            if (startMonthData == null)
            {
                return 0;
            }
            else
            {
                return startMonthData.Amount;
            }
        }
    }
}