namespace PRJ_MKS_BTT.Response
{
    public class RegisterResponse
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public string Message { get; set; } = "Registration successful. Please verify your email.";
        public bool EmailSent { get; set; } = true;
    }

}
