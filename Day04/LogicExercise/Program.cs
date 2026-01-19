using Utils;

namespace LogicExercise
{
    class Program
    {
        static List<Rule> _rules = new List<Rule>
        {
            new Rule(3, "foo"),
            new Rule(5, "bar"),
            new Rule(7, "jazz"),
        };

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
            if (number == 0) return "null";

            string result = "";

            foreach (Rule rule in _rules)
            {
                if (rule.isDivideable(number))
                {
                    result += rule.Value;
                }
            }

            return result.Length > 0 ? result : number.ToString();
        }
    }
}
