using System;
using System.IO;

namespace PdfToolConsole.Tests
{
    namespace PdfToolConsoleTest
    {
        public abstract class PdfToolConsoleTestBase : IDisposable
        {
            protected const string _testDataDir = "../../../TestData";
            protected const string _existFileName = "PdfRotateTest.pdf";
            protected string _existFilePath;
            protected const string _outputFileName = "output.pdf";
            protected string _outputFilePath;

            public void Dispose()
            {
                if (File.Exists(_outputFilePath))
                {
                    try
                    {
                        File.Delete(_outputFilePath);
                    }
                    catch
                    {
                        //
                    }
                }
            }
        }
    }
}
