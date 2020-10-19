using System;
using Xunit;
using PdfRotator;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;

namespace PdfRotator.Tests
{
    namespace PdfTests
    {
        public abstract class PdfTestBase : IDisposable
        {
            protected Pdf _pdf;
            protected string _testDataDir;
            protected const string _existFileName = "PdfRotateTest.pdf";
            protected string _existFilePath;
            protected const string _notExistFileName = "NotExistFile";
            protected const string _notExistFilePath = "NotExistFile";
            protected string _outputFileName;
            protected string _outputFilePath;

            public void Init(string id)
            {
                _testDataDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                _testDataDir = "../../../TestData";
                if (!Directory.Exists(_testDataDir))
                    throw new DirectoryNotFoundException($"'{_testDataDir}' not found.");

                _existFilePath = Path.Combine(_testDataDir, _existFileName);
                if (!File.Exists(_existFilePath))
                    throw new FileNotFoundException($"'{_existFilePath}' not found.");

                _outputFileName = $"{id}_output.pdf";
                _outputFilePath = Path.Combine(_testDataDir, _outputFileName);
            }

            public void Dispose()
            {
                if (File.Exists(_outputFilePath))
                    File.Delete(_outputFilePath);
            }
        }

        public class Count_Tests : PdfTestBase
        {
            public Count_Tests()
            {
                Init(nameof(Count_Tests));
            }

            [Fact]
            public void ページ数5のPDFファイルは5を返す()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageCountAtReading = _pdf.Count();

                //Act
                int pageCount = _pdf.Count();

                //Assert
                Assert.Equal(5, pageCount);
                Assert.Equal(pageCountAtReading, pageCount);
            }
        }

        public class Write_Tests : PdfTestBase
        {
            public Write_Tests()
            {
                Init(nameof(Write_Tests));
            }

            [Fact]
            public void PDFを保存する()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 1;
                int degree = -90;
                _pdf.Rotate(pageNumber, degree);

                //Act
                _pdf.Write(_outputFilePath);

                //Assert
                Assert.True(File.Exists(_outputFilePath));
                var pdf = new Pdf(_outputFilePath);
                Assert.Equal(degree, pdf.GetPageRotate(pageNumber));
            }
        }

        public class Rotate_Tests : PdfTestBase
        {
            public Rotate_Tests()
            {
                Init(nameof(Rotate_Tests));
            }

            [Fact]
            public void ページ1の向きdegreeは0()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                //Assert
                Assert.Equal(0, _pdf.GetPageRotate(1));
            }

            [Fact]
            public void ページ1を右に90degree回転する()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 1;
                int degree = 90;

                //Act
                _pdf.Rotate(pageNumber, degree);
                int rotate = _pdf.GetPageRotate(pageNumber);

                //Assert
                Assert.Equal(degree, rotate);
            }

            [Fact]
            public void ページ1を左に90degree回転する()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 1;
                int degree = -90;

                //Act
                _pdf.Rotate(pageNumber, degree);
                int rotate = _pdf.GetPageRotate(pageNumber);

                //Assert
                Assert.Equal(degree, rotate);
            }
        }

        public class Read_Tests : PdfTestBase
        {
            public Read_Tests()
            {
                Init(nameof(Read_Tests));
            }

            [Fact]
            public void Pdfファイルの読み込みに成功する()
            {
                //Assert
                _pdf = new Pdf(_existFilePath);
                Assert.Equal(5, _pdf.Count());
            }

            [Fact]
            public void Pdfファイルの読み込みに失敗する()
            {
                //Assert
                Assert.Throws<FileNotFoundException>(() => new Pdf(_notExistFilePath));
            }
        }
    }
}
