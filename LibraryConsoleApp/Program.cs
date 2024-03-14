using DataAccess;
using DataAccess.Entities;
using DataAccess.Repositories;
using LibraryConsoleApp.Handlers;
using Microsoft.EntityFrameworkCore;

namespace LibraryConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {

        ApplicationUI app = new ApplicationUI();
        await app.RunAsync();

    }

    private static void Exit()
    {
        Environment.Exit(0);
    }

}
