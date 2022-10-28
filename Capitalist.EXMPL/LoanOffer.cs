namespace Capitalist.EXMPL;

public class LoanOffer {
    public long id { get; set; }
    public double Value { get; set; }
    public int Year { get; set; }
    public double Percentage { get; set; }

    public string Information() {
        return $"Value: {Value}\nPercentage: {Percentage}\nYears: {Year}";
    }
}