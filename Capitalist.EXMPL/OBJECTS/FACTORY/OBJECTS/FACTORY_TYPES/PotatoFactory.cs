using Capitalist.EXMPL.UI;

namespace Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS.FACTORY_TYPES;

public class PotatoFactory : Factory {
    public PotatoFactory(ICapitalist owner) : base(owner) {
        Owner = owner;
        Name  = "Potato factory";
        
        Cost = 210 + 210 * CapitalistGame.Market.Inflation;
    }
    
    private ICapitalist Owner { get; }

    public override double GetPayment() =>
        Math.Round(21 + 21 * CapitalistGame.Market.Inflation, 3);
    
    public override void DoWork() {
        if (Owner.Balance <= GetPayment() || !IsWork)
            return;
        
        Owner.Balance -= GetPayment();
        Owner.Inventory["Potato"] += 10;
    }
}