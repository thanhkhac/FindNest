namespace FindNest.Params
{
    public class RentPostSearchParams : SearchParams
    {
        public string? Title { get; set; }
        public int? RegionId { get; set; }
        public List<int> RentCategoryIds { get; set; } = new List<int>();
        public long? MinPrice { get; set; }
        public long? MaxPrice { get; set; }
        
        public long? MinArea { get; set; }
        public long? MaxArea { get; set; }
        public string? Address { get; set; }
        public bool IsNegotiatedPrice { get; set; } = true;
        public bool IsPriceMinMaxFilter { get; set; } = true;

        public override string ToString()
        {
            return
                $"{nameof(Title)}: {Title}, {nameof(RegionId)}: {RegionId}, {nameof(RentCategoryIds)}: {RentCategoryIds}, {nameof(MinPrice)}: {MinPrice}, {nameof(MaxPrice)}: {MaxPrice}, {nameof(MinArea)}: {MinArea}, {nameof(MaxArea)}: {MaxArea}, {nameof(Address)}: {Address}, {nameof(IsNegotiatedPrice)}: {IsNegotiatedPrice}, {nameof(IsPriceMinMaxFilter)}: {IsPriceMinMaxFilter}";
        }
    }
}