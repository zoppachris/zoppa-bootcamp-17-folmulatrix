using Utils;

namespace LogicExercise
{
    class Program
    {

        static void Main()
        {
            LogicIteration iteration = new();

            Console.WriteLine("Current iteration rules :");
            iteration.DisplayRule();
            Console.WriteLine();
            iteration.Iteration();

            Console.WriteLine("\n- Adding rule, divisor 3, return \"foo\" : Success");
            iteration.AddRule(3, "foo");
            Console.WriteLine("- Adding rule, divisor 4, return \"baz\" : Success");
            iteration.AddRule(4, "baz");
            Console.WriteLine("- Adding rule, divisor 5, return \"bar\" : Success");
            iteration.AddRule(5, "bar");
            Console.WriteLine("- Adding rule, divisor 7, return \"jazz\" : Success");
            iteration.AddRule(7, "jazz");
            Console.WriteLine("- Adding rule, divisor 9, return \"huzz\" : Success");
            iteration.AddRule(9, "huzz");
            Console.WriteLine("- Adding rule, divisor 12, return \"\" :");
            iteration.AddRule(12, "");
            Console.WriteLine("- Adding rule, divisor -5, return \"baal\" :");
            iteration.AddRule(-5, "baal");

            EnterToContinue();

            Console.WriteLine("\nUpdated iteration rules :");
            iteration.DisplayRule();
            Console.WriteLine("\nThere will be a combination if iterate number modulo is zero by multiple rules,\nex: 36 will be foobazhuzz (modulo 0 by each 3, 4 and 9)");

            Console.WriteLine("\nThis is an example for 15 iteration with updated rules.");
            EnterToContinue();

            iteration.Iteration();

            EnterToContinue();

            // Logic Iteration with input
            Console.WriteLine("\nYou can iterate with number input too.");
            int inputIteration = InputUtilities.ReadInteger(min: 1, max: 1260);
            iteration.Iteration(inputIteration);

            Console.Write("\nEnter any key to end..");
            Console.ReadKey();
        }

        static void EnterToContinue()
        {
            Console.WriteLine("\nEnter any key to continue..");
            Console.ReadKey();
        }
    }
}
