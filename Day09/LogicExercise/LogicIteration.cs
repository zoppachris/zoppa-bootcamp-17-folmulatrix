using System.Text;

namespace LogicExercise
{
    public class LogicIteration
    {
        public List<Rule> Rules = new List<Rule>
        {
            new Rule(3, "foo"),
            new Rule(5, "bar")
        };

        public void Iteration(int countIteration = 15)
        {
            Console.WriteLine($"---Logic Iteration---");

            Console.WriteLine($"Return value for {countIteration} iteration : ");
            for (int i = 1; i <= countIteration; i++)
            {
                Console.WriteLine(ReturnIterate(i));
            }
        }

        private string ReturnIterate(int number = 0)
        {
            if (number == 0) return "null";

            StringBuilder result = new StringBuilder();

            foreach (Rule rule in Rules.OrderBy(r => r.Divisor))
            {
                if (rule.isDivideable(number))
                {
                    result.Append(rule.Value);
                }
            }

            return result.Length > 0 ? result.ToString() : number.ToString();
        }
    }
}