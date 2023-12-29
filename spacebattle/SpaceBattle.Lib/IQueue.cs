namespace SpaceBattle.Lib;

public interface IQueue
{
    void Push(ICommand cmd);
    ICommand Pop();
}
