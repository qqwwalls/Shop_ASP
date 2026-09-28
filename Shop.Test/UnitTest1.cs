using Xunit;
using MyCalculator;

namespace Shop.Test
{
    public class CalculatorTest
    {
        [Fact]
        public void SumTest()
        {
            // Arrange
            var calculator = new Calculator();
            int a = 5;
            int b = 10;

            // Act
            int result = calculator.Sum(a, b);

            // Assert
            Assert.Equal(15, result);
        }
    }
}
