namespace LogicExercise
{
    class Program
    {
         static void Main(string[] args)
        {
            FooBarIteration();
        }

        static void FooBarIteration()
        {
            Console.WriteLine($"---Foobar Iteration---");

            int n = InputInteger();
            
            Console.WriteLine($"Foobar for {n} iteration : ");
            for (int i = 1; i <= n; i++)
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

            Console.WriteLine("Please enter a number (ex : 15) : ");

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
