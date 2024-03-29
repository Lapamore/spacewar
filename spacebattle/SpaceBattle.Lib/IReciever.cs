namespace SpaceBattle.Lib;

public interface IReciever
{
    public ICommand Recieve();
    public bool IsEmpty();
}
