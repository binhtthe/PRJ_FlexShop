namespace PRJ_MKS_BTT.Model
{
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }

        public int BuyerId { get; set; }
        public int ShippingAddressId { get; set; }

        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; }
        public string BuyerNote { get; set; }
        public string CancelReason { get; set; }
        public int? CancelledBy { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public User Buyer { get; set; }
        public UserAddress ShippingAddress { get; set; }
        public ICollection<OrderItem> Items { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Shipment> Shipments { get; set; }
        public ICollection<OrderVoucher> OrderVouchers { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
