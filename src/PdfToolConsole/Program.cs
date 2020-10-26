using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PdfToolConsole
{
    class Program : ConsoleAppBase
    {
        static async Task Main(string[] args)
        {
            args = EspcapeBackslash(args);

            await Host.CreateDefaultBuilder().RunConsoleAppFrameworkAsync(args);
        }

        private static string[] EspcapeBackslash(string[] args)
        {
            //pathにバックスラッシュが使われている場合にjson deserialize errorとなるので
            //バックスラッシュをエスケープする
            args = args.Select(a => Regex.Replace(a, @"\\", "\\\\")).ToArray();
            return args;
        }
    }
}
