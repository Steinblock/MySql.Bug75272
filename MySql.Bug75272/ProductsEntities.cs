using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySql.Bug75272
{
    public class ProductsEntities : DbContext
    {
        public DbSet<product> products { get; set; }
        public DbSet<category> categories { get; set; }

        static ProductsEntities()
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class product
    {
        // Change type int to long generate Bug "FROM (SELECT"
        public long id { get; set; }
        public string name { get; set; }
        [Required]
        public virtual category category { get; set; }
    }

    // For especific column select
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class productProxy : product {

    }

    public class category
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    // For especific column select
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public class categoryProxy : category {

    }
}
