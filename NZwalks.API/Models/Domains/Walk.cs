namespace NZwalks.API.Models.Domains
{
    public class Walk
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LengthInKm { get; set; }

        public string? WalkImageUrl { get; set; }


        public Guid DifficultyId { get; set; }

        public Guid RegionId { get; set; }


        //navigation properties

        public Difficulty difficulty { get; set; }
        public Region Region { get; set; }



    }

}
