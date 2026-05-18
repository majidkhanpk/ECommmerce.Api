namespace ECommmerce.Api.DTOs
{
    public class ProductQueryParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public string? Search { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string? SortBy { get; set; }

        public bool? descending { get; set; } = false;

    }
}
