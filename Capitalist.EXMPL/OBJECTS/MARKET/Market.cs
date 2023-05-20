using Capitalist.EXMPL.BANK.LOAN;

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

namespace Capitalist.EXMPL.OBJECTS.MARKET;

public class Market : ICapitalist {
    public double Balance { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public double Inflation { get; private set; }
    
    public Market() {
        Inventory = new Dictionary<string, int>();
        
        YearCost = new List<List<double>>();
        Balance  = 10000;
        Storage  = new Dictionary<string, int>() {
            {"Wood", 2125},
            {"Potato", 3875},
            {"Carrot", 980},
            {"Meat", 459},
            {"Gold", 73},
        };
        
        Inflation   = 0;
        Cost        = new Dictionary<string, double>();
        LoanOffers  = new List<LoanOffer>();
        MyLoans     = new List<long>();

        MarketNetwork = new Network(new List<ILayer> {
            new FlattenLayer(),
            new PerceptronLayer(7, 25, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(25, 50, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(50, 25, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(25, 10, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(10, 6, new HeInitialization(), new AdamPerceptronOptimization()),
            new ActivationLayer(new DoubleLeakyReLu()),
            new PerceptronLayer(6)
        });
        
        GenerateCost();
    }
    
    private Network MarketNetwork { get; }
    
    public void Buy(string product, Dictionary<string, float> cost) {
        if (Balance >= cost[product]) Balance -= cost[product];
        Storage[product] += 10;
    }
    
    public void Sell(string product, Dictionary<string, double> cost) {
        if (Storage[product] >= 10) Balance += cost[product];
        Storage[product] -= 10;
    }
    
    public List<LoanOffer> LoanOffers { get; set; }
    
    public void GenerateCost() {
        Cost = new Dictionary<string, double>() {
            {"Wood", Balance / Storage["Wood"]},
            {"Potato", Balance / Storage["Potato"]},
            {"Carrot", Balance / Storage["Carrot"]},
            {"Meat", Balance / Storage["Meat"]},
            {"Gold", Balance / Storage["Gold"]},
        };
        
        if (YearCost.Count < 100) YearCost.Add(new List<double>() {Cost["Wood"], Cost["Potato"], Cost["Carrot"], Cost["Meat"], Cost["Gold"]});
        else YearCost.Clear();
        
        Inflation = Balance / 10000.0 -
                    (Storage["Wood"] + Storage["Potato"] + Storage["Carrot"] + Storage["Meat"] + Storage["Gold"]) /
                    7512.0;
    }
    
    private List<List<double>> YearCost { get; set; }
    public Dictionary<string, int> Storage { get; set; }
    public Dictionary<string, double> Cost { get; private set; }
    public List<long> MyLoans { get; set; }

    private double _previousBudget;
    private double _previousInflation;
    private int _previousAnswer;
    
    public void MarketTurn() {
        MarketNetwork.ForwardFeed(new Tensor((7, 1, 1)));
        
        _previousAnswer = (int)MarketNetwork.ForwardFeed(new Tensor(new Matrix(new[] {
            Inflation,
            Balance,
            Cost["Wood"],
            Cost["Potato"],
            Cost["Carrot"],
            Cost["Meat"],
            Cost["Gold"],
        })), AnswerType.Class);

        switch (_previousAnswer) {
            case 0:
                AddWood();
                break;
            case 1:
                AddPotato();
                break;
            case 2:
                AddCarrot();
                break;
            case 3:
                AddMeet();
                break;
            case 4:
                AddGold();
                break;
            case 5:
                PrintMoneys();
                break;
        }
        
        if (_previousBudget >= Balance || _previousInflation >= Inflation) 
            MarketNetwork.BackPropagation(_previousAnswer, 0, new Mae(), .015d, true);
        
        else 
            MarketNetwork.BackPropagation(_previousAnswer, 1, new Mae(), .015d, true);

        GenerateCost();

        _previousBudget = Balance;
        _previousInflation = Inflation;
    }
    
    private void AddWood() => Storage["Wood"] += new Random().Next() % 50;
    private void AddPotato() => Storage["Potato"] += new Random().Next() % 25;
    private void AddCarrot() => Storage["Carrot"] += new Random().Next() % 30;
    private void AddMeet() => Storage["Meat"] += new Random().Next() % 15;
    private void AddGold() => Storage["Gold"] += new Random().Next() % 5;
    private void PrintMoneys() => Balance += 100d;
}