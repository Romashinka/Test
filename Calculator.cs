
namespace TestTask
{
    enum Operands
    {
        add,
        multiply,
        divide,
        subtract,
    }

    static class Calculator
    {
        public static int PerformOperation(int value1, Operands op, int value2)
        {
            switch (op)
            {
                case Operands.add:
                    return value1 + value2;
                case Operands.divide:
                    return value1 / value2;
                case Operands.multiply:
                    return value1 * value2;
                case Operands.subtract:
                    return value1 - value2;
                default:
                    return 0;
            }
        }
    }
}
