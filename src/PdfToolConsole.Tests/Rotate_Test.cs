using PdfTool;
using PdfToolConsole.Commands;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace PdfToolConsole.Tests
{
    namespace PdfToolConsoleTest
    {
        public class Rotate_Test : PdfToolConsoleTestBase
        {
            [Fact]
            public void Page1���E��90�x��]���������Ƃ̊p�x��90�x()
            {
                //Arrange
                var rotate = new Rotate();
                _existFilePath = Path.Combine(_testDataDir, _existFileName);
                int pageNumber = 1;
                int angle = 90;

                //Act
                _outputFilePath = Path.Combine(_testDataDir, _outputFileName);
                rotate.Pages(_existFilePath, new[] { $"{pageNumber}:{angle}" }, _outputFilePath);
                
                using var pdf = new Pdf(_outputFilePath);
                int actual = pdf.GetPageAngle(pageNumber);
                int expected = angle;

                //Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void Page2���E��180�x��]���������Ƃ̊p�x��180�x()
            {
                //Arrange
                var rotate = new Rotate();
                _existFilePath = Path.Combine(_testDataDir, _existFileName);
                int pageNumber = 2;
                int angle = 180;

                //Act
                _outputFilePath = Path.Combine(_testDataDir, _outputFileName);
                rotate.Pages(_existFilePath, new[] { $"{pageNumber}:{angle}" }, _outputFilePath);
                
                using var pdf = new Pdf(_outputFilePath);
                int actual = pdf.GetPageAngle(pageNumber);
                int expected = angle;

                //Assert
                Assert.Equal(expected, actual);
            }

            [Theory]
            [InlineData(1, 90)]
            [InlineData(2, -90)]
            [InlineData(3, 180)]
            [InlineData(4, -180)]
            [InlineData(5, 270)]
            [InlineData(1, -270)]
            public void �w�肵���y�[�W��x�x��]���������Ƃ̊p�x��x�x(int pageNumber, int angle)
            {
                //Arrange
                var rotate = new Rotate();
                _existFilePath = Path.Combine(_testDataDir, _existFileName);

                //Act
                _outputFilePath = Path.Combine(_testDataDir, _outputFileName);
                rotate.Pages(_existFilePath, new[] { $"{pageNumber}:{angle}" }, _outputFilePath);

                using var pdf = new Pdf(_outputFilePath);
                int actual = pdf.GetPageAngle(pageNumber);
                int expected = angle;

                //Assert
                Assert.Equal(expected, actual);
            }
        }
    }
}
