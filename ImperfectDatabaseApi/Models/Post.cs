namespace ImperfectDatabaseApi.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Author Author { get; set; } 
        public string Text { get; set; } = string.Empty;

        public ICollection<ImageLink> LinksToImages { get; set; } = new HashSet<ImageLink>();
    }

    public class Author
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string FullName { get; internal set; }
    }

    public class ImageLink
    {
        public string Url { get; set; } = string.Empty;
    }
}