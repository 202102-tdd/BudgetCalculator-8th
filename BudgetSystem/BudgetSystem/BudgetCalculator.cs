﻿#region

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
                if (currentDate.ToString("yyyyMM") == start.ToString("yyyyMM"))
                {
                    var startAmount = GetMonthAmount(start, budgets);
                    var totalDaysOfStart = DateTime.DaysInMonth(start.Year, start.Month);
                    var strDays = totalDaysOfStart - start.Day + 1;
                    middleAmount += strDays * startAmount / totalDaysOfStart;
                }
                else if (currentDate.ToString("yyyyMM") == end.ToString("yyyyMM"))
                {
                    var totalDaysOfEnd = DateTime.DaysInMonth(end.Year, end.Month);

                    var endAmount = GetMonthAmount(end, budgets);
                    middleAmount += end.Day * endAmount / totalDaysOfEnd;
                }
                else
                {
                    middleAmount += GetMonthAmount(currentDate, budgets);
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