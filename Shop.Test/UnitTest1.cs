using System;
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

        [Fact]
        public void SubtractTest()
        {
            //A - Arrange
            int a = 20, b = 5;
            Calculator calculator = new Calculator();
            
            //A - Act
            int result = calculator.Subtract(a, b);

            //A - Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public void MultiplyTest()
        {
            //A - Arrange
            int a = 5, b = 4;
            Calculator calculator = new Calculator();
            
            //A - Act
            int result = calculator.Multiply(a, b);

            //A - Assert
            Assert.Equal(20, result);
        }

        [Fact]
        public void DivideTest()
        {
            //A - Arrange
            int a = 10, b = 2;
            Calculator calculator = new Calculator();
            
            //A - Act
            double result = calculator.Divide(a, b);

            //A - Assert
            Assert.Equal(5.0, result);
        }

        [Fact]
        public void DivideByZero_ThrowsExceptionTest()
        {
            //A - Arrange
            int a = 10, b = 0;
            Calculator calculator = new Calculator();
            
            //A - Act & Assert
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(a, b));
        }
    }
}
