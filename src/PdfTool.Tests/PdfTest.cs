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
            public void �y�[�W��5��PDF�t�@�C����5��Ԃ�()
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
            public void PDF��ۑ�����()
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
            public void �y�[�W1�̌���angle��0()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                //Assert
                Assert.Equal(0, _pdf.GetPageAngle(1));
            }

            [Fact]
            public void �y�[�W1���E��90angle��]����()
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
            public void �y�[�W1������90angle��]����()
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
            public void �C�ӂ̃y�[�W��C�ӂ̕����ɉ�]����(int pageNumber, int rotateAngle)
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
            public void PDF�t�@�C���̃y�[�W�����傫�ȃy�[�W���w�肵���ꍇ��pageNumber�ɂ���ArgumentException��Throw����()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int pageNumber = 6;
                int angle = 90;

                //Assert
                Assert.Throws<ArgumentException>("pageNumber", () => _pdf.Rotate(pageNumber, angle));
            }

            [Fact]
            public void �y�[�W��1�������w�肵���ꍇ��pageNumber�ɂ���ArgumentException��Throw����()
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
            public void ��]�p�xangle��0�x90�x180�x270�x�y�т��̕����ȊO�̏ꍇ��angle�ɂ���ArgumentException��Throw����(int rotateAngle)
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
            public void Pdf�t�@�C���̓ǂݍ��݂ɐ�������()
            {
                //Assert
                _pdf = new Pdf(_existFilePath);
                Assert.Equal(5, _pdf.Count());
            }

            [Fact]
            public void Pdf�t�@�C���̓ǂݍ��݂Ɏ��s����()
            {
                //Assert
                Assert.Throws<FileNotFoundException>(() => new Pdf(_notExistFilePath));
            }

            [Fact]
            public void ��ꂽPdf�t�@�C���̓ǂݍ��݂Ɏ��s����()
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
            public void ������int�^�̏�L�l��^������True���Ԃ�(int angle)
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
            public void ������string�^�̏�L�l��^������True���Ԃ�(string angle)
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
            public void ������0_90_180_270�y�т��̕����ȊO��^������False���Ԃ�(int angle)
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
            public void PDF����3�y�[�W���I��������y�[�W����3()
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
            public void PDF����4�y�[�W���I��������y�[�W����4()
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
            public void �I�������y�[�W�̐���Ԃ�(params int[] pageNumbers)
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
            public void PDF�̃y�[�W�����傫���y�[�W���w�肵���ꍇ��ArgumentException�𓊂���()
            {
                //Arrange
                _pdf = new Pdf(_existFilePath);
                int[] targets = { 1, 3, 6 };

                //Act
                //Assert
                Assert.Throws<ArgumentException>("pageNumbers", () =>_pdf.Select(targets));
            }

            [Fact]
            public void Select���\�b�h�Ɉ������w�肵�Ȃ������ꍇ()
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
