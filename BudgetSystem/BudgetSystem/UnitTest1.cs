using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

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
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202103", Amount = 31}
            });

          var cal=  new BudgetCalculator(repo);
          var query = cal.Query(new DateTime(2021,3,1), new DateTime(2021, 3, 31));
          Assert.AreEqual(31, query);
        }
      
        [Test]
        public void Query_One_Day()
        {
            var budget = new Budget();
            IBudgetRepo repo = Substitute.For<IBudgetRepo>();
            repo.GetAll().Returns(new List<Budget>
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
            repo.GetAll().Returns(new List<Budget>
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
            repo.GetAll().Returns(new List<Budget>
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
            repo.GetAll().Returns(new List<Budget>
            {
                new Budget {YearMonth = "202102", Amount = 28},
                new Budget {YearMonth = "202103", Amount = 310}
            });

            var cal = new BudgetCalculator(repo);
            var query = cal.Query(new DateTime(2021, 2, 27), new DateTime(2021, 3, 3));
            Assert.AreEqual(32, query);
        }
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
    }
    
}
