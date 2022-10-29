namespace Capitalist.EXMPL;

public class Market
{
    public double Balance { get; set; }
    public double Inflation { get; private set; }
    public Market() {
        YearCost = new List<List<double>>();;
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
    public void GenerateCost() {
        Cost = new Dictionary<string, double>() {
            {"Wood",Balance / Storage["Wood"]},
            {"Potato",Balance / Storage["Potato"]},
            {"Carrot",Balance / Storage["Carrot"]},
            {"Meet",Balance / Storage["Meet"]},
            {"Gold",Balance / Storage["Gold"]},
        };
        if (YearCost.Count < 100) YearCost.Add(new List<double>() {Cost["Wood"], Cost["Potato"], Cost["Carrot"], Cost["Meet"], Cost["Gold"]});
        else YearCost.Clear();
        Inflation = Balance / 10000.0 -
                    (Storage["Wood"] + Storage["Potato"] + Storage["Carrot"] + Storage["Meet"] + Storage["Gold"]) /
                    7512.0;
    }
    private List<List<double>> YearCost { get; set; }
    public Dictionary<string, int> Storage { get; set; }
    public Dictionary<string, double> Cost { get; private set; }
    public List<long> MyLoans { get; set; }
    public List<long> GettedLoans { get; set; }
    public void CreateLoan() {
        var random = new Random().Next() % Balance;
        var id = new Random().Next();
            LoanOffers.Add(new LoanOffer(random, 1 + new Random().Next() % 640, 
                0.1 + (new Random().Next() % 10) / 10.0, id));
                MyLoans.Add(id);
        Balance -= random;
    }
    public List<double> Predict() {
        var answer = new double[5];
        for (var i = 0; i < 5; i++) {
            var k = 0.0;
            for (var j = 0; j < YearCost.Count; j++) {
                if (j == 0) k = YearCost[j][i];
                answer[i] += YearCost[j][i];
            }
            answer[i] = k - answer[i] / 5;
        }
        return answer.ToList();
    }
}