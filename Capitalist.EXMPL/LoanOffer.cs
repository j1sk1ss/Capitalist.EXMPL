namespace Capitalist.EXMPL;

public class LoanOffer {
    public LoanOffer(double value, int year,double percentage, long id) {
        Value = value;
        Year = year;
        this.id = id;
        Payment = (Value + Value * Percentage) / Year;
        Percentage = percentage;
    }
    public long id { get; set; }
    public double Value { get; set; }
    
    public double Payment { get; set; }
    public int Year { get; set; }
    public double Percentage { get; set; }

    public string Information() {
        return $"Value: {Math.Round(Value,3)}\nPercentage: {Math.Round(Percentage,3)}\nYears: {Year}\nPay per day: {Math.Round(Payment,3)}";
    }
}