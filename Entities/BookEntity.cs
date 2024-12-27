namespace AIQueryGeneratorDemo.Entities
{
    public class BookEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid AuthorId { get; set; }

        public AuthorEntity Author { get; set; }
    }
}
