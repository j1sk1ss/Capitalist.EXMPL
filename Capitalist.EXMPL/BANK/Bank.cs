using Capitalist.EXMPL.BANK.LOAN;
using Capitalist.EXMPL.OBJECTS;
using Capitalist.EXMPL.OBJECTS.MARKET;

using FotNET.NETWORK;
using FotNET.NETWORK.LAYERS;
using FotNET.NETWORK.LAYERS.ACTIVATION;
using FotNET.NETWORK.LAYERS.ACTIVATION.ACTIVATION_FUNCTION.DOUBLE_LEAKY_RELU;
using FotNET.NETWORK.LAYERS.FLATTEN;
using FotNET.NETWORK.LAYERS.PERCEPTRON;
using FotNET.NETWORK.LAYERS.PERCEPTRON.ADAM.ADAM_PERCEPTRON;
using FotNET.NETWORK.MATH.Initialization.HE;
using FotNET.NETWORK.MATH.LOSS_FUNCTION.MAE;
using FotNET.NETWORK.MATH.OBJECTS;

namespace Capitalist.EXMPL.BANK;

public class Bank {
    public Bank(double startBudget, double startTax, double startVat, 
        double loans, Market market) {
        Budget     = startBudget;
        TaxPercent = startTax;
        Vat        = startVat;

        AwaliableLoan = loans;
        Market        = market;

        Loans      = new List<LoanOffer>();
        LoanOffers = new List<LoanOffer>();
        
        BankNetwork = new Network(new List<ILayer> {
            new FlattenLayer(),
            new PerceptronLayer(5, 25, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(25, 50, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(50, 25, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(25, 10, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(10, 7, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(7)
        });
    }
    
    public double Budget { get; set; }
    private double TaxPercent { get; set; }
    private double Vat { get; set; }
    private double AwaliableLoan { get; set; }
    private Market Market { get; set; }
    private Network BankNetwork { get; }

    private List<LoanOffer> Loans { get; }
    public List<LoanOffer> LoanOffers { get; }

    private void IncreaseVat() => Vat += .01d;
    private void DecreaseVat() => Vat -= .01d;
    private void PrintMoneys() => Budget += 100d;
    private void IncreaseTax() => TaxPercent += .01d;
    private void DecreaseTax() => TaxPercent -= .01d;
    private void IncreaseLoans() => AwaliableLoan += 100d;
    private void DecreaseLoans() => AwaliableLoan -= 100d;

    private double _previousGpd;
    private int _previousAnswer;
    
    public void BankTurn() {
        BankNetwork.ForwardFeed(new Tensor(5, 1, 1));
        
        GetPercent();
        CreateLoans();
        
        BankNetwork.BackPropagation(_previousAnswer, _previousGpd >= _gpd ? 0 : 1, 
            new Mae(), .015d, true);

        _previousAnswer = (int)BankNetwork.ForwardFeed(new Tensor(new Matrix(new[] {
            Budget,
            TaxPercent,
            Vat,
            AwaliableLoan,
            Market.Inflation
        })), AnswerType.Class);

        switch (_previousAnswer) {
            case 0:
                IncreaseVat();
                break;
            case 1:
                DecreaseVat();
                break;
            case 2:
                PrintMoneys();
                break;
            case 3:
                IncreaseTax();
                break;
            case 4:
                DecreaseTax();
                break;
            case 5:
                IncreaseLoans();
                break;
            case 6:
                DecreaseLoans();
                break;
        }
        
        _previousGpd = _gpd;
        _gpd = 0;
    }

    private void CreateLoans() {
        var random = new Random().Next() % AwaliableLoan;
        var id = new Random().Next();
        
        LoanOffers.Add(new LoanOffer(random, 1 + new Random().Next() % 640, 
            0.1 + (new Random().Next() % 10) / 10.0, id, null!));
        Budget -= random;
    }
    
    private double _gpd;
    
    public bool Transaction(ICapitalist firstSubject, ICapitalist secondSubject, double value) {
        if (firstSubject.Balance < value + value * Vat) return false;
        
        firstSubject.Balance  -= value + value * Vat;
        secondSubject.Balance += value;

        _gpd += 2 * value + value * Vat;
        
        return true;
    }
    
    private void GetPercent() {
        foreach (var loan in Loans.Where(loan => loan.Owner != null)) {
            Budget += loan.Payment;
            loan.Owner.Balance -= loan.Payment;

            _gpd += loan.Payment;
        }
    }
    
    private LoanOffer CreateLoan((double value, int years) loan, ICapitalist owner) {
        if (AwaliableLoan < loan.value) return null!;
        
        var percent = loan.years / loan.value;

        return new LoanOffer(loan.value, loan.years, percent, new Random().Next(), owner);
    }

    public void GetLoan(ICapitalist subject, (double value, int years) loan) {
        if (subject.LoanOffers.Count > 0) return;

        var createdLoan = CreateLoan(loan, subject);
        if (createdLoan == null) return;
        
        subject.MyLoans.Add(createdLoan.Id);
        Loans.Add(createdLoan);

        subject.Balance += createdLoan.Value;
    }
    
    public void GetLoan(ICapitalist subject, LoanOffer loan) {
        if (subject.LoanOffers.Count > 0) return;
        
        subject.MyLoans.Add(loan.Id);
        Loans.Add(loan);

        subject.Balance += loan.Value;
    }
}