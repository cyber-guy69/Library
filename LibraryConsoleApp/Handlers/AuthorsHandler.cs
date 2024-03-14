using DataAccess.Entities;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace LibraryConsoleApp.Handlers;

public class AuthorsHandler
{
    private readonly AuthorRepository _authorRepository;
    public AuthorsHandler(AuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }
    public async Task HandleAsync()
    {
        Console.Clear();
        await Console.Out.WriteLineAsync("Author section:");
        bool backToMainPage = true;
        while (backToMainPage)
        {
            Console.WriteLine();
            Console.WriteLine("|--------------------------|");

            authorMeniu();
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    await ListAllAuthors();
                    break;
                case "2":
                    await UpdateAuthor();
                    break;
                case "3":
                    await CreateAuthor();
                    break;
                case "4":
                    await DeleteAuthor();
                    break;
                case "5":
                    await FindOneAuthorAndHesBooks();
                    break;
                case "6":
                    await FindAuthorsByName();
                    break;
                case "7":
                    backToMainPage = false;
                    break;
            }
        }
    }
    private async Task ListAllAuthors()
    {
        List<Author> authors = await _authorRepository.ReadAllAsync();
        await Console.Out.WriteLineAsync($"{"Id",-4} {"Name",-20} {"Surname",-20}");
        foreach (Author author in authors)
        {
            await Console.Out.WriteLineAsync($"{author.Id,-4} {author.Name,-20} {author.Surname,-20} ");
        }
    }

    private async Task UpdateAuthor()
    {
        Console.Clear();
        List<Author> authors = await _authorRepository.ReadAllAsync();
        await Console.Out.WriteLineAsync($"{"Id",-4} {"Name",-20} {"Surname",-20}");
        foreach (Author author in authors)
        {
            await Console.Out.WriteLineAsync($"{author.Id,-4} {author.Name,-20} {author.Surname,-20} ");
        }

        await Console.Out.WriteLineAsync("Enter author Id. Or press [q] for exit, or Enter for continue:)");
        while (true)
        {

            string input = await Console.In.ReadLineAsync();

            if (!input.Equals("q") && !long.TryParse(input, out _))
            {
                await Console.Out.WriteLineAsync("Invalid input. Please enter a valid author Id or [q] for exit.");
                continue;
            }
            if (input.Equals("q"))
            {
                break;
            }

            if (long.TryParse(input, out long id))
            {
                foreach (Author author in authors)
                {

                    if (author.Id == id)
                    {

                        await Console.Out.WriteLineAsync("Enter new author name: ");
                        author.Name = await Console.In.ReadLineAsync();
                        await Console.Out.WriteLineAsync("Enter new Surname or leave empty and press [Enter]");
                        author.Surname = await Console.In.ReadLineAsync();
                        await _authorRepository.UpdateAsync(author);
                        return;

                    }



                }
                await Console.Out.WriteLineAsync($"There is no author with this Id: {id}");


            }



        }



    }

    private async Task CreateAuthor()
    {

        await Console.Out.WriteLineAsync("Welcome, here you can add your author to library");

        await Console.Out.WriteLineAsync("Enter [q] for exit in every step. [Enter] for continue");
        string input = Console.ReadLine();
        if (input.ToLower().Equals("q")) return;

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



    }

    private async Task DeleteAuthor()
    {
        List<Author> authors = await _authorRepository.ReadAllAsync();
        ListAllAuthors();
        await Console.Out.WriteLineAsync("SELECT author [ID] you want to delete, or PRESS [q] for EXIT");

        while (true)
        {
            string input = Console.ReadLine();
            long.TryParse(input, out long id);

            if (input.ToLower().Equals("q"))
            {
                return;

            }

            Author author = authors.FirstOrDefault(a => a.Id == id);
            if (author == null || author.Books.Any())
            {
                Console.WriteLine("Author not found or Author has a book and cannot be deleted. [Q] for exit");
                continue;
            }
            else
            {
                await _authorRepository.DeleteAsync(id);
                await Console.Out.WriteLineAsync($"Author: {author.Name} {author.Surname} was deleted successfully");
                return;
            }
        }

    }



    private async Task FindOneAuthorAndHesBooks()
    {
        Console.Clear();
        List<Author> authors = await _authorRepository.ReadAllAsync();
        foreach (Author auth in authors)
        {
            await Console.Out.WriteLineAsync($"[{auth.Id}] " + auth.Name + " " + auth.Surname);
        }
        await Console.Out.WriteLineAsync("Chose author to see hes books: ");
        string input = Console.ReadLine();
        int.TryParse(input, out int id);
        var author = await _authorRepository.ReadOneAsync(id);
        if (author != null)
        {
            foreach (var book in author.Books)
            {
                await Console.Out.WriteLineAsync($"{book.Title},  {book.Genre}");

            }
        }
        else
        {
            await Console.Out.WriteLineAsync("Author does not exist");

        }


    }

    private async Task FindAuthorsByName()
    {
        await Console.Out.WriteLineAsync("Enter author name you want to find");
        string keyWord = Console.ReadLine();
        List<Author> authors = await _authorRepository.FindAuthorsByName(keyWord);
        await Console.Out.WriteLineAsync();
        if (authors.Count != 0)
        {
            foreach (Author auth in authors)
            {
                Console.Out.WriteLine("|--------------------------|");
                Console.Out.WriteLine($"Id: {auth.Id} Author:{auth.Name} {auth.Surname}");
                Console.Out.WriteLine("Authors books:");

                foreach (var book in auth.Books)
                {
                    await Console.Out.WriteLineAsync($"{book.Title,-50} | {book.Genre,-20}");

                }
            }
        }
        else
        {
            Console.Out.WriteLine("No matching to provided name or surname");
        }
    }

    private void authorMeniu()
    {

        Console.WriteLine("1. List all authors");
        Console.WriteLine("2. Update author");
        Console.WriteLine("3. Create author");
        Console.WriteLine("4. Delete author");
        Console.WriteLine("5. All authors books");
        Console.WriteLine("6. Search Author by name");
        Console.WriteLine("7. Back to main menu");
    }


}
