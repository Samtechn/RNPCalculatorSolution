namespace RNPCalculatorWepAPI.Constantes
{
    public static class Operands
    {
        public static char[] OperandsList { get; } = new char[] { '+', '-', '*', '/' };

        public static bool IsValidOperator(char op)
        {
            return OperandsList.Contains(op);
        }
    }
}