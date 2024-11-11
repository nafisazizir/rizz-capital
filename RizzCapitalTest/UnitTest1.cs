namespace Reports.Tests
{
    public class FinancialReportGeneratorTests
    {
        private readonly FinancialReportGenerator _reportGenerator;

        public FinancialReportGeneratorTests()
        {
            _reportGenerator = new FinancialReportGenerator();
        }

        [Fact]
        public void AddExpense_ValidInput_ShouldAddExpenseSuccessfully()
        {
            // Arrange
            string date = "2024-01-15";
            decimal amount = 100.50m;
            string category = "Food";

            // Act
            _reportGenerator.AddExpense(date, amount, category);

            // Assert
            var output = CaptureConsoleOutput(() => _reportGenerator.ListTransactions());
            Assert.Contains("2024-01-15: $100.50, Category=\"Food\"", output);
        }

        [Theory]
        [InlineData("", 100, "Food", "Date must be in YYYY-MM-DD format")]
        [InlineData("2024-01-15", 0, "Food", "Amount must be greater than 0")]
        [InlineData("2024-01-15", -100, "Food", "Amount must be greater than 0")]
        [InlineData("2024-01-15", 100, "", "Category cannot be empty")]
        [InlineData("2024-13-15", 100, "Food", "Invalid date format. Use YYYY-MM-DD")]
        [InlineData("2024-01-15", 100, "xxxxxxxxxxxxxxxxxxxxx", "Category cannot exceed 20 characters")]
        public void AddExpense_InvalidInput_ShouldThrowArgumentException(
            string date, decimal amount, string category, string expectedError)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => _reportGenerator.AddExpense(date, amount, category));
            Assert.Equal(expectedError, exception.Message);
        }

        [Fact]
        public void AddIncome_ValidInput_ShouldAddIncomeSuccessfully()
        {
            // Arrange
            string date = "2024-01-15";
            decimal amount = 5000m;

            // Act
            _reportGenerator.AddIncome(date, amount);

            // Assert
            var output = CaptureConsoleOutput(() => _reportGenerator.ListTransactions());
            Assert.Contains("2024-01-15: $5000", output);
        }

        [Theory]
        [InlineData("", 100, "Date must be in YYYY-MM-DD format")]
        [InlineData("2024-01-15", 0, "Amount must be greater than 0")]
        [InlineData("2024-01-15", -100, "Amount must be greater than 0")]
        [InlineData("2024-13-15", 100, "Invalid date format. Use YYYY-MM-DD")]
        public void AddIncome_InvalidInput_ShouldThrowArgumentException(
            string date, decimal amount, string expectedError)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(
                () => _reportGenerator.AddIncome(date, amount));
            Assert.Equal(expectedError, exception.Message);
        }

        [Fact]
        public void GenerateMonthlyReport_WithTransactions_ShouldGenerateCorrectReport()
        {
            // Arrange
            _reportGenerator.AddIncome("2024-01-15", 5000m);
            _reportGenerator.AddExpense("2024-01-16", 1000m, "Food");
            _reportGenerator.AddExpense("2024-01-17", 2000m, "Rent");

            // Act
            var report = CaptureConsoleOutput(() => _reportGenerator.GenerateMonthlyReport(2024, 1));

            // Assert
            Assert.Contains("Monthly Report for 2024-01:", report);
            Assert.Contains("Total Income: $5000", report);
            Assert.Contains("Total Expenses: $3000", report);
            Assert.Contains("* Food: $1000", report);
            Assert.Contains("* Rent: $2000", report);
        }

        [Fact]
        public void GenerateMonthlyReport_NoTransactions_ShouldIndicateNoTransactions()
        {
            // Act
            var report = CaptureConsoleOutput(() => _reportGenerator.GenerateMonthlyReport(2024, 2));

            // Assert
            Assert.Contains("No transactions found for 2024-02", report);
        }

        [Fact]
        public void ListTransactions_MultipleMonths_ShouldDisplayChronologically()
        {
            // Arrange
            _reportGenerator.AddIncome("2024-01-15", 5000m);
            _reportGenerator.AddExpense("2024-02-16", 1000m, "Food");
            _reportGenerator.AddIncome("2024-02-17", 6000m);

            // Act
            var output = CaptureConsoleOutput(() => _reportGenerator.ListTransactions());

            // Assert
            Assert.Contains("2024-01:", output);
            Assert.Contains("2024-02:", output);
            Assert.Contains("2024-01-15: $5000", output);
            Assert.Contains("2024-02-16: $1000", output);
            Assert.Contains("2024-02-17: $6000", output);

            // Verify chronological order
            int pos1 = output.IndexOf("2024-01");
            int pos2 = output.IndexOf("2024-02");
            Assert.True(pos1 < pos2);
        }

        private string CaptureConsoleOutput(Action action)
        {
            // using var writer = new StringWriter();
            // Console.SetOut(writer);
            // action();
            // return writer.ToString();

            var originalOutput = Console.Out;
            var output = new System.IO.StringWriter();

            try
            {
                Console.SetOut(output);
                action();
            }
            finally
            {
                Console.SetOut(originalOutput);
                output.Dispose();
            }

            return output.ToString();
        }
    }
}