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
    
    private static string Balance(ICapitalist subject) =>
        $"Ur balance is: {Math.Round(subject.Balance, 3)}$" + $", Inflation is: {Math.Round(CapitalistGame.Market.Inflation, 3)}%";
}