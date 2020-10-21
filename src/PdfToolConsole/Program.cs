using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using PdfToolConsole.Commands;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace PdfToolConsole
{
    class Program : ConsoleAppBase
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder().RunConsoleAppFrameworkAsync(args);
        }
    }
}
