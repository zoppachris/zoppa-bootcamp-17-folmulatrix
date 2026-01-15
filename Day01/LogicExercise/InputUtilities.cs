namespace LogicExercise
{
    public static class InputUtilities
    {
        private static int _usedCount = 0;
        public static int UsedCount => _usedCount;
        public static bool IsInitialized { get; private set; } = true;

        public static int ReadInteger(string prompt = "Please enter a number : ", int? min = null, int? max = null)
        {
            int value;

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out value))
                {
                    bool inRange = (min.HasValue ? value >= min.Value : true) && (max.HasValue ? value <= max.Value : true);

                    if (inRange)
                    {
                        return value;
                    }
                    else
                    {
                        if (min.HasValue && max.HasValue)
                        {
                            Console.WriteLine($"Invalid input. Please enter a number between {min.Value}-{max.Value}.");
                        }
                        else if (min.HasValue)
                        {
                            Console.WriteLine($"Invalid input. Please enter a number between greater than or equal to {min.Value}.");
                        }
                        else if (max.HasValue)
                        {

                            Console.WriteLine($"Invalid input. Please enter a number between less  thanor equal to {max.Value}.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid integer input.");
                }
            }
        }
    }
}