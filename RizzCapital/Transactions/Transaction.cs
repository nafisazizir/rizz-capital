namespace Transactions
{
    public abstract class Transaction
    {
        public DateTime Date { get; }
        public decimal Amount { get; }
        public string Category { get; }

        protected Transaction(DateTime date, decimal amount, string category)
        {
            Date = date;
            Amount = amount;
            Category = category;
        }
    }
}