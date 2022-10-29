﻿namespace Capitalist.EXMPL;

internal class CapitalistGame
{
    private readonly Player _player = new Player();
    private List<Bot> _bots = new List<Bot>();
    private Market _market = new Market();
    private EconomyAction _economyAction = new();
    private static void Main()
    {
        var game = new CapitalistGame();
        var action = new EconomyAction();
        game.Menu();
    }

    private void Menu()
    {
        Console.Clear();
        Console.WriteLine($"This is ur main game page \nDate is: {_economyAction._dateTime.ToString("MM/dd/yyyy")!}" +
                          " \nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          $"1) Show extended balance. ({Math.Round(_player.Balance, 3)}$)" +
                          $" Inflation is: {Math.Round(_market.Inflation, 3)}%" +
                          $" \n2) Show global market. \n" +
                          "3) Trade banknotes. \n4) Inventory window. \n" +
                          "5) Show businesses list. \n6) Show workers market. \n" +
                          "7) Next step (after _ count of days)");
        var tmp = Console.ReadLine();
        if (tmp!.Length > 1) {
            Console.Clear();
            _economyAction.Step(_market, _products, int.Parse(tmp.Split("_")[1]));
        }
        switch (tmp) {
            case "1": {
                Console.Clear();
                BalanceWindow();
            } break;
            case "2": {
                Console.Clear();
                GlobalMarketWindow();
            } break;
            case "4": {
                Console.Clear();
                InventoryWindow();
            } break;
        }
        Menu();
    }

    private void BalanceWindow() {
        Console.Clear();
        Console.WriteLine($"Ur balance is: {Math.Round(_player.Balance, 3)}$" +
                          $", Inflation is: {Math.Round(_market.Inflation, 3)}%");
        Console.WriteLine("This is ur balance page" +
                          "\nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          "1) Take loan. \n2) Get loan. \n");
        switch (int.Parse(Console.ReadLine()!)) {
            case 0: return; break;
            case 1: TakeLoanWindow(); break;
            case 2: CreateLoanOffer(); break;
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
        Console.WriteLine($"Ur balance is: {Math.Round(_player.Balance, 3)}$" +
                          $", Inflation is: {Math.Round(_market.Inflation, 3)}%");
        Console.WriteLine("\nList of enabled loans: \n" +
                          "<==================================================>");
        for (var t = 0; t < _market.LoanOffers.Count; t++)
            Console.WriteLine($"<====>\n{t})"+_market.LoanOffers[t].Information());
        Console.WriteLine("Choose the number of loan or 999 to exit: ");
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
    private void CreateLoanOffer() {
        Console.Clear();
        Console.WriteLine($"Ur balance is: {Math.Round(_player.Balance, 3)}$" +
                          $", Inflation is: {Math.Round(_market.Inflation, 3)}%");
        Console.WriteLine("Type loan value: ");
        var vl = double.Parse(Console.ReadLine()!);
            Console.WriteLine("Type percentage: ");
            var percentage = double.Parse(Console.ReadLine()!);
                Console.WriteLine("Type years: ");
                var years = int.Parse(Console.ReadLine()!);
                var loanOffer = new LoanOffer() {
            Value =vl,
            Percentage = percentage,
            Year = years
        };
        _player.Balance -= loanOffer.Value;
        _player.GettedLoans.Add(loanOffer.id);
        _player.LoanOffers.Add(loanOffer);
        _market.LoanOffers.Add(loanOffer);
    }
    private readonly List<string> _products = new() {"Wood","Potato","Carrot","Meet", "Gold"};
    private void GlobalMarketWindow() {
        for (var i = 0; i < _market.Storage.Count; i++) {
            Console.WriteLine($"{i}) "+_products[i]+" - "+_market.Storage[_products[i]]+
                              $" ({Math.Round(_market.Cost[_products[i]], 3)}$)");
        }
        Console.WriteLine($"Ur balance is: {Math.Round(_player.Balance, 3)}$" +
                          $", Inflation is: {Math.Round(_market.Inflation, 3)}%");
        Console.WriteLine("To buy enter num and 1, to sell 2, after _ add count (prices for 1 product) / 999 -> EXIT");

        var line = Console.ReadLine()!;
        if (line == "999") return;
        var tmp = line.Split("_");
        ChangeProducts(int.Parse(tmp[0]) switch {
            01 => "Wood_B",
            11 => "Potato_B",
            21 => "Carrot_B",
            31 => "Meet_B",
            41 => "Gold_B",
            02 => "Wood_S",
            12 => "Potato_S",
            22 => "Carrot_S",
            32 => "Meet_S",
            42 => "Gold_S"}, int.Parse(tmp[1]));
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