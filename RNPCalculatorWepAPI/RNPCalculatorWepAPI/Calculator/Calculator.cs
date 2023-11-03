using System.Reflection.Metadata.Ecma335;

namespace RNPCalculatorWepAPI.Calculator
{
    internal class CalculatorClass : ICalculator
    {
        public double Calculate(char op, double d1, double d2) 
        {
            return op switch
            {
                '+' => ApplyAddition(d1, d2),
                '-' => ApplySoustraction(d1, d2),
                '*' => ApplyMultiplication(d1, d2),
                '/' => ApplyDivision(d1, d2),
            };
        }

        private static double ApplyAddition(double d1, double d2)
        {
            return d1 + d2;
        }

        private static double ApplyMultiplication(double d1, double d2)
        {
            return d1 * d2;
        }

        private static double ApplyDivision(double d1, double d2)
        {
            return d2 / d1;
        }

        private static double ApplySoustraction(double d1, double d2)
        {
            return d2 - d1;
        }

    }
}
