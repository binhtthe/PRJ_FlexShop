namespace PRJ_MKS_BTT.Model
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }

        public Product Product { get; set; }
    }
}
