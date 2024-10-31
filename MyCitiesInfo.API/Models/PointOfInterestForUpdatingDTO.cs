using System.ComponentModel.DataAnnotations;

namespace MyCitiesInfo.API.Models
{
    public class PointOfInterestForUpdatingDTO
    {
        [Required(ErrorMessage = "You should provide a Name value.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
