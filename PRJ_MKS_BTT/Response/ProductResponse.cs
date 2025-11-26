namespace PRJ_MKS_BTT.Response
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public decimal Weight { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsFreeShipping { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<ImageResponse> Images { get; set; } = new();
        public List<SkuResponse> Skus { get; set; } = new();
    }

    public class ImageResponse
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
    }

    public class SkuResponse
    {
        public int SkuId { get; set; }
        public string SkuCode { get; set; }
        public decimal Price { get; set; }
        public decimal? CompareAtPrice { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
    }

}
