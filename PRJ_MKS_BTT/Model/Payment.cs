namespace PRJ_MKS_BTT.Model
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }

        public string Gateway { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Order Order { get; set; }
    }
}
