using System;
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
            var startMonthData = budgets.FirstOrDefault(a => a.YearMonth == start.ToString("yyyyMM"));
            //var endMonthData = budgets.FirstOrDefault(a => a.YearMonth == end.ToString("yyyyMM"));
            if (startMonthData == null)
            {
                return 0;
            }
            else
            {

            }
            var interval = (end - start).Days + 1;
            return budgets[0].Amount / 31 * interval;
        }
    }
}