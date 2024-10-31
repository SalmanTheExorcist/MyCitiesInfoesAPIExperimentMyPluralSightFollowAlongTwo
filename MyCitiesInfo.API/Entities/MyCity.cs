using MyCitiesInfo.API.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyCitiesInfo.API.Entities
{
    public class MyCity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        public ICollection<PointOfInterest> MyCityPointOfInterests { get; set; }
                                               = new List<PointOfInterest>();

        public MyCity(string name)
        {
            Name = name;
        }
    }
}
