namespace Capitalist.EXMPL;

internal class CapitalistGame
{
    private readonly Player _player = new Player();
    private readonly List<Bot> _bots = new() {
        new Bot(), new Bot(), new Bot(), new Bot(), new Bot(), new Bot()
    };
    private readonly Market _market = new Market();
    private readonly EconomyAction _economyAction = new();
    private static void Main()
    {
        var game = new CapitalistGame();
        var action = new EconomyAction();
        game.Menu();
    }
    private void Menu()
    {
        Console.Clear();
        Console.WriteLine($"This is ur main game page \nDate is: {_economyAction.DateTime.ToString("MM/dd/yyyy")!}" +
                          " \nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          $"1) Show extended balance. {Balance()}" +
                          $"\n2) Show global market.\n3) Inventory window." +
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
        _economyAction.Step(_market, _products, 1, _bots, _player);
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
            case 0: return; break;
            case 1: OpenFactory(); break;
            case 2: ShowFactories(); break;
        }
    }
    
    private void ShowFactories()
    {
        Console.Clear();
        for (var i = 0; i < _player.factories.Count; i++) {
            Console.WriteLine($"{i}) {_player.factories[i].Name} ->" +
                              $" {_player.factories[i].Payment}");
        }
        Console.WriteLine($"Press any key or type a num to extend menu:");
        ExtendedFactory(_player.factories[int.Parse(Console.ReadLine()!)]);
    }

    private void ExtendedFactory(Factory factory)
    {
        Console.Clear();
        Console.WriteLine($"Name: {factory.Name}.\n" +
                          $"Payment: {factory.Payment}.\n" +
                          $"Is working?: {factory.IsWork}");
        Console.WriteLine("<====>\n1) Turn on/off this factory.\n " +
                          "2) Get all stuff from storage to inventory.");
        switch (Console.ReadLine())
        {
            case "1":
                factory.IsWork = !factory.IsWork;
                break;
            case "2":
                for (var i = 0; i < factory.Inventory.Count; i++) {
                    _player.Inventory[_products[i]] += factory.Inventory[_products[i]];
                    factory.Inventory[_products[i]] = 0;
                }
                break;
            default: return;
        }
        Console.WriteLine($"Press any key to exit:");
        Console.ReadLine();
    }
    private void OpenFactory()
    {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine($"0) Wood factory. ({Math.Round(80 + 80 * _market.Inflation,3)}$)\n" +
                          $"1) Potato farm. ({Math.Round(210 + 210 * _market.Inflation,3)}$\n" +
                          $"2) Carrot farm. ({Math.Round(300 + 300 * _market.Inflation,3)}$)\n" +
                          $"3) Meet farm. ({Math.Round(450 + 450 * _market.Inflation,3)}$)\n" +
                          $"4) Golden mine. ({Math.Round(1200 + 1200 * _market.Inflation,3)}$)");
        var money = 0.0;
        switch (Console.ReadLine()) {
            case "0":
                money = 80 + 80 * _market.Inflation;
                if (_player.Balance > money) {
                    _player.Balance -= money;
                    _market.Balance += money;
                    _player.factories.Add(new Factory("Wood", 1.65));
                }
                break;
            case "1":
                money = 80 + 80 * _market.Inflation;
                if (_player.Balance > money) {
                    _player.Balance -= money;
                    _market.Balance += money;
                    _player.factories.Add(new Factory("Potato", 2.35));
                }
                break;
            case "2":
                money = 80 + 80 * _market.Inflation;
                if (_player.Balance > money) {
                    _player.Balance -= money;
                    _market.Balance += money;
                    _player.factories.Add(new Factory("Carrot", 3.65));
                }
                break;
            case "3":
                money = 80 + 80 * _market.Inflation;
                if (_player.Balance > money) {
                    _player.Balance -= money;
                    _market.Balance += money;
                    _player.factories.Add(new Factory("Meet", 4.0));
                }
                break;
            case "4":
                money = 80 + 80 * _market.Inflation;
                if (_player.Balance > money) {
                    _player.Balance -= money;
                    _market.Balance += money;
                    _player.factories.Add(new Factory("Gold", 6.9));
                }
                break;
            default: return;
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
            case 0: return; break;
            case 1: TakeLoanWindow(); break;
            case 2: GetLoans(); break;
        }
    }
    private void InventoryWindow() {
        Console.Clear();
        for (var i = 0; i < _player.Inventory.Count; i++) {
            Console.WriteLine($"{i}) {_products[i]} -> {_player.Inventory[_products[i]]}");
        }
        Console.WriteLine($"Press any key to exit:");
        Console.ReadLine();
    }
    private void TakeLoanWindow() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("\nList of enabled loans:\n" +
                          "<==================================================>");
        for (var t = 0; t < _market.LoanOffers.Count; t++)
            Console.WriteLine($"<====>\n{t})"+_market.LoanOffers[t].Information());
        Console.WriteLine("<==================================================>" +
                          "\nChoose the number of loan or 999 to exit:");
        var i = int.Parse(Console.ReadLine()!);
        if (i == 999) return;
        var loan = _market.LoanOffers[i];
        _market.LoanOffers.RemoveAt(i);
        if (_player.GettedLoans.Contains(loan.id)) {
            _player.Balance += loan.Value;
            _player.GettedLoans.RemoveAt(_player.GettedLoans.IndexOf(loan.id));
        }
        else {
            _player.Balance += loan.Value;
            _player.LoanOffers.Add(loan);
            _market.GettedLoans.Add(loan.id);
            _market.MyLoans.RemoveAt(_market.MyLoans.IndexOf(loan.id));
        }
    }
    private string Balance()
    {
        return $"Ur balance is: {Math.Round(_player.Balance, 3)}$" +
               $", Inflation is: {Math.Round(_market.Inflation, 3)}%";
    }
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
        for (var i = 0; i < _market.Storage.Count; i++) {
            Console.WriteLine($"{i}) "+_products[i]+" - "+_market.Storage[_products[i]]+
                              $" ({Math.Round(_market.Cost[_products[i]], 3)}$)");
        }
        Console.WriteLine($"Ur balance is: {Math.Round(_player.Balance, 3)}$" +
                          $", Inflation is: {Math.Round(_market.Inflation, 3)}%\nTo buy enter num and 1, to sell 2, " +
                          "after _ add count (prices for 1 product)");
        var line = Console.ReadLine()!;
        if (line == "") return;
        ChangeProducts(line.Split("_")[0] switch {
            "01" => "Wood_B", "11" => "Potato_B",
            "21" => "Carrot_B", "31" => "Meet_B",
            "41" => "Gold_B", "02" => "Wood_S",
            "12" => "Potato_S", "22" => "Carrot_S",
            "32" => "Meet_S", "42" => "Gold_S",
            _ => ""
        }, int.Parse(line.Split("_")[1]));
    }
    private void ChangeProducts(string name, int count) {
        switch (name[^1])
        {
            case('B'): 
                if (!(_player.Balance >= count * _market.Cost[name[..^2]])) return;
                _player.Balance -= count * _market.Cost[name[..^2]];
                _market.Balance += count * _market.Cost[name[..^2]];
                _player.Inventory[name[..^2]] += count;
                _market.Storage[name[..^2]] -= count;
                break;
            case ('S'): 
                if (!(_market.Balance >= count * _market.Cost[name[..^2]])) return;
                _player.Balance += count * _market.Cost[name[..^2]];
                _market.Balance -= count * _market.Cost[name[..^2]];
                _player.Inventory[name[..^2]] -= count;
                _market.Storage[name[..^2]] += count;
                break;
        }
    }
}