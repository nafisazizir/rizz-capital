using Reports;

namespace RizzCapital
{
    public class CommandProcessor
    {
        private readonly IFinancialReportGenerator _generator;

        public CommandProcessor(IFinancialReportGenerator generator)
        {
            _generator = generator;
        }

        public void ProcessCommand(string input)
        {
            try
            {
                var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) return;

                var command = parts[0].ToUpperInvariant();
                switch (command)
                {
                    case "ADDEXPENSE":
                        if (parts.Length != 4)
                            throw new ArgumentException("AddExpense requires date, amount, and category");
                        _generator.AddExpense(parts[1], decimal.Parse(parts[2]), parts[3].Trim('"'));
                        break;

                    case "ADDINCOME":
                        if (parts.Length != 3)
                            throw new ArgumentException("AddIncome requires date and amount");
                        _generator.AddIncome(parts[1], decimal.Parse(parts[2]));
                        break;

                    case "LISTTRANSACTIONS":
                        _generator.ListTransactions();
                        break;

                    case "GENERATEMONTHLYREPORT":
                        if (parts.Length != 3)
                            throw new ArgumentException("GenerateMonthlyReport requires year and month");
                        _generator.GenerateMonthlyReport(int.Parse(parts[1]), int.Parse(parts[2]));
                        break;

                    default:
                        throw new ArgumentException($"Unknown command: {command}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}