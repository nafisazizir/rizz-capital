using System.Text.RegularExpressions;
using Transactions;

namespace Reports
{
    public class FinancialReportGenerator : IFinancialReportGenerator
    {
        private readonly Dictionary<YearMonth, List<Transaction>> _transactions;
        private const int MaxCategoryLength = 20;

        public FinancialReportGenerator()
        {
            _transactions = new Dictionary<YearMonth, List<Transaction>>();
        }

        public void AddExpense(string dateStr, decimal amount, string category)
        {
            ValidateInputs(dateStr, amount, category);

            var date = ParseDate(dateStr);
            var expense = new Expense(date, amount, category);
            AddTransaction(expense);

            Console.WriteLine($"Expense added: Date={dateStr}, Amount={amount}, Category=\"{category}\"");
        }

        public void AddIncome(string dateStr, decimal amount)
        {
            ValidateInputs(dateStr, amount);

            var date = ParseDate(dateStr);
            var income = new Income(date, amount);
            AddTransaction(income);

            Console.WriteLine($"Income added: Date={dateStr}, Amount={amount}");
        }

        public void ListTransactions()
        {
            Console.WriteLine("Transactions:\n");

            foreach (var monthGroup in _transactions.OrderBy(x => x.Key.Year).ThenBy(x => x.Key.Month))
            {
                Console.WriteLine($"{monthGroup.Key}:");

                // Group and display income
                var incomeTransactions = monthGroup.Value.OfType<Income>();
                if (incomeTransactions.Any())
                {
                    Console.WriteLine("  Income:");
                    foreach (var income in incomeTransactions.OrderBy(t => t.Date))
                    {
                        Console.WriteLine($"    - {income.Date:yyyy-MM-dd}: ${income.Amount}");
                    }
                }

                // Group and display expenses
                var expenseTransactions = monthGroup.Value.OfType<Expense>();
                if (expenseTransactions.Any())
                {
                    Console.WriteLine("  Expenses:");
                    foreach (var expense in expenseTransactions.OrderBy(t => t.Date))
                    {
                        Console.WriteLine($"    - {expense.Date:yyyy-MM-dd}: ${expense.Amount}, Category=\"{expense.Category}\"");
                    }
                }

                Console.WriteLine();
            }
        }

        public void GenerateMonthlyReport(int year, int month)
        {
            var yearMonth = new YearMonth(year, month);
            if (!_transactions.ContainsKey(yearMonth))
            {
                Console.WriteLine($"No transactions found for {yearMonth}");
                return;
            }

            var monthlyTransactions = _transactions[yearMonth];
            var totalIncome = monthlyTransactions.OfType<Income>().Sum(t => t.Amount);
            var expenses = monthlyTransactions.OfType<Expense>().ToList();
            var totalExpenses = expenses.Sum(t => t.Amount);

            Console.WriteLine($"Monthly Report for {yearMonth}:");
            Console.WriteLine($"- Total Income: ${totalIncome}");
            Console.WriteLine($"- Total Expenses: ${totalExpenses}");
            Console.WriteLine("- By Category:");

            // Add income category
            if (totalIncome > 0)
            {
                Console.WriteLine($"  * Income: ${totalIncome}");
            }

            // Group expenses by category
            var categoryGroups = expenses
                .GroupBy(e => e.Category)
                .OrderBy(g => g.Key);

            foreach (var group in categoryGroups)
            {
                Console.WriteLine($"  * {group.Key}: ${group.Sum(e => e.Amount)}");
            }
        }

        private void AddTransaction(Transaction transaction)
        {
            var yearMonth = new YearMonth(transaction.Date.Year, transaction.Date.Month);

            if (!_transactions.ContainsKey(yearMonth))
            {
                _transactions[yearMonth] = new List<Transaction>();
            }

            _transactions[yearMonth].Add(transaction);
        }

        private static DateTime ParseDate(string dateStr)
        {
            if (!DateTime.TryParse(dateStr, out DateTime date))
                throw new ArgumentException("Invalid date format. Use YYYY-MM-DD");
            return date;
        }

        private static void ValidateInputs(string dateStr, decimal amount, string? category = null)
        {
            if (!Regex.IsMatch(dateStr, @"^\d{4}-\d{2}-\d{2}$"))
                throw new ArgumentException("Date must be in YYYY-MM-DD format");

            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than 0");

            if (category != null)
            {
                if (string.IsNullOrWhiteSpace(category))
                    throw new ArgumentException("Category cannot be empty");

                if (category.Length > MaxCategoryLength)
                    throw new ArgumentException($"Category cannot exceed {MaxCategoryLength} characters");
            }
        }
    }
}