using AIQueryGeneratorDemo.Entities;
using AIQueryGeneratorDemo.Models;
using AIQueryGeneratorDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AIQueryGeneratorDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController(AppDbContext dbContext, IAssistantService assistantService) : ControllerBase
    {
        [HttpPost("seed")]
        public async Task<IActionResult> SeedBooksData()
        {
            dbContext.Authors.AddRange(
                new AuthorEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Mark Twain",
                    Books = new List<BookEntity>
                    {
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "The Adventures of Tom Sawyer\r\n",
                        },
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Adventures of Huckleberry Finn",
                        },
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "The Prince and the Pauper",
                        },
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Life on the Mississippi",
                        },
                    },
                },
                new AuthorEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "Jane Austen",
                    Books = new List<BookEntity>
                    {
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Pride and Prejudice",
                        },
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Sense and Sensibility",
                        },
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Emma",
                        },
                    }
                },
                new AuthorEntity
                {
                    Id = Guid.NewGuid(),
                    Name = "George Orwell",
                    Books = new List<BookEntity>
                    {
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Animal Farm",
                        },
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "Homage to Catalonia",
                        },
                        new BookEntity
                        {
                            Id = Guid.NewGuid(),
                            Title = "The Road to Wigan Pier",
                        },
                    }
                });

            await dbContext.SaveChangesAsync();

            dbContext.Orders.AddRange(
                new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = dbContext.Books.First().Id,
                    Quantity = 2,
                    DateTime = DateTime.Today.AddDays(-10),
                },
                new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = dbContext.Books.Skip(1).First().Id,
                    Quantity = 6,
                    DateTime = DateTime.Today.AddDays(-20),
                },
                new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = dbContext.Books.Skip(2).First().Id,
                    Quantity = 1,
                    DateTime = DateTime.Today.AddDays(-1),
                },
                new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = dbContext.Books.Skip(3).First().Id,
                    Quantity = 15,
                    DateTime = DateTime.Today.AddDays(-5),
                },
                new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = dbContext.Books.Skip(4).First().Id,
                    Quantity = 7,
                    DateTime = DateTime.Today.AddDays(-5),
                },
                new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = dbContext.Books.Skip(5).First().Id,
                    Quantity = 3,
                    DateTime = DateTime.Today.AddDays(-15),
                },
                new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = dbContext.Books.Skip(6).First().Id,
                    Quantity = 2,
                    DateTime = DateTime.Today.AddDays(-35),
                },
                new OrderEntity
                {
                    Id = Guid.NewGuid(),
                    BookId = dbContext.Books.Skip(7).First().Id,
                    Quantity = 7,
                    DateTime = DateTime.Today.AddDays(-8),
                });

            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("query")]
        public async Task<IActionResult> ProcessQuery([FromBody]string query)
        {
            var schema = Helper.GenerateSchema(dbContext);
            var assistantResponse = await assistantService.ExecuteQuery(query, schema);
            var queryResponse = JsonConvert.DeserializeObject<QueryResponse>(assistantResponse);
            var books = await dbContext.Books.FromSqlRaw(queryResponse.Query).ToListAsync();

            return Ok(books);
        }
    }
}
