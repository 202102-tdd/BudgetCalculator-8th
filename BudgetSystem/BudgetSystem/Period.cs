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
            var overlappingStart = Start > budget.FirstDay() ? Start : budget.FirstDay();
            var overlappingEnd = End < budget.LastDay() ? End : budget.LastDay();

            return (overlappingEnd - overlappingStart).Days + 1;
        }
    }
}