namespace Capitalist.EXMPL;

public class EconomyAction
{
    public DateTime DateTime = DateTime.Now;
    public void Step(Market market, List<string> keys, int count, List<Bot> bots, Player player) {
        for (var i = 0; i < count; i++) {
            if (player.Balance < 0) return;
            var dateTime = DateTime.AddDays(1);
            DateTime = dateTime;

            foreach (var t in bots) {
                BotLogic(t, market);
            }

            for (var t = 0; t < player.LoanOffers.Count; t++) {
                market.Balance += player.LoanOffers[t].Payment;
                player.Balance -= player.LoanOffers[t].Payment;
                if (--player.LoanOffers[t].Year > 1) continue;
                    player.LoanOffers.RemoveAt(t);
                    break;
            }
            foreach (var t1 in player.factories) {
                t1.DoWork();
                if (t1.IsWork) {
                    market.Balance += t1.Payment + t1.Payment * market.Inflation;
                    player.Balance -= t1.Payment + t1.Payment * market.Inflation;
                }
            }
            Refile(market, keys);
            //Destroy(market, keys);
            market.GenerateCost();
                switch (market.LoanOffers.Count) {
                    case < 10 when market.Inflation > 5.0:
                        market.CreateLoan();
                        break;
                    case >= 1 when market.Inflation > 0 && market.LoanOffers.Count > 10:
                        market.LoanOffers.RemoveAt(0);
                        break;
                }
        }
    }

    private static void BotLogic(ICapitalist bot, Market market)
    {
        for (var t = 0; t < bot.LoanOffers.Count; t++) {
            market.Balance += bot.LoanOffers[t].Payment;
            bot.Balance -= bot.LoanOffers[t].Payment;
            if (--bot.LoanOffers[t].Year > 1) continue;
            bot.LoanOffers.RemoveAt(t);
            break;
        }
    }
    private static void Refile(Market market, IReadOnlyList<string> keys)
    {
        market.Storage["Wood"] += new Random().Next() % 50;
        market.Storage["Potato"] += new Random().Next() % 25;
        market.Storage["Carrot"] += new Random().Next() % 30;
        market.Storage["Meet"] += new Random().Next() % 15;
        market.Storage["Gold"] += new Random().Next() % 5;
        if (market.Inflation < 1.0) market.Balance += new Random().Next() % 400;
    }
    private static void Destroy(Market market, IReadOnlyList<string> keys) {
        for (var i = 0; i < market.Storage.Count; i++) {
            var count = new Random().Next() % 5;
                if (count >= market.Storage[keys[i]]) continue;
                market.Storage[keys[i]] -= count;
                market.Balance += count * market.Cost[keys[i]];
        }
    }
}