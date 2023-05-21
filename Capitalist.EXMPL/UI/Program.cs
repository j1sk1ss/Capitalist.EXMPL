using Capitalist.EXMPL.BANK;
using Capitalist.EXMPL.MANAGER;
using Capitalist.EXMPL.OBJECTS.BOT;
using Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS;
using Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS.FACTORY_TYPES;
using Capitalist.EXMPL.OBJECTS.MARKET;
using Capitalist.EXMPL.OBJECTS.PLAYER;
using Capitalist.EXMPL.UI.WINDOWS;

namespace Capitalist.EXMPL.UI;

internal class CapitalistGame {
    public static readonly Player Player = new(1000);
    
    public static readonly List<Bot> Bots = new() {
        new Bot(1200), new Bot(1200), 
        new Bot(1200), new Bot(1200), 
        new Bot(1200), new Bot(1200)
    };
    
    public static readonly Market Market = new();
    
    public static readonly EconomyAction EconomyAction = new();

    public static readonly Bank Bank = new(10000, .1d, .1d, .1d, Market);
    
    private static void Main() {
        var game = new CapitalistGame();
        game.Menu();
    }
    
    private void Menu() {
        while (true) {
            Console.Clear();
            Console.WriteLine(Interface.GetMenu(Player));
        
            switch (Console.ReadLine()!) {
                case "1": 
                    BalanceWindow();
                    break;
                case "2": 
                    GlobalMarketWindow();
                    break;
                case "3": 
                    InventoryWindow();
                    break;
                case "4": 
                    BusinessesWindow();
                    break;
            }
        
            EconomyAction.Step(1);  
        }
    }
    
    private static void BusinessesWindow() {
        Console.Clear();
        Console.WriteLine(Interface.GetFactoryMenu(Player));
        
        var answer = Console.ReadLine();
        if (answer == "") return;
        
        switch (int.Parse(answer!)) {
            case 1: OpenFactory(); break;
            case 2: ShowFactories(); break;
        }
    }
    
    private static void ShowFactories() {
        Console.Clear();
        Console.WriteLine(Interface.FactoryList());
        
        var tmp = Console.ReadLine()!;
        if (tmp == "999") return;
        
        ExtendedFactory(Player.Factories[int.Parse(tmp)]);
    }

    private static void ExtendedFactory(Factory factory) {
        Console.Clear();
        Console.WriteLine(Interface.ExtendedFactory(factory));
        
        switch (Console.ReadLine()) {
            case "1":
                factory.IsWork = !factory.IsWork;
                break;
            default: return;
        }
    }
    
    private static void OpenFactory() {
        Console.Clear();
        Console.WriteLine(Interface.FactoryManager(Player));
        
        double money;
        switch (Console.ReadLine()) {
            case "0":
                money = 80 + 80 * Market.Inflation;
                if (Player.Balance >= money) {
                    Player.Factories.Add(new WoodFactory(Player));
                    Player.Balance -= money;
                }
                
                break;
            case "1":
                money = 210 + 210 * Market.Inflation;
                if (Player.Balance >= money) {
                    Player.Factories.Add(new PotatoFactory(Player));
                    Player.Balance -= money;
                }
                
                break;
            case "2":
                money = 300 + 300 * Market.Inflation;
                if (Player.Balance >= money) {
                    Player.Factories.Add(new CarrotFactory(Player));
                    Player.Balance -= money;
                }
                
                break;
            case "3":
                money = 450 + 450 * Market.Inflation;
                if (Player.Balance >= money) {
                    Player.Factories.Add(new MeatFactory(Player));
                    Player.Balance -= money;
                }
                
                break;
            case "4":
                money = 1200 + 1200 * Market.Inflation;
                if (Player.Balance >= money) {
                    Player.Factories.Add(new GoldenFactory(Player));
                    Player.Balance -= money;
                }
                
                break;
            default: return;
        }
    }
    
    private static void BalanceWindow() {
        Console.Clear();
        Console.WriteLine(Interface.GetBalance(Player));
        
        var answer = Console.ReadLine();
        if (answer == "") return;
        
        switch (int.Parse(answer!)) {
            case 1: TakeLoanWindow(); break;
            case 2: GetLoans(); break;
        }
    }
    
    private static void InventoryWindow() {
        Console.Clear();
        Console.WriteLine(Interface.GetInventory(Player));
        Console.WriteLine("Press any key to exit:");
        Console.ReadLine();
    }
    
    private static void TakeLoanWindow() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("\nList of enabled loans:\n" +
                          "<==================================================>");
        
        for (var t = 0; t < Bank.LoanOffers.Count; t++)
            Console.WriteLine($"<====>\n{t})" + Bank.LoanOffers[t].Information());
        
        Console.WriteLine("<==================================================>" +
                          "\nChoose the number of loan or 999 to exit:");
        
        var i = int.Parse(Console.ReadLine()!);
        if (i == 999) return;
        
        var loan = Bank.LoanOffers[i];
        Bank.LoanOffers.RemoveAt(i);
        
        if (Player.GettedLoans.Contains(loan.Id)) {
            Player.Balance += loan.Value;
            Player.GettedLoans.RemoveAt(Player.GettedLoans.IndexOf(loan.Id));
        }
        else Bank.GetLoan(Player, loan);
    }
    
    private static string Balance() =>
        $"Ur balance is: {Math.Round(Player.Balance, 3)}$" + $", Inflation is: {Math.Round(Market.Inflation, 3)}%";
    
    private static void GetLoans() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("List of active loans:\n" +
                          "<==================================================>\n");
        
        foreach (var t1 in Player.LoanOffers)
            Console.WriteLine($"<====>\n"+t1.Information());
        
        Console.WriteLine("<==================================================>");
        Console.ReadLine();
    }
    
    private readonly List<string> _products = new() {"Wood","Potato","Carrot","Meat", "Gold"};
    
    private void GlobalMarketWindow() {
        for (var i = 0; i < Market.Storage.Count; i++) {
            Console.WriteLine($"{i}) "+_products[i]+" - "+Market.Storage[_products[i]]+
                              $" ({Math.Round(Market.Cost[_products[i]], 3)}$)");
        }
        
        Console.WriteLine($"Chose product for extended cost view");
        Console.WriteLine($"{Balance()}%\nTo buy enter num and 1, to sell 2, " +
                          "after _ add count (prices for 1 product)");
        
        var line = Console.ReadLine()!;
        
        if (line == "") return;

        if (line.Length == 1) {
            Console.WriteLine(Interface.CostGraph(int.Parse(line)));
            Console.ReadLine();
            return;
        }
        
        ChangeProducts(line.Split("_")[0] switch {
            "01" => "Wood_B", "11"   => "Potato_B",
            "21" => "Carrot_B", "31" => "Meet_B",
            "41" => "Gold_B", "02"   => "Wood_S",
            "12" => "Potato_S", "22" => "Carrot_S",
            "32" => "Meet_S", "42"   => "Gold_S",
            _ => ""
        }, int.Parse(line.Split("_")[1]));
    }
    
    private static void ChangeProducts(string name, int count) {
        switch (name[^1]) {
            case('B'):
                if (!Bank.Transaction(Player, Market, count * Market.Cost[name[..^2]])) return;
                
                Player.Inventory[name[..^2]] += count;
                Market.Storage[name[..^2]]   -= count;
                break;
            case ('S'): 
                if (!Bank.Transaction(Market, Player, count * Market.Cost[name[..^2]])) return;
                
                Player.Inventory[name[..^2]] -= count;
                Market.Storage[name[..^2]]   += count;
                break;
        }
    }
}