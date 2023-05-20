using Capitalist.EXMPL.UI;

namespace Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS.FACTORY_TYPES;

public class WoodFactory : Factory {
    public WoodFactory(ICapitalist owner) : base(owner) {
        Owner = owner;
        Name = "Wood factory";

        Cost = 80 + 80 * CapitalistGame.Market.Inflation;
    }
    
    private ICapitalist Owner { get; }

    public override double GetPayment() =>
        Math.Round(8 + 8 * CapitalistGame.Market.Inflation, 3);
    
    public override void DoWork() {
        if (Owner.Balance <= GetPayment() || !IsWork)
            return;
        
        Owner.Balance -= GetPayment();
        Owner.Inventory["Wood"] += 10;
    }
}