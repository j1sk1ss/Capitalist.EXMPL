using Capitalist.EXMPL.UI;

namespace Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS.FACTORY_TYPES;

public class MeatFactory : Factory {
    public MeatFactory(ICapitalist owner) : base(owner) {
        Owner = owner;
        Name  = "Meat factory";
        
        Cost = 450 + 450 * CapitalistGame.Market.Inflation;
    }
    
    private ICapitalist Owner { get; }

    public override double GetPayment() =>
        Math.Round(45 + 45 * CapitalistGame.Market.Inflation, 3);
    
    public override void DoWork() {
        if (Owner.Balance <= GetPayment() || !IsWork)
            return;
        
        Owner.Balance -= GetPayment();
        Owner.Inventory["Meat"] += 10;
    }
}