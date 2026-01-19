using Utils;

namespace LogicExercise
{
    class Program
    {
        static void Main(string[] args)
        {
            FooBarIteration();

            // Foobar Iteration with input
            Console.WriteLine("\nYou can iterate Foobar with number input.");
            int inputIteration = InputUtilities.ReadInteger(min: 1, max: 100);
            FooBarIteration(inputIteration);
        }

        static void FooBarIteration(int countIteration = 15)
        {
            Console.WriteLine($"---Foobar Iteration---");

            Console.WriteLine($"Foobar for {countIteration} iteration : ");
            for (int i = 1; i <= countIteration; i++)
            {
                Console.WriteLine(ReturnFooBar(i));
            }
        }

        static string ReturnFooBar(int number = 0)
        {
            if (number == 0)
            {
                return "null";
            }

            string result = number.ToString();

            if (number % 3 == 0)
            {
                result = "foo";
            }
            if (number % 5 == 0)
            {
                result = "bar";
            }
            if ((number % 3 == 0) && (number % 5 == 0))
            {
                result = "foobar";
            }

            return result;
        }
    }
}
