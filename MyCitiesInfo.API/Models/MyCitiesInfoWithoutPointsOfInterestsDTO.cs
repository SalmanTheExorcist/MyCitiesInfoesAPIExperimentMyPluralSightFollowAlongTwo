namespace MyCitiesInfo.API.Models
{
    /// <summary>
    /// A MyCity without Points-Of-Interests.
    /// </summary>
    public class MyCitiesInfoWithoutPointsOfInterestsDTO
    {

        /// <summary>
        /// The id of MyCity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of MyCity
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The description of MyCity
        /// </summary>
        public string? Description { get; set; }
    }
}
