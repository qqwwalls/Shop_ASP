using Xunit;
using MyCalculator;

namespace MyCalculator.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Sum_AddsTwoNumbers_ReturnsCorrectResult()
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
