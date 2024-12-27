using AIQueryGeneratorDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIQueryGeneratorDemo
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AuthorEntity> Authors { get; set; }

        public DbSet<BookEntity> Books { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorEntity>().HasKey(x => x.Id);

            modelBuilder.Entity<BookEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<BookEntity>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId);

            modelBuilder.Entity<OrderEntity>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderEntity>()
                .HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookId);
        }
    }
}
