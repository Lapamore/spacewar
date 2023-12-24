namespace SpaceBattle.Lib;

public interface IMoveCommandStartable
{
    public IUObject UObject { get; }
    public IDictionary<string, object> Parameters { get; }
}
