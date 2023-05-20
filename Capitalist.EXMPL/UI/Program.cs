﻿using Capitalist.EXMPL.BANK;
using Capitalist.EXMPL.MANAGER;
using Capitalist.EXMPL.OBJECTS.BOT;
using Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS;
using Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS.FACTORY_TYPES;
using Capitalist.EXMPL.OBJECTS.MARKET;
using Capitalist.EXMPL.OBJECTS.PLAYER;

namespace Capitalist.EXMPL.UI;

internal class CapitalistGame {
    public static readonly Player Player = new();
    
    public static readonly List<Bot> Bots = new() {
        new Bot(), new Bot(), new Bot(), new Bot(), new Bot(), new Bot()
    };
    
    public static readonly Market Market = new();
    
    public static readonly EconomyAction EconomyAction = new();

    public static readonly Bank Bank = new(10000, .1d, .1d, .1d, Market);
    
    private static void Main() {
        var game = new CapitalistGame();
        game.Menu();
    }
    
    private void Menu() {
        Console.Clear();
        Console.WriteLine($"This is ur main game page \nDate is: {EconomyAction.DateTime.ToString("MM/dd/yyyy")!}" +
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
        
        EconomyAction.Step(1);
        Menu();
    }
    
    private void BusinessesWindow() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("Choose layer:" +
                          "\nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          "1) Open factory. \n2) Ur factories. \n");

        var answer = Console.ReadLine();
        if (answer == "") return;
        
        switch (int.Parse(answer!)) {
            case 1: OpenFactory(); break;
            case 2: ShowFactories(); break;
        }
    }
    
    private void ShowFactories() {
        Console.Clear();
        
        for (var i = 0; i < Player.Factories.Count; i++) 
            Console.WriteLine($"{i}) {Player.Factories[i].Name} ->" + 
            $" {Math.Round(Player.Factories[i].GetPayment() + Player.Factories[i].GetPayment() * Market.Inflation,3)}$");
        
        Console.WriteLine("Press 999 or type a num to extend menu:");
        
        var tmp = Console.ReadLine()!;
        if (tmp == "999") return;
        
        ExtendedFactory(Player.Factories[int.Parse(tmp)]);
    }

    private void ExtendedFactory(Factory factory) {
        Console.Clear();
        Console.WriteLine($"Name: {factory.Name}.\n" +
                          $"Payment: {Math.Round(factory.GetPayment() + factory.GetPayment() * Market.Inflation,3)}$.\n" +
                          $"Is working?: {factory.IsWork}");
        Console.WriteLine("<====>\n1) Turn on/off this factory.");
        
        switch (Console.ReadLine()) {
            case "1":
                factory.IsWork = !factory.IsWork;
                break;
            default: return;
        }
        
        Console.WriteLine("Press any key to exit:");
        Console.ReadLine();
    }
    
    private void OpenFactory() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine($"0) Wood factory. ({Math.Round(80 + 80 * Market.Inflation, 3)}$)\n" +
                          $"1) Potato farm. ({Math.Round(210 + 210 * Market.Inflation, 3)}$\n" +
                          $"2) Carrot farm. ({Math.Round(300 + 300 * Market.Inflation, 3)}$)\n" +
                          $"3) Meat farm. ({Math.Round(450 + 450 * Market.Inflation, 3)}$)\n" +
                          $"4) Golden mine. ({Math.Round(1200 + 1200 * Market.Inflation, 3)}$)");

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
    
    private void BalanceWindow() {
        Console.Clear();
        Console.WriteLine(Balance());
        Console.WriteLine("This is ur balance page" +
                          "\nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          "1) Take loan. \n2) Ur loans. \n");

        var answer = Console.ReadLine();
        if (answer == "") return;
        
        switch (int.Parse(answer!)) {
            case 1: TakeLoanWindow(); break;
            case 2: GetLoans(); break;
        }
    }
    
    private void InventoryWindow() {
        Console.Clear();
        
        for (var i = 0; i < Player.Inventory.Count; i++) 
            Console.WriteLine($"{i}) {_products[i]} -> {Player.Inventory[_products[i]]}");
        
        Console.WriteLine("Press any key to exit:");
        Console.ReadLine();
    }
    
    private void TakeLoanWindow() {
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
    
    private string Balance() =>
        $"Ur balance is: {Math.Round(Player.Balance, 3)}$" + $", Inflation is: {Math.Round(Market.Inflation, 3)}%";
    
    private void GetLoans() {
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
        
        Console.WriteLine($"Ur balance is: {Math.Round(Player.Balance, 3)}$" +
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