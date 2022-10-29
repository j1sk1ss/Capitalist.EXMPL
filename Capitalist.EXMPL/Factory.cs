namespace Capitalist.EXMPL;

public class Factory
{
    public Factory(string name, double payment) {
        Name = name;
        Payment = payment;
        Inventory = new Dictionary<string, int>() {
            {"Wood", 0},
            {"Potato", 0},
            {"Carrot", 0},
            {"Meet", 0},
            {"Gold", 0},
        };
    }
    public bool IsWork { get; set; }
    public string Name { get; set; }
    public double Payment { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    
    public void DoWork() {
        if (IsWork) {
            Inventory[Name switch {
                "Forest" => "Wood",
                "Farm1" => "Potato",
                "Farm2" => "Carrot",
                "Farm3" => "Meet",
                "Mine" => "Gold",
                _ => Name
            }] += 10;
        }
    }

    public int Take(string name, int count) {
        Inventory[name] -= count;
        return count;
    }
}