namespace LogicExercise
{
    public struct Rule
    {
        public int Divisor { get; }
        public string Value { get; }

        public Rule(int divisor, string value)
        {
            Divisor = divisor;
            Value = value;
        }

        public bool isDivideable(ref int number)
        {
            return number % Divisor == 0;
        }
    }
}