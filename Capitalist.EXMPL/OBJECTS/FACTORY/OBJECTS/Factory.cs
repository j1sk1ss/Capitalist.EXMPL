using Capitalist.EXMPL.UI;

namespace Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS;

public abstract class Factory {
    protected Factory(ICapitalist owner) {
        Owner = owner;
        Name = "";
    }
    
    public string Name;
    public bool IsWork { get; set; }
    
    private ICapitalist Owner { get; }

    public abstract void DoWork();

    public abstract double GetPayment();

    public static double Cost;

    public void FactoryTurn() {
        if (Owner.Balance < GetPayment()) {
            IsWork = false;
            return;
        }
        
        CapitalistGame.Bank.Budget += GetPayment();
        Owner.Balance -= GetPayment();
    }
}