namespace SpaceBattle.Lib;

public class SendCommand : IStrategy
{
    public object Invoke(params object[] args)
    {
        var sendcommand = new ISendCommand((ISender)args[0], (ICommand)args[1]);
        return sendcommand;
    }
}
