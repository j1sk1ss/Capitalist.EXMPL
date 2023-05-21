using System.Text;
using Capitalist.EXMPL.OBJECTS;
using Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS;

namespace Capitalist.EXMPL.UI.WINDOWS;

public static class Interface {
    public static string GetMenu(ICapitalist subject) =>
        $"This is ur main game page \nDate is: {CapitalistGame.EconomyAction.DateTime.ToString("MM/dd/yyyy")!}" +
        " \nList of enabled actions: \n" +
        "<==================================================> \n" +
        $"1) Show extended balance. {Balance(subject)}" +
        "\n2) Show global market.\n3) Inventory window." +
        "\n4) Show businesses list.";

    public static string GetFactoryMenu(ICapitalist subject) => $"{Balance(subject)}\nChoose layer:" +
                                             "\nList of enabled actions: \n" + 
                                             "<==================================================> \n" + 
                                             "1) Open factory. \n2) Ur factories. \n";

    public static string FactoryList() {
        var currentInterface = "";
        for (var i = 0; i < CapitalistGame.Player.Factories.Count; i++) 
            currentInterface += $"{i}) {CapitalistGame.Player.Factories[i].Name} ->" + 
                              $" {Math.Round(CapitalistGame.Player.Factories[i].GetPayment() + CapitalistGame.Player.Factories[i].GetPayment() * CapitalistGame.Market.Inflation,3)}$\n";
        
        currentInterface += "Press 999 or type a num to extend menu:";
        return currentInterface;
    }
    
    public static string ExtendedFactory(Factory factory) {
        var extendedFactory = "";
        extendedFactory += $"Name: {factory.Name}.\n" +
                          $"Payment: {Math.Round(factory.GetPayment() + factory.GetPayment() * CapitalistGame.Market.Inflation,3)}$.\n" +
                          $"Is working?: {factory.IsWork}\n<====>\n1) Turn on/off this factory.\nPress any key to exit:";
        
        return extendedFactory;
    }
    
    private static readonly List<string> Products = new() {"Wood","Potato","Carrot","Meat", "Gold"};
    
    public static string GetInventory(ICapitalist subject) {
        var inventory = "";
        
        for (var i = 0; i < subject.Inventory.Count; i++) 
            inventory += $"{i}) {Products[i]} -> {CapitalistGame.Player.Inventory[Products[i]]}\n";
        
        inventory += "Press any key to exit:";
        return inventory;
    }

    public static string FactoryManager(ICapitalist subject) {
        var current = Balance(subject);
        current += $"0) Wood factory. ({Math.Round(80 + 80 * CapitalistGame.Market.Inflation, 3)}$)\n" +
                   $"1) Potato farm.  ({Math.Round(210 + 210 * CapitalistGame.Market.Inflation, 3)}$\n" +
                   $"2) Carrot farm.  ({Math.Round(300 + 300 * CapitalistGame.Market.Inflation, 3)}$)\n" +
                   $"3) Meat farm.    ({Math.Round(450 + 450 * CapitalistGame.Market.Inflation, 3)}$)\n" +
                   $"4) Golden mine.  ({Math.Round(1200 + 1200 * CapitalistGame.Market.Inflation, 3)}$)";

        return current;
    }
    
    public static string GetBalance(ICapitalist subject) {
       var current = Balance(subject);
       current += "\n \n" + InflationGraph() + "\n";
       current += $"\nBank VAT: {CapitalistGame.Bank.Vat}%\nBank TAX: {CapitalistGame.Bank.TaxPercent}%\n";
       current += "\nThis is ur balance page" +
                  "\nList of enabled actions: \n" +
                  "<==================================================> \n" +
                  "1) Take loan. \n2) Ur loans. \n";

       return current;
    }
    
    public static string CostGraph(int position) =>
         DrawGraph(CapitalistGame.Market.YearCost.Select(year => year[position]).ToList());

    private static string InflationGraph() =>
        DrawGraph(CapitalistGame.Market.YearInflation);

    private static string DrawGraph(IReadOnlyList<double> values) {
        var max = values.Max();
        var min = values.Min();
        
        var graphBuilder = new StringBuilder();
        graphBuilder.Append(Math.Round(max, 2));
        graphBuilder.AppendLine();
        
        const int height = 10;
        const int width = 60;
        
        for (var row = 0; row <= height; row++) {
            for (var col = 0; col < width; col++) {
                var index = col * values.Count / width;
                var num = values[index];

                if (num >= max - row * max / height)
                    graphBuilder.Append(row == height ? ' ' : '#');
                else
                    graphBuilder.Append(' ');
            }
            
            graphBuilder.AppendLine();
        }
        
        graphBuilder.AppendLine();
        graphBuilder.Append(Math.Round(min, 2));

        return graphBuilder.ToString();
    }
    
    private static string Balance(ICapitalist subject) =>
        $"Ur balance is: {Math.Round(subject.Balance, 3)}$" + $", Inflation is: {Math.Round(CapitalistGame.Market.Inflation, 3)}%";
}