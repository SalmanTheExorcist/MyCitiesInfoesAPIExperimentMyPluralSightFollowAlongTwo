using MyCitiesInfo.API.Models;

namespace MyCitiesInfo.API
{
    public class MyCitiesInfoesDataStore
    {
        public List<MyCitiesInfoDTO> MyCitiesInfoes { get; set; }
      //  public static MyCitiesInfoesDataStore Current { get; } = new MyCitiesInfoesDataStore();

        public MyCitiesInfoesDataStore()
        {
            // init dummy data
            MyCitiesInfoes = new List<MyCitiesInfoDTO>()
            {
                new MyCitiesInfoDTO()
                {
                     Id = 1,
                     Name = "New York City",
                     Description = "The one with that big park.",
                     MyCityPointOfInterests = new List<PointOfInterestDTO>()
                     {
                         new PointOfInterestDTO() {
                             Id = 1,
                             Name = "Central Park",
                             Description = "The most visited urban park in the United States." },
                          new PointOfInterestDTO() {
                             Id = 2,
                             Name = "Empire State Building",
                             Description = "A 102-story skyscraper located in Midtown Manhattan." },
                     }
                },
                new MyCitiesInfoDTO()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished.",
                    MyCityPointOfInterests = new List<PointOfInterestDTO>()
                     {
                         new PointOfInterestDTO() {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PointOfInterestDTO() {
                             Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium." },
                     }
                },
                new MyCitiesInfoDTO()
                {
                    Id= 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                    MyCityPointOfInterests = new List<PointOfInterestDTO>()
                     {
                         new PointOfInterestDTO() {
                             Id = 5,
                             Name = "Eiffel Tower",
                             Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." },
                          new PointOfInterestDTO() {
                             Id = 6,
                             Name = "The Louvre",
                             Description = "The world's largest museum." },
                     }
                }
            };



        }

    }
}
