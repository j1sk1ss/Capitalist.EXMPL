namespace Capitalist.EXMPL;

internal class CapitalistGame
{
    private readonly Player _player = new Player();
    private List<Bot> _bots = new List<Bot>();
    private Market _market = new Market();
    public DateTime DateTime = DateTime.Now.Date;
    private static void Main()
    {
        var game = new CapitalistGame();
        var action = new EconomyAction();
        game.Menu();
    }

    private void Menu()
    {
        Console.Clear();
        Console.WriteLine($"This is ur main game page \nDate is: {DateTime.ToString("MM/dd/yyyy")!}" +
                          " \nList of enabled actions: \n" +
                          "<==================================================> \n" +
                          "1) Show balance window. \n2) Show global market. \n" +
                          "3) Trade banknotes. \n4) Inventory window. \n" +
                          "5) Show businesses list. \n6) Show workers market. \n" +
                          "7) Next step (one year)");
        switch (int.Parse(Console.ReadLine()!)) {
            case 1: {
                Console.Clear();
                BalanceWindow();
            } break;
            case 2: {
                Console.Clear();
                GlobalMarketWindow();
            } break;
        }
        Menu();
    }

    private void BalanceWindow() {
        Console.Clear();
        Console.WriteLine($"Ur balance is: {_player.Balance}\n");
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
    private void TakeLoanWindow() {
        Console.Clear();
        Console.WriteLine($"Ur balance is: {_player.Balance}\n");
        Console.WriteLine("\nList of enabled loans: \n" +
                          "<==================================================>");
        for (var t = 0; t < _market.LoanOffers.Count; t++)
            Console.WriteLine($"<====>\n{t})   "+_market.LoanOffers[t].Information()+"\n<====>");
        Console.WriteLine("Choose the number of loan or 999 to exit: ");
        var i = int.Parse(Console.ReadLine()!);
        if (i == 999) return;
        var loan = _market.LoanOffers[i];
        if (_player.GettedLoans.Contains(loan.id)) {
            _player.Balance += loan.Value;
            _market.LoanOffers.RemoveAt(i);
            _player.GettedLoans.RemoveAt(_player.GettedLoans.IndexOf(loan.id));
        }
        else {
            _player.Balance += loan.Value;
            _player.LoanOffers.Add(loan);
        }
    }
    private void CreateLoanOffer() {
        Console.Clear();
        Console.WriteLine($"Ur balance is: {_player.Balance}");
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
        Console.WriteLine("To buy enter num and 1, to sell 2");
        Console.WriteLine("Wood_B" + " " + "Wood_B"[^1] + " " + "Wood_B"[..^2]);
        ChangeProducts(int.Parse(Console.ReadLine()!) switch {
            01 => "Wood_B",
            11 => "Potato_B",
            21 => "Carrot_B",
            31 => "Meet_B",
            41 => "Gold_B",
            02 => "Wood_S",
            12 => "Potato_S",
            22 => "Carrot_S",
            32 => "Meet_S",
            42 => "Gold_S",
            _ => "EXIT" });
    }
        private void ChangeProducts(string name) {
            if (name == "EXIT") return;
            switch (name[^1])
            {
                case('B'): 
                    if (!(_player.Balance >= _market.Cost[name[..^2]])) return;
                    _player.Balance -= _market.Cost[name[..^2]];
                    _market.Balance += _market.Cost[name[..^2]];
                    _player.Inventory[name[..^2]] += 10;
                    _market.Storage[name[..^2]] -= 10;
                    break;
                case ('S'): 
                    if (!(_market.Balance >= _market.Cost[name[..^2]])) return;
                    _player.Balance += _market.Cost[name[..^2]];
                    _market.Balance -= _market.Cost[name[..^2]];
                    _player.Inventory[name[..^2]] -= 10;
                    _market.Storage[name[..^2]] += 10;
                    break;
            }
        }
}