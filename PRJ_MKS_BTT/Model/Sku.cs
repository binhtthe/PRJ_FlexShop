namespace PRJ_MKS_BTT.Model
{
    public class Sku
    {
        public int SkuId { get; set; }
        public int ProductId { get; set; }

        public string SkuCode { get; set; }
        public decimal Price { get; set; }
        public decimal CompareAtPrice { get; set; }
        public int Stock { get; set; }
        public string AttributesJson { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }

        public Product Product { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
