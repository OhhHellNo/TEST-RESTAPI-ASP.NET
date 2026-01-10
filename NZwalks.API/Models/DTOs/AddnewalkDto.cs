using NZwalks.API.Models.Domains;
using System.ComponentModel.DataAnnotations;

namespace NZwalks.API.Models.DTOs
{
    public class AddnewalkDto
    {
        
        [Required(ErrorMessage = "Name of walk is required")]
        [MaxLength(30, ErrorMessage = "walk name should not exceed 30 chars")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "description for walk is needed")]
        public required string Description { get; set; }


        [Required(ErrorMessage = "walk length is requieed.")]
        [Range(0, 50, ErrorMessage = "range between 0 to 50")]
        public int LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        [Required]
        public required RegionDto Region { get; set; }

        [Required]
        public required DifficultyDto Difficulty { get; set; }
    }
}
