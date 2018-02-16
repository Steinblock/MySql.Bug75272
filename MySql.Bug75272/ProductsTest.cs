using System;
using System.Linq;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace MySql.Bug75272
{
    [TestClass]
    public class ProductsTest
    {
        [TestMethod]
        public void CanGetProducts()
        {

            // https://bugs.mysql.com/bug.php?id=75272
            // Poor performance with Entity Framework Include Queries and Order By

            Action<string> log = msg =>
            {
                Console.WriteLine(msg);
                if (msg.ToUpper().Contains("FROM (SELECT"))
                    Assert.Fail("query should not contain a subselect: " + msg);
            };

            using (var context = new ProductsEntities())
            {

                context.Database.Log = q => Console.WriteLine(q);

                //var products1 = context.products
                //    .OrderBy(x => x.id)
                //    .Take(10)
                //    .ToList();
                
                var products2 = context.products
                    .Include(x => x.category)
                    .Select(x => new productProxy {
                        id = x.id,
                        name = x.name,
                        category = new categoryProxy {
                            name = x.name
                        }
                    })
                    .OrderBy(x => x.id)
                    .Take(10)
                    .ToList();

            }

        }

    }
}
