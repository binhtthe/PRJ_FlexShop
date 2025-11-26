namespace PRJ_MKS_BTT.Request
{

    public class ProductRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public int CategoryId { get; set; }
        public decimal Weight { get; set; }
        public bool IsFeatured { get; set; } = false;
        public bool IsFreeShipping { get; set; } = false;

        public List<ImageRequest> Images { get; set; } = new();
        public List<SkuRequest> Skus { get; set; } = new();
    }

    public class ImageRequest
    {
        public string Url { get; set; }
        public int SortOrder { get; set; } = 0;
    }

    public class SkuRequest
    {
        
        public string SkuCode { get; set; }
        public decimal Price { get; set; }
        public decimal? CompareAtPrice { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
    }

}
