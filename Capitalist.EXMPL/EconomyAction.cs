namespace Capitalist.EXMPL;

public class EconomyAction
{
    public DateTime _dateTime = DateTime.Now;
    public void Step(Market market, List<string> keys, int count) {
        for (var i = 0; i < count; i++) {
            var dateTime = _dateTime.AddDays(1);
            _dateTime = dateTime;
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
    private void Refile(Market market, IReadOnlyList<string> keys)
    {
        for (var i = 0; i < market.Storage.Count; i++) {
            market.Storage[keys[i]] += new Random().Next() % 50;
        }
        market.Balance += new Random().Next() % 1000;
    }
}