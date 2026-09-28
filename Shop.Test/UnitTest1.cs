using Xunit;
using MyCalculator;

namespace Shop.Test
{
    public class CalculatorTest
    {
        [Fact]
        public void SumTest()
        {
            //A - Arrange
            int a = 10, b = 20;
            Calculator calculator = new Calculator();
            
            //A - Act
            int result = calculator.Sum(a, b);

            //A - Assert
            Assert.Equal(30, result);
        }
    }
}
