using Utils;

namespace LogicExercise
{
    class Program
    {

        static void Main()
        {
            LogicIteration iteration = new();

            iteration.Rules.Add(new Rule(4, "baz"));
            iteration.Rules.Add(new Rule(7, "jazz"));
            iteration.Rules.Add(new Rule(9, "huzz"));

            Console.WriteLine("Iteration Rules :");
            foreach (Rule rule in iteration.Rules.OrderBy(r => r.Divisor))
            {
                Console.WriteLine($"- If iterate number modulo by {rule.Divisor} is 0 then return {rule.Value}");
            }
            Console.WriteLine("\nThere will a combination if iterate number modulo is zero by multiple rules,\nex: 36 will be foobazhuzz (modulo 0 by each 3, 4 and 9)");

            Console.WriteLine("\nThis is an example for 15 iteration");
            Console.Write("Enter any key to continue..");
            Console.ReadKey();

            iteration.Iteration();
            
            Console.Write("\nEnter any key to continue..");
            Console.ReadKey();

            // Logic Iteration with input
            Console.WriteLine("\nYou can iterate with number input too.");
            int inputIteration = InputUtilities.ReadInteger(min: 1, max: 1260);
            iteration.Iteration(inputIteration);

            
            Console.Write("\nEnter any key to end..");
            Console.ReadKey();
        }
    }
}
