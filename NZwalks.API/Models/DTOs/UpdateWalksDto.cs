using NZwalks.API.Models.Domains;

namespace NZwalks.API.Models.DTOs
{
    public class UpdateWalksDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }

        public Guid DifficultyId { get; set; }

        public Guid RegionId { get; set; }


    }
}
