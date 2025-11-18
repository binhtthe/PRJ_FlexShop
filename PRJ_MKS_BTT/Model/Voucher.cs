namespace PRJ_MKS_BTT.Model
{
    public class Voucher
    {
        public int VoucherId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal MaxDiscountAmount { get; set; }
        public decimal MinOrderValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
        public int SellerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Seller Seller { get; set; }
        public ICollection<OrderVoucher> OrderVouchers { get; set; }
    }
}
