namespace Transactions
{
    public class Income : Transaction
    {
        public Income(DateTime date, decimal amount)
            : base(date, amount, "Income") { }
    }
}