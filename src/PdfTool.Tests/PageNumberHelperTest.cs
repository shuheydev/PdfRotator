using PdfTool.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PdfTool.Tests
{
    namespace PageNumberHelperTest
    {
        public class ToInt_Test
        {
            [Fact]
            public void 文字列の数字を配列で渡すとintの配列にして返す()
            {
                //Arrange
                string[] numbers = { "1", "2" };
                
                //Act
                var actual = PageNumberHelper.ToInt(numbers);
                var expected = new int[] { 1, 2 };
                //Assert
                //Assert.IsType<int[]>(actual);
                Assert.True(expected.SequenceEqual(actual));
            }

            [Fact]
            public void 範囲指定をするとintの配列にして返す()
            {
                //Arrange
                string[] numbers = { "4-8", "10-12" };

                //Act
                var actual = PageNumberHelper.ToInt(numbers);
                var expected = new int[] { 4, 5, 6, 7, 8, 10, 11, 12 };

                //Assert
                //Assert.IsType<int[]>(actual);
                Assert.True(expected.SequenceEqual(actual));
            }
        }
    }
}
