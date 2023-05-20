using Capitalist.EXMPL.BANK.LOAN;
using Capitalist.EXMPL.OBJECTS.FACTORY;
using Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS;

namespace Capitalist.EXMPL.OBJECTS.PLAYER;

public class Player : ICapitalist {
    public Player() {
        Balance   = 100;
        Inventory = new Dictionary<string, int>() {
            {"Wood", 0},
            {"Potato", 0},
            {"Carrot", 0},
            {"Meat", 0},
            {"Gold", 0},
        };
        
        LoanOffers  = new List<LoanOffer>();
        Factories   = new List<Factory>();
        MyLoans     = new List<long>();
        GettedLoans = new List<long>();
    }
    
    public double Balance { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public List<LoanOffer> LoanOffers { get; set; }
    public List<Factory> Factories { get; set; }
    public List<long> MyLoans { get; set; }
    public List<long> GettedLoans { get; set; }

    public void PlayerTurn() {
        foreach (var factory in Factories) {
            factory.DoWork();
            factory.FactoryTurn();
        }
    }
    
    public void Buy(string product, Dictionary<string, float> cost) {
        if (Balance >= cost[product]) Balance -= cost[product];
        Inventory[product] += 10;
    }
    
    public void Sell(string product, Dictionary<string, double> cost) {
        if (Inventory[product] >= 10) Balance += cost[product];
        Inventory[product] -= 10;
    }

    public void TakeLoan(LoanOffer loanOffer) {
        MyLoans.Add(loanOffer.Id);
        Balance += loanOffer.Value;
    } 
}