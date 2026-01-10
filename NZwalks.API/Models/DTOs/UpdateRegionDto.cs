using System.ComponentModel.DataAnnotations;

namespace NZwalks.API.Models.DTOs
{
    public class UpdateRegionDto
    {
        [Required(ErrorMessage = "Name of Region is required")]
        public required string Name { get; set; }

        [Required]
        [MaxLength(3, ErrorMessage = "code should be exactly 3 words")]
        [MinLength(3, ErrorMessage = "code should be exactly 3 words")]
        public required string Code { get; set; }


        public string? RegionImageUrl { get; set; }
    }
}
