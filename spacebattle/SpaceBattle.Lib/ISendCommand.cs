namespace SpaceBattle.Lib;

public class ISendCommand : ICommand
{
    private readonly ISender sender;
    private readonly ICommand icommand;
    public ISendCommand(ISender _sender, ICommand _icommand)
    {
        sender = _sender;
        icommand = _icommand;
    }
    public void Execute()
    {
        sender.Send(icommand);
    }
}
