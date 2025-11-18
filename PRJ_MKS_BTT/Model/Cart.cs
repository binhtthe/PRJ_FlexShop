namespace PRJ_MKS_BTT.Model
{
    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; }
        public ICollection<CartItem> Items { get; set; }
    }
}
