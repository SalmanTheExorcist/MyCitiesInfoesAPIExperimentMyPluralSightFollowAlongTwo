namespace MyCitiesInfo.API.Models
{
    public class MyCitiesInfoDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<PointOfInterestDTO> MyCityPointOfInterests { get; set; } 
                                               = new List<PointOfInterestDTO>();
                

        //--This will be a calculated field
         public int NumberOfPointsOfInterest
        {
            get
            {
                return MyCityPointOfInterests.Count;
            }
            
        }

    }
}
