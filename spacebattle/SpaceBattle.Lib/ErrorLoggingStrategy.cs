namespace SpaceBattle.Lib;

public class ErrorLoggingStrategy : IStrategy
{
    public object Invoke(object[] args)
    {
        return $"При выполнении команды <{((ICommand)args[0]).GetType()}> возникло исключение <{((Exception)args[1]).GetType()}>";
    }
}
