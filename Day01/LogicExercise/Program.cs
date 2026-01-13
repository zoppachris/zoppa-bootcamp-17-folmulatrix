namespace LogicExercise
{
    class Program
    {
         static void Main(string[] args)
        {
            FooBarIteration();

            // Foobar Iteration with input
            Console.WriteLine("\nYou can iterate Foobar with number input.");
            int inputIteration = InputInteger();
            FooBarIteration(inputIteration);
        }

        static void FooBarIteration(int countIteration = 15)
        {
            Console.WriteLine($"---Foobar Iteration---");
            
            Console.WriteLine($"Foobar for {countIteration} iteration : ");
            for (int i = 1; i <= countIteration; i++)
            {
                switch (i)
                {
                    case int x when (x % 3 == 0) && (x % 5 == 0) :
                        Console.WriteLine("foobar");
                        break;

                    case int x when x % 3 == 0 :
                        Console.WriteLine("foo");
                        break;

                    case int x when x % 5 == 0 :
                        Console.WriteLine("bar");
                        break;

                    default:
                        Console.WriteLine(i);
                        break;
                }
            }
        }
    
        static int InputInteger()
        {
            int number = 1;
            bool isValidInput = false;

            Console.WriteLine("Please enter a number : ");

            while(!isValidInput)
            {
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out number))
                    {
                        isValidInput = true;
                    } 
                else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number : ");
                    }
            }

            return number;
        }
    }
}
