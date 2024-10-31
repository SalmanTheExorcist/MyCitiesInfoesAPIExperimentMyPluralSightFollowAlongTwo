using Microsoft.EntityFrameworkCore;
using MyCitiesInfo.API.Entities;

namespace MyCitiesInfo.API.DbContexts
{
    public class MyCitiesInfoContext : DbContext
    {




        public DbSet<MyCity> MyCities { get; set; }

        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        //--------------------------------------------
        //--Constructor below:
        public MyCitiesInfoContext(DbContextOptions<MyCitiesInfoContext> dbContextOptions)
            : base(dbContextOptions)
        {
            
        }

        //--------------------------------------------
        //---Seeding the database with data below:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //--Adding few MyCity Data:
            modelBuilder.Entity<MyCity>()
               .HasData(
              new MyCity("New York City")
              {
                  Id = 1,
                  Description = "The one with that big park."
              },
              new MyCity("Antwerp")
              {
                  Id = 2,
                  Description = "The one with the cathedral that was never really finished."
              },
              new MyCity("Paris")
              {
                  Id = 3,
                  Description = "The one with that big tower."
              });

            //--Adding few Points-Of-Interest Data:
            modelBuilder.Entity<PointOfInterest>()
             .HasData(
               new PointOfInterest("Central Park")
               {
                   Id = 1,
                   MyCityId = 1,
                   Description = "The most visited urban park in the United States."

               },
               new PointOfInterest("Empire State Building")
               {
                   Id = 2,
                   MyCityId = 1,
                   Description = "A 102-story skyscraper located in Midtown Manhattan."
               },
                 new PointOfInterest("Cathedral")
                 {
                     Id = 3,
                     MyCityId = 2,
                     Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                 },
               new PointOfInterest("Antwerp Central Station")
               {
                   Id = 4,
                   MyCityId = 2,
                   Description = "The the finest example of railway architecture in Belgium."
               },
               new PointOfInterest("Eiffel Tower")
               {
                   Id = 5,
                   MyCityId = 3,
                   Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
               },
               new PointOfInterest("The Louvre")
               {
                   Id = 6,
                   MyCityId = 3,
                   Description = "The world's largest museum."
               }
               );




            base.OnModelCreating(modelBuilder);
        }






        //--------------------------------------------
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{

        //    optionsBuilder.UseSqlite("connectionString");

        //    base.OnConfiguring(optionsBuilder);
        //}

    }//--End-Class
}//--End-Namespace
