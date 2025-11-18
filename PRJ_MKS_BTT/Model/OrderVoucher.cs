namespace PRJ_MKS_BTT.Model
{
    public class OrderVoucher
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int VoucherId { get; set; }
        public decimal DiscountAmount { get; set; }

        public Order Order { get; set; }
        public Voucher Voucher { get; set; }
    }
}
