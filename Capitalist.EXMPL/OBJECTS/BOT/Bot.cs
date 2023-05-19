using Capitalist.EXMPL.BANK.LOAN;
using Capitalist.EXMPL.OBJECTS.FACTORY;
using FotNET.NETWORK;
using FotNET.NETWORK.LAYERS;
using FotNET.NETWORK.LAYERS.ACTIVATION;
using FotNET.NETWORK.LAYERS.ACTIVATION.ACTIVATION_FUNCTION.DOUBLE_LEAKY_RELU;
using FotNET.NETWORK.LAYERS.PERCEPTRON;
using FotNET.NETWORK.LAYERS.PERCEPTRON.ADAM.ADAM_PERCEPTRON;
using FotNET.NETWORK.MATH.Initialization.HE;
using FotNET.NETWORK.MATH.LOSS_FUNCTION.MAE;
using FotNET.NETWORK.MATH.OBJECTS;

namespace Capitalist.EXMPL.OBJECTS.BOT;

public class Bot : ICapitalist {
    public Bot() {
        Balance   = 500;
        Inventory = new Dictionary<string, int>() {
            {"Wood", 0},
            {"Potato", 0},
            {"Carrot", 0},
            {"Meet", 0},
            {"Gold", 0},
        };
        
        LoanOffers  = new List<LoanOffer>();  
        Factories   = new List<Factory>();  
        MyLoans     = new List<long>();
        GettedLoans = new List<long>();

        BotNetwork = new Network(new List<ILayer> {
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
        BotNetwork.BackPropagation(_previousAnswer, _previousBudget >= Balance ? 0 : 1, 
            new Mae(), .015d, true);
        
        foreach (var factory in Factories) 
            factory.DoWork();

        _previousAnswer = (int)BotNetwork.ForwardFeed(new Tensor(new Matrix(new[] {
            Balance,
            MyLoans.Count,
            Factories.Count,
            Inventory["Wood"],
            Inventory["Potato"],
            Inventory["Carrot"],
            Inventory["Meet"],
            Inventory["Gold"]
        })), AnswerType.Class);

        switch (_previousAnswer) {
            
        }
        
        _previousBudget = Balance;
    }

    public void OpenWoodFactory() {
        
    }
    
    public void OpenPotatoFactory() {
        
    }
    
    public void OpenCarrotFactory() {
        
    }
    
    public void OpenMeetFactory() {
        
    }
    
    public void OpenGoldFactory() {
        
    }
    
    
    public void Buy(string product, Dictionary<string, float> cost) {
        if (Balance >= cost[product]) Balance -= cost[product];
        Inventory[product] += 10;
    }
    
    public void Sell(string product, Dictionary<string, float> cost) {
        if (Inventory[product] >= 10) Balance += cost[product];
        Inventory[product] -= 10;
    }
    
    public void TakeLoan(LoanOffer loanOffer) {
        MyLoans.Add(loanOffer.Id);
        Balance += loanOffer.Value;
    } 
}