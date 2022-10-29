namespace Capitalist.EXMPL;

public class LoanOffer {
    public long id { get; set; }
    public double Value { get; set; }
    public int Year { get; set; }
    public double Percentage { get; set; }

    public string Information() {
        return $"Value: {Math.Round(Value,3)}\nPercentage: {Math.Round(Percentage,3)}\nYears: {Year}";
    }
}