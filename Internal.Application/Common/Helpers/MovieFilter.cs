namespace Internal.Application.Common.Helpers
{
    public class MovieFilter : BaseFilter
    {
        public float? Duration { get; set; }
        public int? Category { get; set; }
        public bool? RatingsAsc { get; set; }
    }
}
