using Capitalist.EXMPL.UI;

namespace Capitalist.EXMPL.MANAGER;

public class EconomyAction {
    public DateTime DateTime = DateTime.Now;
    public void Step(int count) {
        for (var i = 0; i < count; i++) {
            if (CapitalistGame.Player.Balance < 0) return;
            
            DateTime = DateTime.AddDays(1);

            foreach (var bot in CapitalistGame.Bots) 
                bot.BotTurn();
            
            CapitalistGame.Bank.BankTurn();
            CapitalistGame.Market.MarketTurn();
            CapitalistGame.Player.PlayerTurn();
        }
    }
}