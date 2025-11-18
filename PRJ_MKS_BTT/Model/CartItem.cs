namespace PRJ_MKS_BTT.Model
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int SkuId { get; set; }
        public int Qty { get; set; }
        public DateTime AddedAt { get; set; }

        public Cart Cart { get; set; }
        public Sku Sku { get; set; }
    }
}
