using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class BookRepository
	{
		private readonly LibraryDbContext _context;

		public BookRepository(LibraryDbContext context)
		{
			_context = context;
		}
		public async Task<Book> ReadOneAsync(long id)
		{

			var book = await (
				from b in _context.Books
				where b.Id == id
				select new Book
				{
					Id = b.Id,
					Title = b.Title,
					Genre = b.Genre,
					AuthorId = b.AuthorId,
					Author = new Author
					{
						Id = b.Author.Id,
						Name = b.Author.Name,
						Surname = b.Author.Surname
					}
				})
				.FirstOrDefaultAsync();

			return book;
		}

		public async Task<List<Book>> ReadAllAsync()
		{
			return await _context.Books
		   .Select(b => new Book
		   {
			   Id = b.Id,
			   Title = b.Title,
			   Genre = b.Genre,

			   Author = new Author
			   {
				   Id = b.Author.Id,
				   Name = b.Author.Name,
				   Surname = b.Author.Surname
			   }
		   })
		   .ToListAsync();
		}

		public async Task UpdateAsync(Book book)
		{
			var eBook = await _context.Books.FindAsync(book.Id);
			if (eBook == null)
			{
				throw new KeyNotFoundException($"Book with Id: {book.Id} not found!");

			}
			_context.Books.Entry(eBook).CurrentValues.SetValues(book);
			await _context.SaveChangesAsync();
		}


		public async Task CreateAsync(Book book)
		{
			if (string.IsNullOrWhiteSpace(book.Title) || ContainsNumbersOrSymbols(book.Title))
			{
				throw new Exception("Invalid Title format");
			}
			if (ContainsNumbersOrSymbols(book.Genre) || string.IsNullOrWhiteSpace(book.Genre))
			{
				throw new Exception("Invalid Genre Format");
			}

			_context.Books.Add(book);
			await _context.SaveChangesAsync();

		}

		public async Task DeleteAsync(long id)
		{
			Book book = await _context.Books.FirstOrDefaultAsync(a => a.Id == id);
			_context.Books.Remove(book);
			await _context.SaveChangesAsync();
		}

		public async Task<List<Book>> FindBooksByName(string keyWord)
		{
			return await _context.Books.Where(b => b.Title.Contains(keyWord)).ToListAsync();
		}

		private bool ContainsNumbersOrSymbols(string input)
		{
			return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^a-zA-Z\s]");
		}
	}
}
