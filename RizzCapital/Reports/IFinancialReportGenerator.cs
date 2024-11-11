namespace Reports
{
    public interface IFinancialReportGenerator
    {
        void AddExpense(string date, decimal amount, string category);
        void AddIncome(string date, decimal amount);
        void ListTransactions();
        void GenerateMonthlyReport(int year, int month);
    }
}