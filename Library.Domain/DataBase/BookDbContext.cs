using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.DataBase
{
	public class BookDbContext : DbContext
	{
		public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) 
		{
				
		}
		public DbSet<Books> Books { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Configure entity mappings if needed
			modelBuilder.Entity<Books>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
				entity.Property(e => e.Author).IsRequired().HasMaxLength(200);
				entity.Property(e =>  e.ISBN).IsRequired().HasMaxLength(200);
			});

			// Seed data
			SeedData(modelBuilder);
		}

		private void SeedData(ModelBuilder modelBuilder)
		{
			// Seed Books
			modelBuilder.Entity<Books>().HasData(
				new Books
				{
					Id = 1,
					Title = "The Great Gatsby",
					Author = "F. Scott Fitzgerald",
					ISBN = "9780743273565",
					PublishedDate = DateTime.UtcNow
				},
				new Books
				{
					Id = 2,
					Title = "To Kill a Mockingbird",
					Author = "Harper Lee",
					ISBN = "9780061120084",
					PublishedDate = DateTime.UtcNow
				},
				new Books
				{
					Id = 3,
					Title = "Taiwo Ofere's Code",
					Author = "Ofere Taiyelolu",
					ISBN = "9780451524935",
					PublishedDate = DateTime.UtcNow
				}
			);

		}
	}

}
