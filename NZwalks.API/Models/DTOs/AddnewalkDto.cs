using NZwalks.API.Models.Domains;

namespace NZwalks.API.Models.DTOs
{
    public class AddnewalkDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
