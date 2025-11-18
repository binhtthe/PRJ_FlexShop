using System.ComponentModel.DataAnnotations;

namespace PRJ_MKS_BTT.Model
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int SkuId { get; set; }
        public int ProductId { get; set; }

        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public string ProductTitle { get; set; }
        public string ProductImage { get; set; }
        public string SkuAttributes { get; set; }

        public Order Order { get; set; }
        public Sku Sku { get; set; }
        public Product Product { get; set; }
    }
}
