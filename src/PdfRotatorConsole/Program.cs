using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;
using PdfRotator;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ConsoleAppFramework;
using PdfRotatorConsole.Commands;

namespace PdfRotatorConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder().RunConsoleAppFrameworkAsync(args);
        }
    }
}
