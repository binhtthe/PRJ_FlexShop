using System.ComponentModel.DataAnnotations;

namespace PRJ_MKS_BTT.Model
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public bool IsRevoked { get; set; }

        public User User { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
