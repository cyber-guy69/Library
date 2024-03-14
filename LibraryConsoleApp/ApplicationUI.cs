using DataAccess.Repositories;
using LibraryConsoleApp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryConsoleApp
{
    public class ApplicationUI
    {
        private readonly AuthorsHandler _authorsHandler;
        private readonly BooksHandler _booksHandler;

        public ApplicationUI()
        {

            LibraryDbContextFactory dbcf = new LibraryDbContextFactory();//pasiema is appsettings.json failo reikiamus duomenys susijungti su duombaze
            var context = new LibraryDbContextFactory().CreateDbContext([]); //sukuriamas librarydbcontext objektas, kuris bendrauja su duombaze

            AuthorRepository authorRepository = new AuthorRepository(context); //Is repositories paduodamas kontekstas i repositories, kad jos galetu bendrauti su duomenu baze
            BookRepository bookRepository = new BookRepository(context);

            _authorsHandler = new AuthorsHandler(authorRepository);//Atsakingas uz while su swich kad authorepositories galetu irasyti, gaut duomenis is panasiai
            _booksHandler = new BooksHandler(bookRepository, authorRepository);
        }


        public async Task RunAsync()
        {

            await Console.Out.WriteLineAsync();
            while (true)
            {
                Console.Clear();
                await Console.Out.WriteLineAsync("Welcome to Library, choose authors ir books:");
                startMeniu();
                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        await _authorsHandler.HandleAsync();
                        break;
                    case "2":
                        await _booksHandler.HandleAsync();
                        break;
                    case "3":
                        exit();
                        break;

                }

            }

        }


        public void exit()
        {
            Environment.Exit(0);
        }

        public void startMeniu()
        {
            Console.WriteLine("1. Authors meniu");
            Console.WriteLine("2. Books meniu");
            Console.WriteLine("3. Exit");
        }







    }


}
