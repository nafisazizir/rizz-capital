using Reports;

namespace RizzCapital
{
    public class Program
    {
        public static void Main()
        {
            var generator = new FinancialReportGenerator();
            var processor = new CommandProcessor(generator);

            Console.WriteLine("Financial Report Generator");
            Console.WriteLine("Enter commands (type 'exit' to quit):\n");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) continue;
                if (input.ToLowerInvariant() == "exit") break;

                processor.ProcessCommand(input);
            }
        }
    }
}