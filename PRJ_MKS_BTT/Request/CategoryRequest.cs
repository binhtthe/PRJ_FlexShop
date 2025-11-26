namespace PRJ_MKS_BTT.Request
{
    public class CategoryRequest
    {
    
        public int? ParentId { get; set; } = null;
        public string Name { get; set; }
        public string Slug { get; set; }
        public string IconUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
