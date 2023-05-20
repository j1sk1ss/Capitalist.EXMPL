using Capitalist.EXMPL.BANK.LOAN;
using Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS;
using Capitalist.EXMPL.OBJECTS.FACTORY.OBJECTS.FACTORY_TYPES;
using Capitalist.EXMPL.UI;
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

namespace Capitalist.EXMPL.OBJECTS.BOT;

public class Bot : ICapitalist {
    public Bot(double balance) {
        Balance   = balance;
        Inventory = new Dictionary<string, int>() {
            {"Wood", 0},
            {"Potato", 0},
            {"Carrot", 0},
            {"Meat", 0},
            {"Gold", 0},
        };
        
        LoanOffers  = new List<LoanOffer>();  
        Factories   = new List<Factory>();  
        MyLoans     = new List<long>();
        GettedLoans = new List<long>();

        BotNetwork = new Network(new List<ILayer> {
            new FlattenLayer(),
            new PerceptronLayer(8, 25, new HeInitialization(), new AdamPerceptronOptimization()),
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
    }
    
    private Network BotNetwork { get; }
    
    public double Balance { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public List<Factory> Factories { get; set; }
    public List<LoanOffer> LoanOffers { get; set; }
    public List<long> GettedLoans { get; set; }
    public List<long> MyLoans { get; set; }

    private double _previousBudget;
    private int _previousAnswer;
    
    public void BotTurn() {
        BotNetwork.ForwardFeed(new Tensor((8, 1, 1)));
        
        BotNetwork.BackPropagation(_previousAnswer, _previousBudget >= Balance ? 0 : 1, 
            new Mae(), .015d, true);

        foreach (var factory in Factories) {
            factory.DoWork();   
            factory.FactoryTurn();
        }

        foreach (var key in Inventory.Keys)
            Sell(key, CapitalistGame.Market.Cost);   
        
        _previousAnswer = (int)BotNetwork.ForwardFeed(new Tensor(new Matrix(new[] {
            Balance,
            MyLoans.Count,
            Factories.Count,
            Inventory["Wood"],
            Inventory["Potato"],
            Inventory["Carrot"],
            Inventory["Meat"],
            Inventory["Gold"]
        })), AnswerType.Class);

        switch (_previousAnswer) {
            case 0:
                OpenWoodFactory();
                break;
            case 1:
                OpenPotatoFactory();
                break;
            case 2:
                OpenCarrotFactory();
                break;
            case 3:
                OpenMeatFactory();
                break;
            case 4:
                OpenGoldFactory();
                break;
            case 5:
                OpenLoan();
                break;
        }
        
        _previousBudget = Balance;
    }

    private void OpenWoodFactory() {
        if (WoodFactory.Cost > Balance) return;
        Balance -= WoodFactory.Cost;
        
        Factories.Add(new WoodFactory(this));
    }
    
    private void OpenPotatoFactory() {
        if (PotatoFactory.Cost > Balance) return;
        Balance -= PotatoFactory.Cost;
        
        Factories.Add(new PotatoFactory(this));
    }
    
    private void OpenCarrotFactory() {
        if (CarrotFactory.Cost > Balance) return;
        Balance -= CarrotFactory.Cost;
        
        Factories.Add(new CarrotFactory(this));
    }
    
    private void OpenMeatFactory() {
        if (MeatFactory.Cost > Balance) return;
        Balance -= MeatFactory.Cost;
        
        Factories.Add(new MeatFactory(this));
    }
    
    private void OpenGoldFactory() {
        if (GoldenFactory.Cost > Balance) return;
        Balance -= GoldenFactory.Cost;
        
        Factories.Add(new GoldenFactory(this));
    }

    private void OpenLoan() {
        CapitalistGame.Bank.GetLoan(this, (new Random().Next() % 1000, 100));
    }
    
    public void Buy(string product, Dictionary<string, float> cost) {
        if (Balance >= cost[product]) Balance -= cost[product];
        Inventory[product] += 10;
    }
    
    public void Sell(string product, Dictionary<string, double> cost) {
        if (Inventory[product] >= 10) Balance += cost[product];
        Inventory[product] -= 10;
    }
}