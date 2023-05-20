using Capitalist.EXMPL.UI;

namespace Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS.FACTORY_TYPES;

public class CarrotFactory : Factory {
    public CarrotFactory(ICapitalist owner) : base(owner) {
        Owner = owner;
        Name  = "Carrot";
        
        Cost = 300 + 300 * CapitalistGame.Market.Inflation;
    }
    
    private ICapitalist Owner { get; }

    public override double GetPayment() =>
        Math.Round(30 + 30 * CapitalistGame.Market.Inflation, 3);
    
    public override void DoWork() {
        if (Owner.Balance <= GetPayment() || !IsWork)
            return;
        
        Owner.Balance -= GetPayment();
        Owner.Inventory["Carrot"] += 10;
    }
}