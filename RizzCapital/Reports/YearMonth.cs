namespace Reports
{
    public readonly struct YearMonth
    {
        public int Year { get; }
        public int Month { get; }

        public YearMonth(int year, int month)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Month must be between 1 and 12");

            Year = year;
            Month = month;
        }

        public override string ToString() => $"{Year}-{Month:D2}";
    }
}