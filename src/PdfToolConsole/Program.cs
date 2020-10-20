using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace PdfToolConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder().RunConsoleAppFrameworkAsync(args);
        }
    }
}
