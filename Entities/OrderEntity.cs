namespace AIQueryGeneratorDemo.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }

        public Guid BookId { get; set; }

        public int Quantity { get; set; }

        public DateTime DateTime { get; set; }

        public BookEntity Book { get; set; }
    }
}
