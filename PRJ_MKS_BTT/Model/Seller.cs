namespace PRJ_MKS_BTT.Model
{
    public class Seller
    {
        public int SellerId { get; set; }
        public int UserId { get; set; }
        public string ShopName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public decimal Rating { get; set; }
        public int FollowerCount { get; set; }
        public int TotalSold { get; set; }
        public bool IsVerified { get; set; }
        public bool IsMall { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Voucher> Vouchers { get; set; }
    }
}
