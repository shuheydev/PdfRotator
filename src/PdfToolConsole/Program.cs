using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Text;
using PdfTool;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ConsoleAppFramework;
using PdfToolConsole.Commands;

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
