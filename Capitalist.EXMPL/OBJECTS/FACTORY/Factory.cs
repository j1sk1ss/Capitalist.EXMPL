namespace Capitalist.EXMPL.OBJECTS.FACTORY;

public class Factory {
    public Factory(string name, double payment, ICapitalist owner) {
        Name    = name;
        Payment = payment;

        Owner = owner;
    }
    
    public bool IsWork { get; set; }
    public string Name { get; }
    public double Payment { get; }
    
    private ICapitalist Owner { get; }
    
    public void DoWork() {
        if (Owner.Balance <= Payment)
            return;
        else
            Owner.Balance -= Payment;
        
        if (IsWork) {
            Owner.Inventory[Name switch {
                "Forest" => "Wood",
                "Farm1"  => "Potato",
                "Farm2"  => "Carrot",
                "Farm3"  => "Meet",
                "Mine"   => "Gold",
                _        => Name
            }] += 10;
        }
    }
}