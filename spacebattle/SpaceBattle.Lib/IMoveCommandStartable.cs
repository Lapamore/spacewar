namespace SpaceBattle.Lib
{
    public interface IMoveCommandStartable
    {
        IUObject target { get; }
        IDictionary<string, object> property { get; }
    }
}