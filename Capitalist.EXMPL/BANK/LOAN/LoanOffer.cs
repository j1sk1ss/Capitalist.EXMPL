using Capitalist.EXMPL.OBJECTS;

namespace Capitalist.EXMPL.BANK.LOAN;

public class LoanOffer {
    public LoanOffer(double value, int year, double percentage, long id, ICapitalist owner) {
        Value      = value;
        Owner      = owner;
        Year       = year;
        Id         = id;
        Payment    = (Value + Value * Percentage) / Year;
        Percentage = percentage;
    }

    public ICapitalist Owner { get; }
    public long Id { get; }
    public double Value { get; }
    public double Payment { get; }
    public int Year { get; set; }
    
    private double Percentage { get; }

    public string Information() =>
         $"Value: {Math.Round(Value,3)}\nPercentage: {Math.Round(Percentage,3)}\nYears: {Year}\nPay per day: {Math.Round(Payment,3)}";
}