namespace Transactions
{
    public class Expense : Transaction
    {
        public Expense(DateTime date, decimal amount, string category)
            : base(date, amount, category) { }
    }
}