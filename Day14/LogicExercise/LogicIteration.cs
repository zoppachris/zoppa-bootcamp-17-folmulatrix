using System.Text;

namespace LogicExercise
{
    public class LogicIteration
    {
        private List<Rule> _rules = new();

        public void AddRule(int input, string output)
        {
            int positiveInput = Math.Abs(input);

            if (_rules.Any(rule => rule.Divisor == positiveInput) || string.IsNullOrEmpty(output))
            {
                // Menghindari Exception
                Console.WriteLine(string.IsNullOrEmpty(output) ? "Return word cannot be empty" : "Divisor already added in rules\nnegative number with the same value with\npositive number is the same (ex : 3 and -3)");
                Console.WriteLine($"Failed to add rule by divisor {input}.");
                return;
            }

            _rules.Add(new Rule(input, output));
        }
        public void DisplayRule()
        {
            if (IsRuleEmpty())
            {
                Console.WriteLine("There is no rule.");
            }
            else
            {
                foreach (Rule rule in _rules.OrderBy(r => r.Divisor))
                {
                    Console.WriteLine($"- If iterate number modulo by {rule.Divisor} is 0 then return {rule.Value}");
                }
            }
        }
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

            if (!IsRuleEmpty())
            {
                foreach (Rule rule in _rules.OrderBy(r => r.Divisor))
                {
                    if (rule.isDivideable(number))
                    {
                        result.Append(rule.Value);
                    }
                }
            }

            return result.Length > 0 ? result.ToString() : number.ToString();
        }
        private bool IsRuleEmpty()
        {
            return _rules is null or [];
        }
    }
}