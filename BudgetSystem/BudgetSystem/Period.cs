#region

using System;

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

        private DateTime Start { get; }
        private DateTime End { get; }

        public int OverlappingDays(Budget budget)
        {
            var another = new Period(budget.FirstDay(), budget.LastDay());
            var firstDay = budget.FirstDay();
            var lastDay = budget.LastDay();
            var overlappingStart = Start > firstDay ? Start : firstDay;
            var overlappingEnd = End < lastDay ? End : lastDay;

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}