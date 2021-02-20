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
            //var endMonthData = budgets.FirstOrDefault(a => a.YearMonth == end.ToString("yyyyMM"));
            var startAmount= GetMonthAmount(start, budgets);
            
            // 起始
            var startMonth_TotalDays = DateTime.DaysInMonth(start.Year, start.Month);
            var strDays = startMonth_TotalDays - start.Day + 1;

            if (start.Month == end.Month)
            {
                var interval = (end - start).Days + 1;
                return interval*startAmount / startMonth_TotalDays ;
            }
           
            var amount1 = (strDays * startAmount / startMonth_TotalDays);
           // return amount1;
            var endMonth_TotalDays = DateTime.DaysInMonth(end.Year, end.Month);
        
            var endAmount = GetMonthAmount(end, budgets);
            var endDays = end.Day;
            var amount2 = (endDays * endAmount / endMonth_TotalDays) ;
          
            // 結束
            return amount1 + amount2;
            // var strDays = StartTotalDays(start);




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