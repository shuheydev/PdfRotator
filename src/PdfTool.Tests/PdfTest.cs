using System;
using System.IO;
using Xunit;

namespace PdfTool.Tests
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
            protected string _brokenFileName = "PdfRotateTest_Broken.pdf";
            protected string _brokenFilePath;


            public void Init(string id)
            {
                _testDataDir = "../../../TestData";
                if (!Directory.Exists(_testDataDir))
                {
                    throw new DirectoryNotFoundException($"'{_testDataDir}' not found.");
                }

                _existFilePath = Path.Combine(_testDataDir, _existFileName);
                if (!File.Exists(_existFilePath))
                {
                    throw new FileNotFoundException($"'{_existFilePath}' not found.");
                }

                _brokenFilePath = Path.Combine(_testDataDir, _brokenFileName);
                if (!File.Exists(_brokenFilePath))
                {
                    throw new FileNotFoundException($"'{_brokenFilePath}' not found.");
                }

                _outputFileName = $"{id}_output.pdf";
                _outputFilePath = Path.Combine(_testDataDir, _outputFileName);
            }

            public void Dispose()
            {
                if (File.Exists(_outputFilePath))
                {
                    File.Delete(_outputFilePath);
                }
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
                int angle = -90;
                _pdf.Rotate(pageNumber, angle);

                //Act
                _pdf.Write(_outputFilePath);

                //Assert
                Assert.True(File.Exists(_outputFilePath));
                var pdf = new Pdf(_outputFilePath);
                Assert.Equal(angle, pdf.GetPageAngle(pageNumber));
            }
        }

        public class Rotate_Tests : PdfTestBase
        {
            public Rotate_Tests()
            {
                Init(nameof(Rotate_Tests));
            }

            [Fact]
            public void ページ1の向きangleは0()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                //Assert
                Assert.Equal(0, _pdf.GetPageAngle(1));
            }

            [Fact]
            public void ページ1を右に90angle回転する()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 1;
                int angle = 90;

                //Act
                _pdf.Rotate(pageNumber, angle);
                int rotate = _pdf.GetPageAngle(pageNumber);

                //Assert
                Assert.Equal(angle, rotate);
            }

            [Fact]
            public void ページ1を左に90angle回転する()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 1;
                int angle = -90;

                //Act
                _pdf.Rotate(pageNumber, angle);
                int rotate = _pdf.GetPageAngle(pageNumber);

                //Assert
                Assert.Equal(angle, rotate);
            }

            [Theory]
            [InlineData(1, 90)]
            [InlineData(2, -90)]
            [InlineData(3, 180)]
            [InlineData(4, -180)]
            [InlineData(5, 270)]
            [InlineData(1, -270)]
            public void 任意のページを任意の方向に回転する(int pageNumber, int rotateAngle)
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);

                //Act
                _pdf.Rotate(pageNumber, rotateAngle);
                int rotate = _pdf.GetPageAngle(pageNumber);

                //Assert
                Assert.Equal(rotateAngle, rotate);
            }

            [Fact]
            public void PDFファイルのページ数より大きなページを指定した場合はpageNumberについてArgumentExceptionをThrowする()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 6;
                int angle = 90;

                //Assert
                Assert.Throws<ArgumentException>("pageNumber", () => _pdf.Rotate(pageNumber, angle));
            }

            [Fact]
            public void ページに1未満を指定した場合はpageNumberについてArgumentExceptionをThrowする()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 0;
                int angle = 90;

                //Assert
                Assert.Throws<ArgumentException>("pageNumber", () => _pdf.Rotate(pageNumber, angle));
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(-366)]
            [InlineData(45)]
            [InlineData(361)]
            [InlineData(360)]
            [InlineData(-360)]
            public void 回転角度angleが0度90度180度270度及びその負数以外の場合はangleについてArgumentExceptionをThrowする(int rotateAngle)
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 3;

                //Assert
                Assert.Throws<ArgumentException>("rotateAngle", () => _pdf.Rotate(pageNumber, rotateAngle));
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

            [Fact]
            public void 壊れたPdfファイルの読み込みに失敗する()
            {
                //Assert
                Assert.Throws<iTextSharp.text.exceptions.InvalidPdfException>(() => new Pdf(_brokenFilePath));
            }
        }

        public class IsAcceptableAngle_Test : PdfTestBase
        {
            [Theory]
            [InlineData(0)]
            [InlineData(90)]
            [InlineData(180)]
            [InlineData(270)]
            [InlineData(-90)]
            [InlineData(-180)]
            [InlineData(-270)]
            public void 引数にint型の上記値を与えたらTrueが返る(int angle)
            {
                //Assert
                Assert.True(Pdf.IsAcceptableAngle(angle));
            }

            [Theory]
            [InlineData("0")]
            [InlineData("90")]
            [InlineData("180")]
            [InlineData("270")]
            [InlineData("-90")]
            [InlineData("-180")]
            [InlineData("-270")]
            public void 引数にstring型の上記値を与えたらTrueが返る(string angle)
            {
                //Assert
                Assert.True(Pdf.IsAcceptableAngle(angle));
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(91)]
            [InlineData(222)]
            [InlineData(360)]
            [InlineData(-93)]
            [InlineData(-181)]
            [InlineData(-360)]
            public void 引数に0_90_180_270及びその負数以外を与えたらFalseが返る(int angle)
            {
                //Assert
                Assert.False(Pdf.IsAcceptableAngle(angle));
            }
        }

        public class Select_Test : PdfTestBase
        {
            public Select_Test()
            {
                Init(nameof(Select_Test));
            }

            [Fact]
            public void PDFから3ページ分選択したらページ数は3()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);

                //Act
                _pdf.Select(1, 3, 5);
                var actual = _pdf.Count();

                //Assert
                Assert.Equal(3, actual);
            }

            [Fact]
            public void PDFから4ページ分選択したらページ数は4()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);

                //Act
                _pdf.Select(1, 3, 5);
                var actual = _pdf.Count();

                //Assert
                Assert.Equal(3, actual);
            }

            [Theory]
            [InlineData(1)]
            [InlineData(1, 2)]
            [InlineData(1, 2, 3)]
            [InlineData(1, 2, 3, 4)]
            [InlineData(1, 2, 3, 4, 5)]
            public void 選択したページの数を返す(params int[] pageNumbers)
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);

                //Act
                _pdf.Select(pageNumbers);
                int actual = _pdf.Count();
                int expected = pageNumbers.Length;

                //Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void PDFのページよりも大きいページを指定した場合にArgumentExceptionを投げる()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int[] targets = { 1, 3, 6 };

                //Act
                //Assert
                Assert.Throws<ArgumentException>("pageNumbers", () =>_pdf.Select(targets));
            }

            [Fact]
            public void Selectメソッドに引数を指定しなかった場合()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);

                //Act
                //Assert
                Assert.Throws<ArgumentException>("pageNumbers", () => _pdf.Select());
            }
        }
    }
}
