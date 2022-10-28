namespace Capitalist.EXMPL;

public class Player {
    public Player() {
        Balance = 100;
        Inventory = new Dictionary<string, int>() {
            {"Wood", 0},
            {"Potato", 0},
            {"Carrot", 0},
            {"Meet", 0},
            {"Gold", 0},
        };
    }
    public double Balance { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
}