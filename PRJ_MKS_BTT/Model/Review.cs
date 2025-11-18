namespace PRJ_MKS_BTT.Model
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public string MediaJson { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
