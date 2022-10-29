namespace Capitalist.EXMPL;

public class Player : ICapitalist
{
    public Player() {
        Balance = 100;
        Inventory = new Dictionary<string, int>() {
            {"Wood", 0},
            {"Potato", 0},
            {"Carrot", 0},
            {"Meet", 0},
            {"Gold", 0},
        };
        LoanOffers = new List<LoanOffer>();
        MyLoans = new List<long>();
        GettedLoans = new List<long>();
    }
    public double Balance { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public List<LoanOffer> LoanOffers { get; set; }
    public List<long> MyLoans { get; set; }
    public List<long> GettedLoans { get; set; }
    public LoanOffer CreateLoanOffers(int year, float percentage, double val) {
        return new LoanOffer(val, year, percentage, new Random().Next());
    }
    public void Buy(string product, Dictionary<string, float> cost) {
        if (Balance >= cost[product]) Balance -= cost[product];
        Inventory[product] += 10;
    }
    public void Sell(string product, Dictionary<string, float> cost) {
        if (Inventory[product] >= 10) Balance += cost[product];
        Inventory[product] -= 10;
    }

    public void TakeLoan(LoanOffer loanOffer)
    {
        MyLoans.Add(loanOffer.id);
        Balance += loanOffer.Value;
    } 
}