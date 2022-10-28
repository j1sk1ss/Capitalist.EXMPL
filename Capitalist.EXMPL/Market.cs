namespace Capitalist.EXMPL;

public class Market
{
    public double Balance { get; set; }
    public Market() {
        Balance = 10000;
        Storage = new Dictionary<string, int>() {
            {"Wood", 2125},
            {"Potato", 3875},
            {"Carrot", 980},
            {"Meet", 459},
            {"Gold", 73},
        };
        Cost = new Dictionary<string, double>();
        LoanOffers = new List<LoanOffer>();
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
    }
    public Dictionary<string, int> Storage { get; set; }
    public Dictionary<string, double> Cost { get; set; }
}