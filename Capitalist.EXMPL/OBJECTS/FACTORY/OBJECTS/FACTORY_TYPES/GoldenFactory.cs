using Capitalist.EXMPL.UI;

namespace Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS.FACTORY_TYPES;

public class GoldenFactory : Factory {
    public GoldenFactory(ICapitalist owner) : base(owner) {
        Owner = owner;
        Name  = "Golden factory";
        
        Cost = 1200 + 1200 * CapitalistGame.Market.Inflation;
    }
    
    private ICapitalist Owner { get; }

    public override double GetPayment() =>
        Math.Round(120 + 120 * CapitalistGame.Market.Inflation, 3);
    
    public override void DoWork() {
        if (Owner.Balance <= GetPayment() || !IsWork)
            return;
        
        Owner.Balance -= GetPayment();
        Owner.Inventory["Gold"] += 10;
    }
}