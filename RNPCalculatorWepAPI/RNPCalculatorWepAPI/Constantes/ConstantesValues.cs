namespace RNPCalculatorWepAPI.Constantes
{
    public static class FilePathValues
    {
        public static readonly string jsonPath = @"BackUpFile/Stacks.json";
    }

    public static class OperandValues
    {
        public static char[] OperandsList { get; } = new char[] { '+', '-', '*', '/' };

        public static bool IsValidOperator(char op)
        {
            return OperandsList.Contains(op);
        }
    }
}
