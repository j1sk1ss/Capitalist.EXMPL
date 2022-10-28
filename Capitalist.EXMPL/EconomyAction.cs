namespace Capitalist.EXMPL;

public class EconomyAction
{
    private readonly CapitalistGame _capitalist = new();
    private Market _market = new();
    public void Step() {
        var dateTime = _capitalist.DateTime.AddDays(1);
        _capitalist.DateTime = dateTime;
        
        
    }
}