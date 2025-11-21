namespace PRJ_MKS_BTT.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public int CategoryId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Status { get; set; }
        public decimal Weight { get; set; }

        public int ViewCount { get; set; }
        public int SoldCount { get; set; }
        public int StockCount { get; set; }

        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsFreeShipping { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Seller Seller { get; set; }
        public Category Category { get; set; }
        public ICollection<Sku> Skus { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
    }
}
