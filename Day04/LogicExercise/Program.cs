using Utils;

namespace LogicExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            LogicIteration();

            // Logic Iteration with input
            Console.WriteLine("\nYou can iterate with number input.");
            int inputIteration = InputUtilities.ReadInteger(min: 1, max: 315);
            LogicIteration(inputIteration);
        }

        static void LogicIteration(int countIteration = 15)
        {
            Console.WriteLine($"---Logic Iteration---");

            Console.WriteLine($"Return value for {countIteration} iteration : ");
            for (int i = 1; i <= countIteration; i++)
            {
                Console.WriteLine(ReturnIterate(i));
            }
        }

        static string ReturnIterate(int number = 0)
        {
            if (number == 0)
            {
                return "null";
            }

            string result = "";

            if (number % 3 == 0)
            {
                result += "foo";
            }
            if (number % 5 == 0)
            {
                result += "bar";
            }
            if (number % 7 == 0)
            {
                result += "jazz";
            }
            if (string.IsNullOrEmpty(result))
            {
                result = number.ToString();
            }

            return result;
        }
    }
}
