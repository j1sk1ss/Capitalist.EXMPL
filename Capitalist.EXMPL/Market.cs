namespace Capitalist.EXMPL;

public class Market
{
    public double Balance { get; set; }
    public double Inflation { get; set; }
    public Market() {
        Balance = 10000;
        Storage = new Dictionary<string, int>() {
            {"Wood", 2125},
            {"Potato", 3875},
            {"Carrot", 980},
            {"Meet", 459},
            {"Gold", 73},
        };
        Inflation = 0;
        Cost = new Dictionary<string, double>();
        LoanOffers = new List<LoanOffer>();
        MyLoans = new List<long>();
        GettedLoans = new List<long>();
        GenerateCost();
    }
    public void Buy(string product, Dictionary<string, float> cost) {
        if (Balance >= cost[product]) Balance -= cost[product];
        Storage[product] += 10;
    }
    public void Sell(string product, Dictionary<string, float> cost) {
        if (Storage[product] >= 10) Balance += cost[product];
        Storage[product] -= 10;
    }

    public List<LoanOffer> LoanOffers { get; set; }

    public void GenerateCost()
    {
        Cost = new Dictionary<string, double>() {
            {"Wood",Balance / Storage["Wood"]},
            {"Potato",Balance / Storage["Potato"]},
            {"Carrot",Balance / Storage["Carrot"]},
            {"Meet",Balance / Storage["Meet"]},
            {"Gold",Balance / Storage["Gold"]},
        };
        Inflation = Balance / 10000.0 -
                    (Storage["Wood"] + Storage["Potato"] + Storage["Carrot"] + Storage["Meet"] + Storage["Gold"]) /
                    7512.0;
    }
    public Dictionary<string, int> Storage { get; set; }
    public Dictionary<string, double> Cost { get; set; }
    
    public List<long> MyLoans { get; set; }
    
    public List<long> GettedLoans { get; set; }
    
    public void TakeLoan(long id)
    {
        
    }
    public void CreateLoan()
    {
        var random = new Random().Next() % Balance;
        var ID = new Random().Next();
        LoanOffers.Add(new LoanOffer(){
            id = ID,
            Year = 1 + new Random().Next() % 10,
            Value = random,
            Percentage = 0.1 + new Random().Next() % 1.0
        });
        MyLoans.Add(ID);
        Balance -= random;
    }
}