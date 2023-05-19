using Capitalist.EXMPL.BANK;
using Capitalist.EXMPL.MANAGER;
using Capitalist.EXMPL.OBJECTS.BOT;
using Capitalist.EXMPL.OBJECTS.FACTORY;
using Capitalist.EXMPL.OBJECTS.MARKET;
using Capitalist.EXMPL.OBJECTS.PLAYER;

namespace Capitalist.EXMPL.UI;

internal class CapitalistGame {
    private readonly Player _player = new();
    
    private readonly List<Bot> _bots = new() {
        new Bot(), new Bot(), new Bot(), new Bot(), new Bot(), new Bot()
    };
    
    private static readonly Market Market = new();
    
    private readonly EconomyAction _economyAction = new();

    private readonly Bank _bank = new(10000, .1d, .1d, .1d, Market);
    
    private static void Main() {
        var game = new CapitalistGame();
        game.Menu();
    }
    
    private void Menu() {
        Console.Clear();
        Console.WriteLine($"This is ur main game page \nDate is: {_economyAction.DateTime.ToString("MM/dd/yyyy")!}" +
                          " \nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          $"1) Show extended balance. {Balance()}" +
                          "\n2) Show global market.\n3) Inventory window." +
                          "\n4) Show businesses list.");
        
        switch (Console.ReadLine()!) {
            case "1": {
                Console.Clear();
                BalanceWindow();
            } break;
            case "2": {
                Console.Clear();
                GlobalMarketWindow();
            } break;
            case "3": {
                Console.Clear();
                InventoryWindow();
            } break;
            case "4": 
                Console.Clear();
                BusinessesWindow();
                break;
        }
        
        _economyAction.Step(Market, 1, _bots, _player);
        Menu();
    }
    
    private void BusinessesWindow() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("Choose layer:" +
                          "\nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          "1) Open factory. \n2) Ur factories. \n");
        
        switch (int.Parse(Console.ReadLine()!)) {
            case 1: OpenFactory(); break;
            case 2: ShowFactories(); break;
        }
    }
    
    private void ShowFactories() {
        Console.Clear();
        
        for (var i = 0; i < _player.Factories.Count; i++) 
            Console.WriteLine($"{i}) {_player.Factories[i].Name} ->" + 
            $" {Math.Round(_player.Factories[i].Payment + _player.Factories[i].Payment*Market.Inflation,3)}$");
        
        Console.WriteLine("Press 999 or type a num to extend menu:");
        
        var tmp = Console.ReadLine()!;
        if (tmp == "999") return;
        
        ExtendedFactory(_player.Factories[int.Parse(tmp)]);
    }

    private void ExtendedFactory(Factory factory) {
        Console.Clear();
        Console.WriteLine($"Name: {factory.Name}.\n" +
                          $"Payment: {Math.Round(factory.Payment + factory.Payment * Market.Inflation,3)}$.\n" +
                          $"Is working?: {factory.IsWork}");
        Console.WriteLine("<====>\n1) Turn on/off this factory.\n " +
                          "2) Get all stuff from storage to inventory.");
        
        switch (Console.ReadLine()) {
            case "1":
                factory.IsWork = !factory.IsWork;
                break;
            case "2":
                //for (var i = 0; i < factory.Inventory.Count; i++) {
                //    _player.Inventory[_products[i]] += factory.Inventory[_products[i]];
                 //   factory.Inventory[_products[i]] = 0;
                //}
                break;
            default: return;
        }
        
        Console.WriteLine($"Press any key to exit:");
        Console.ReadLine();
    }
    
    private void OpenFactory() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine($"0) Wood factory. ({Math.Round(80 + 80 * Market.Inflation, 3)}$)\n" +
                          $"1) Potato farm. ({Math.Round(210 + 210 * Market.Inflation, 3)}$\n" +
                          $"2) Carrot farm. ({Math.Round(300 + 300 * Market.Inflation, 3)}$)\n" +
                          $"3) Meet farm. ({Math.Round(450 + 450 * Market.Inflation, 3)}$)\n" +
                          $"4) Golden mine. ({Math.Round(1200 + 1200 * Market.Inflation, 3)}$)");
        
        double money, payment;
        string product;
        
        switch (Console.ReadLine()) {
            case "0":
                money = 80 + 80 * Market.Inflation;
                payment = 1.65d + 1.65d * Market.Inflation;
                product = "Wood";
                break;
            case "1":
                money = 210 + 210 * Market.Inflation;
                payment = 2.35d + 2.35d * Market.Inflation;
                product = "Potato";
                break;
            case "2":
                money = 300 + 300 * Market.Inflation;
                payment = 2.35d + 2.35d * Market.Inflation;
                product = "Carrot";
                break;
            case "3":
                money = 450 + 450 * Market.Inflation;
                payment = 4d + 4d * Market.Inflation;
                product = "Meet";
                break;
            case "4":
                money = 1200 + 1200 * Market.Inflation;
                payment = 6.9d + 6.9d * Market.Inflation;
                product = "Gold";
                break;
            default: return;
        }
        
        if (_player.Balance > money) {
            _player.Balance -= money;
            Market.Balance += money;
            //_player.Factories.Add(new Factory(product, payment));
        }
    }
    
    
    
    
    private void BalanceWindow() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("This is ur balance page" +
                          "\nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          "1) Take loan. \n2) Ur loans. \n");
        
        switch (int.Parse(Console.ReadLine()!)) {
            case 1: TakeLoanWindow(); break;
            case 2: GetLoans(); break;
        }
    }
    
    private void InventoryWindow() {
        Console.Clear();
        
        for (var i = 0; i < _player.Inventory.Count; i++) 
            Console.WriteLine($"{i}) {_products[i]} -> {_player.Inventory[_products[i]]}");
        
        Console.WriteLine($"Press any key to exit:");
        Console.ReadLine();
    }
    
    private void TakeLoanWindow() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("\nList of enabled loans:\n" +
                          "<==================================================>");
        
        for (var t = 0; t < _bank.LoanOffers.Count; t++)
            Console.WriteLine($"<====>\n{t})" + _bank.LoanOffers[t].Information());
        
        Console.WriteLine("<==================================================>" +
                          "\nChoose the number of loan or 999 to exit:");
        
        var i = int.Parse(Console.ReadLine()!);
        if (i == 999) return;
        
        var loan = _bank.LoanOffers[i];
        _bank.LoanOffers.RemoveAt(i);
        
        if (_player.GettedLoans.Contains(loan.Id)) {
            _player.Balance += loan.Value;
            _player.GettedLoans.RemoveAt(_player.GettedLoans.IndexOf(loan.Id));
        }
        else _bank.GetLoan(_player, loan);
    }
    
    private string Balance() =>
        $"Ur balance is: {Math.Round(_player.Balance, 3)}$" + $", Inflation is: {Math.Round(Market.Inflation, 3)}%";
    
    private void GetLoans() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("List of active loans:\n" +
                          "<==================================================>\n");
        
        foreach (var t1 in _player.LoanOffers)
            Console.WriteLine($"<====>\n"+t1.Information());
        
        Console.WriteLine("<==================================================>");
        Console.ReadLine();
    }
    
    private readonly List<string> _products = new() {"Wood","Potato","Carrot","Meet", "Gold"};
    
    private void GlobalMarketWindow() {
        for (var i = 0; i < Market.Storage.Count; i++) {
            Console.WriteLine($"{i}) "+_products[i]+" - "+Market.Storage[_products[i]]+
                              $" ({Math.Round(Market.Cost[_products[i]], 3)}$)");
        }
        
        Console.WriteLine($"Ur balance is: {Math.Round(_player.Balance, 3)}$" +
                          $", Inflation is: {Math.Round(Market.Inflation, 3)}%\nTo buy enter num and 1, to sell 2, " +
                          "after _ add count (prices for 1 product)");
        
        var line = Console.ReadLine()!;
        if (line == "") return;
        
        ChangeProducts(line.Split("_")[0] switch {
            "01" => "Wood_B", "11"   => "Potato_B",
            "21" => "Carrot_B", "31" => "Meet_B",
            "41" => "Gold_B", "02"   => "Wood_S",
            "12" => "Potato_S", "22" => "Carrot_S",
            "32" => "Meet_S", "42"   => "Gold_S",
            _ => ""
        }, int.Parse(line.Split("_")[1]));
    }
    
    private void ChangeProducts(string name, int count) {
        switch (name[^1]) {
            case('B'):
                if (!_bank.Transaction(_player, Market, count * Market.Cost[name[..^2]])) return;
                
                _player.Inventory[name[..^2]] += count;
                Market.Storage[name[..^2]]    -= count;
                break;
            case ('S'): 
                if (!_bank.Transaction(Market, _player, count * Market.Cost[name[..^2]])) return;
                
                _player.Inventory[name[..^2]] -= count;
                Market.Storage[name[..^2]]    += count;
                break;
        }
    }
}