namespace PRJ_MKS_BTT.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; } = null;

        public string Slug { get; set; }
        public string IconUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
