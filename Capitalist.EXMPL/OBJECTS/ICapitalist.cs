using Capitalist.EXMPL.BANK.LOAN;

namespace Capitalist.EXMPL.OBJECTS;

public interface ICapitalist {
    public double Balance { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public void Buy(string product, Dictionary<string, float> cost);
    public void Sell(string product, Dictionary<string, float> cost);
    public List<LoanOffer> LoanOffers { get; set; }
    public List<long> MyLoans { get; set; }
}