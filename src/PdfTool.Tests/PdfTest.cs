using System;
using Xunit;
using PdfTool;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;

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
                    throw new DirectoryNotFoundException($"'{_testDataDir}' not found.");

                _existFilePath = Path.Combine(_testDataDir, _existFileName);
                if (!File.Exists(_existFilePath))
                    throw new FileNotFoundException($"'{_existFilePath}' not found.");

                _brokenFilePath = Path.Combine(_testDataDir, _brokenFileName);
                if (!File.Exists(_brokenFilePath))
                    throw new FileNotFoundException($"'{_brokenFilePath}' not found.");

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

            [Theory]
            [InlineData(1, 90)]
            [InlineData(2, -90)]
            [InlineData(3, 180)]
            [InlineData(4, -180)]
            [InlineData(5, 270)]
            [InlineData(1, -270)]
            public void 任意のページを任意の方向に回転する(int pageNumber, int rotateDegree)
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);

                //Act
                _pdf.Rotate(pageNumber, rotateDegree);
                int rotate = _pdf.GetPageRotate(pageNumber);

                //Assert
                Assert.Equal(rotateDegree, rotate);
            }

            [Fact]
            public void 範囲外のページを指定した場合はpageNumberについてArgumentExceptionをThrowする()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 6;
                int degree = 90;

                //Assert
                Assert.Throws<ArgumentException>("pageNumber", () => _pdf.Rotate(pageNumber, degree));
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(-366)]
            [InlineData(45)]
            [InlineData(361)]
            [InlineData(360)]
            [InlineData(-360)]
            public void 回転角度degreeが0度90度180度270度及びその負数以外の場合はdegreeについてArgumentExceptionをThrowする(int rotateDegree)
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 3;

                //Assert
                Assert.Throws<ArgumentException>("rotateDegree", () => _pdf.Rotate(pageNumber, rotateDegree));
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

        public class IsAcceptableDegree_Test : PdfTestBase
        {
            [Theory]
            [InlineData(0)]
            [InlineData(90)]
            [InlineData(180)]
            [InlineData(270)]
            [InlineData(-90)]
            [InlineData(-180)]
            [InlineData(-270)]
            public void 引数にint型の上記値を与えたらTrueが返る(int degree)
            {
                //Assert
                Assert.True(Pdf.IsAcceptableDegree(degree));
            }

            [Theory]
            [InlineData("0")]
            [InlineData("90")]
            [InlineData("180")]
            [InlineData("270")]
            [InlineData("-90")]
            [InlineData("-180")]
            [InlineData("-270")]
            public void 引数にstring型の上記値を与えたらTrueが返る(string degree)
            {
                //Assert
                Assert.True(Pdf.IsAcceptableDegree(degree));
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(91)]
            [InlineData(222)]
            [InlineData(360)]
            [InlineData(-93)]
            [InlineData(-181)]
            [InlineData(-360)]
            public void 引数に0_90_180_270及びその負数以外を与えたらFalseが返る(int degree)
            {
                //Assert
                Assert.False(Pdf.IsAcceptableDegree(degree));
            }
        }
    }
}
