using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using DAL.Models;

namespace DAL
{
    public class AllDbContext: DbContext
    {        


        public AllDbContext():base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CruisesDb;Integrated Security=True")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public DbSet<Models.Client> Clients { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.Profile> Profiles { get; set; }
        public DbSet<Models.Tour> Tours { get; set; }
        public DbSet<Models.Layner> Layners { get; set; }
        public DbSet<Models.Insurance> Insurances { get; set; }
        public DbSet<Models.Placement> Placements { get; set; }
        public DbSet<Models.Cabin> Cabins { get; set; }
    }
}
