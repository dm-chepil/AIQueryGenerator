namespace AIQueryGeneratorDemo.Entities
{
    public class AuthorEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<BookEntity> Books { get; set; }
    }
}
