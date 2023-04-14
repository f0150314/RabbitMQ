namespace Common.Others;

public static class Algorithm
{
    public static int Fib(int number)
    {
        if (number is 0 or 1)
        {
            return number;
        }

        return Fib(number - 1) + Fib(number - 2);
    }
}