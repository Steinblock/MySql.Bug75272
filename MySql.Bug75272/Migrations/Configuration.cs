namespace MySql.Bug75272.Migrations
{
    using Data.Entity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductsEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ProductsEntities context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            var categories = new[]
            {
                    new category { name = "Category 1" },
                    new category { name = "Category 2" },
                    new category { name = "Category 3" }
            };
            context.categories.AddOrUpdate(
                c => c.name, categories
            );

            if (context.products.Count() == 0)
            {
                context.Configuration.AutoDetectChangesEnabled = false;

                var rand = new Random();
                for (int i = 1; i <= 100000; i++)
                {
                    var product = new product
                    {
                        category = categories[rand.Next(categories.Count())],
                        name = "Product " + i.ToString(),
                    };
                    context.products.Add(product);

                    if (i % 1000 == 0)
                    {
                        context.ChangeTracker.DetectChanges();
                        context.SaveChanges();
                    }
                }

                context.ChangeTracker.DetectChanges();
                context.SaveChanges();
                context.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        
    }
}
