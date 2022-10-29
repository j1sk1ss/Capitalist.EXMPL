namespace Capitalist.EXMPL;

public class EconomyAction
{
    public DateTime DateTime = DateTime.Now;
    public void Step(Market market, List<string> keys, int count, List<Bot> bots, Player player) {
        for (var i = 0; i < count; i++) {
            var dateTime = DateTime.AddDays(1);
            DateTime = dateTime;
            
            for (var j = 0; j < bots.Count; j++)
            for (var c = 0; c < bots[j].LoanOffers.Count; j++) {
                bots[j].Balance -= bots[j].LoanOffers[c].Payment;
                bots[j].LoanOffers[c].Value -= bots[j].LoanOffers[c].Payment;
                if (--bots[j].LoanOffers[c].Year <= 0) break;
            }

            for (var t = 0; t < player.LoanOffers.Count; t++) {
                market.Balance += player.LoanOffers[t].Payment;
                player.Balance -= player.LoanOffers[t].Payment;
                if (--player.LoanOffers[t].Year > 1) continue;
                    player.LoanOffers.RemoveAt(t);
                    break;
            }




            Refile(market, keys);
            market.GenerateCost();
                switch (market.LoanOffers.Count) {
                    case < 10 when market.Inflation > 0:
                        market.CreateLoan();
                        break;
                    case < 10 when market.Inflation > 0:
                        market.LoanOffers.RemoveAt(0);
                        break;
                }
        }
    }
    private static void Refile(Market market, IReadOnlyList<string> keys) {
        for (var i = 0; i < market.Storage.Count; i++) {
            market.Storage[keys[i]] += new Random().Next() % 50;
        }
        market.Balance += new Random().Next() % 400;
    }
}