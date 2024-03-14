using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryConsoleApp.Handlers
{
    public class BooksHandler
    {
        private readonly BookRepository _bookRepository;
        private readonly AuthorRepository _authorRepository;



        public BooksHandler(BookRepository booksRepository, AuthorRepository authorRepository)
        {
            _bookRepository = booksRepository;
            _authorRepository = authorRepository;

        }

        public async Task HandleAsync()
        {

            await Console.Out.WriteLineAsync("Books section:");
            bool main = true;
            while (main)
            {
                Console.WriteLine();

                bookMeniu();
                string input = Console.ReadLine();
                switch (input)
                {

                    case "1":
                        await CreateBook();
                        break;
                    case "2":
                        await ListAllBooks();
                        break;
                    case "3":
                        await UpdateBook();
                        break;
                    case "4":
                        await DeleteBook();
                        break;
                    case "5":
                        await ListOneBook();
                        break;
                    case "6":
                        await FindBooksByTitle();
                        break;
                    case "7":
                        main = false;
                        break;
                    case "8":
                        Exit();
                        break;

                }
            }

        }

        private async Task ListAllBooks()
        {
            Console.Clear();
            List<Book> books = await _bookRepository.ReadAllAsync();

            books = books.OrderBy(x => x.Id).ToList();
            foreach (Book book in books)
            {

                await Console.Out.WriteLineAsync($"{book.Id,-3}:  {book.Title,-30}  {book.Genre,-30}");

            }
        }

        private async Task CreateBook()
        {

            await Console.Out.WriteLineAsync("You are in book creation meniu. [Q] for exit.");
            await _authorRepository.ReadAllAsync();
            string input = Console.ReadLine();
            if (input.ToLower().Equals("q")) return;

            await Console.Out.WriteLineAsync("Enter new book name: ");
            string bookName = Console.ReadLine();

            await Console.Out.WriteLineAsync("Enter new book genre: ");
            string bookGenre = Console.ReadLine();


            await Console.Out.WriteLineAsync("1. Add author id from the list:");
            await Console.Out.WriteLineAsync("2. Create author and add to the book:");
            await Console.Out.WriteLineAsync("3. [q] for exiting ");

            long authorId = 0;
            while (true)
            {
                string inputForSwitch = Console.ReadLine();
                switch (inputForSwitch)
                {
                    case "1":
                        List<Author> authors = await _authorRepository.ReadAllAsync();
                        foreach (Author auth in authors)
                        {
                            await Console.Out.WriteLineAsync($"Id: {auth.Id} Name: {auth.Name} Surname: {auth.Surname}");
                        }
                        await Console.Out.WriteLineAsync("Select authors id from the list");
                        string inputId = Console.ReadLine();
                        if (long.TryParse(inputId, out long id))
                        {
                            Author auth = await _authorRepository.ReadOneAsync(id);
                            authorId = auth.Id;

                        }
                        break;
                    case "2":
                        await Console.Out.WriteLineAsync("Enter author name: ");
                        string name = Console.ReadLine();
                        if (name.ToLower().Equals("q")) return;

                        await Console.Out.WriteLineAsync("Enter author Surname: ");
                        string surname = Console.ReadLine();
                        if (surname.ToLower().Equals("q")) return;

                        var author = new Author
                        {
                            Name = name,
                            Surname = surname
                        };
                        await _authorRepository.CreateAsync(author);
                        authorId = author.Id;
                        break;
                }
                var book = new Book
                {
                    Title = bookName,
                    Genre = bookGenre,
                    AuthorId = authorId
                };

                await _bookRepository.CreateAsync(book);
                break;
            }





        }

        private async Task UpdateBook()
        {
            List<Book> books = await _bookRepository.ReadAllAsync();
            await ListAllBooks();
            await Console.Out.WriteLineAsync("--------------------------------------------");
            await Console.Out.WriteLineAsync("Select book [id] you want to change/update: ");
            string input = Console.ReadLine();
            long.TryParse(input, out long id);
            bool found = false;
            foreach (Book book in books)
            {
                if (book.Id == id)
                {
                    found = true;
                    await Console.Out.WriteLineAsync("Enter new books title: ");
                    book.Title = await Console.In.ReadLineAsync();
                    await Console.Out.WriteLineAsync("Enter new books genre: ");
                    book.Genre = await Console.In.ReadLineAsync();


                    await _bookRepository.UpdateAsync(book);
                }

            }
            if (!found)
            {
                await Console.Out.WriteLineAsync("Theres no book with such Id.");
            }

        }

        private async Task DeleteBook()
        {
            List<Book> books = await _bookRepository.ReadAllAsync();
            await ListAllBooks();
            string input = Console.ReadLine();
            long.TryParse(input, out long id);
            bool ifBookFound = false;
            foreach (Book book in books)
            {
                if (id == book.Id)
                {
                    await _bookRepository.DeleteAsync(book.Id);
                    ifBookFound = true;
                    break;
                }
            }
            if (!ifBookFound)
            {
                await Console.Out.WriteLineAsync($"There is no such book with id: {id}");

            }

        }

        private async Task ListOneBook()
        {

            List<Book> books = await _bookRepository.ReadAllAsync();
            books = books.OrderBy(book => book.Id).ToList();
            foreach (Book b in books)
            {
                await Console.Out.WriteLineAsync($"[{b.Id}] {b.Title}");
            }
            await Console.Out.WriteLineAsync("Choose book Id to see its author: ");
            string input = Console.ReadLine();
            long.TryParse(input, out long id);
            var book = await _bookRepository.ReadOneAsync(id);

            if (book != null)
            {
                await Console.Out.WriteLineAsync($"Author Id: {book.Author.Id} Name: {book.Author.Name} Surname: {book.Author.Surname}");

            }
            else
            {
                await Console.Out.WriteLineAsync("Book not found!");
            }

        }


        public async Task FindBooksByTitle()
        {
            Console.Clear();
            await Console.Out.WriteLineAsync("Enter book title you want to find");
            string keyWord = Console.ReadLine();
            List<Book> books = await _bookRepository.FindBooksByName(keyWord);
            await Console.Out.WriteLineAsync();
            if (books != null)
            {
                foreach (Book book in books)
                {
                    Console.Out.WriteLine("|--------------------------|");
                    Console.Out.WriteLine($"Id: {book.Id,-5} Title: {book.Title}");
                    if (book.Author != null)
                    {
                        Console.WriteLine(book.Author.Name);
                    }

                }
            }
            else
            {
                Console.Out.WriteLine("No matching to provided Title");
            }
        }


        private void Exit()
        {
            Environment.Exit(0);
        }

        private void bookMeniu()
        {

            Console.WriteLine("-----------------------");
            Console.WriteLine("1. Create book");
            Console.WriteLine("2. List all books");
            Console.WriteLine("3. Update book");
            Console.WriteLine("4. Delete book");
            Console.WriteLine("5. List author of Book");
            Console.WriteLine("6. Find Books by title");
            Console.WriteLine("7. Main Menu");
            Console.WriteLine("8. Exit");
        }







    }
}
