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
          var query = cal.query(new DateTime(2021,3,1), new DateTime(2021, 3, 31));
          Assert.AreEqual(31, query);
        }
    }

    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }
    }
    
}
