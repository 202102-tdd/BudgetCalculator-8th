#region

using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

#endregion

namespace BudgetSystem
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void Query_Month()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202103", Amount = 31}
                         });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 3, 1), new DateTime(2021, 3, 31));
            Assert.AreEqual(31, query);
        }

        [Test]
        public void Query_One_Day()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202103", Amount = 310}
                         });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 3, 1), new DateTime(2021, 3, 1));
            Assert.AreEqual(10, query);
        }

        [Test]
        public void Query_No_Data()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202102", Amount = 310}
                         });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 3, 1), new DateTime(2021, 3, 1));
            Assert.AreEqual(0, query);
        }

        [Test]
        public void Query_Illegal_DateTime()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202103", Amount = 310}
                         });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 3, 1), new DateTime(2021, 2, 1));
            Assert.AreEqual(0, query);
        }

        [Test]
        public void Query_Cross_Month()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202102", Amount = 28},
                             new Budget {YearMonth = "202103", Amount = 310}
                         });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2021, 3, 3));
            Assert.AreEqual(32, query);
        }

        [Test]
        public void Query_Cross_Two_Months()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202102", Amount = 28},
                             new Budget {YearMonth = "202103", Amount = 310},
                             new Budget {YearMonth = "202104", Amount = 3000}
                         });
            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2021, 4, 3));
            Assert.AreEqual(612, query);
        }

        [Test]
        public void Query_Cross_Two_Months_WithNoDATA()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202102", Amount = 28},
                             // new Budget {YearMonth = "202103", Amount = 310},
                             new Budget {YearMonth = "202104", Amount = 3000}
                         });
            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2021, 4, 3));
            Assert.AreEqual(302, query);
        }

        [Test]
        public void Query_Cross_Year()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202102", Amount = 28},
                             // new Budget {YearMonth = "202103", Amount = 310},
                             new Budget {YearMonth = "202204", Amount = 3000}
                         });
            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2022, 4, 1));
            Assert.AreEqual(102, query);
        }

        [Test]
        [Category("fix month integer bug")]
        public void Query_Cross_Year_with_diff_month()
        {
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             // new Budget {YearMonth = "202102", Amount = 28},
                             new Budget {YearMonth = "202104", Amount = 3000}
                         });
            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2020, 4, 27), new DateTime(2021, 4, 2));
            Assert.AreEqual(200, query);
        }

        [Test]
        public void start_day_less_than_end_day()
        {
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll()
                .Returns(new List<Budget>
                         {
                             new Budget {YearMonth = "202103", Amount = 31},
                             new Budget {YearMonth = "202104", Amount = 3000}
                         });
            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 3, 27), new DateTime(2021, 4, 29));
            Assert.AreEqual(5 + 2900, query);
        }
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public int Days()
        {
            var firstDay = DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null);
            return DateTime.DaysInMonth(firstDay.Year, firstDay.Month);
        }

        public int DailyAmount()
        {
            return (Amount / Days());
        }
    }
}