namespace FindNest.Params
{
    public class RentPostSearchParams
    {
        public string? Title { get; set; }
        public int? RegionId { get; set; }
        public int? RentCategoryId { get; set; }
        public long? FromPrice { get; set; }
        public long? ToPrice { get; set; }
        public string? Address { get; set; }
        public bool IsNegotiatedPrice { get; set; }
    }
}