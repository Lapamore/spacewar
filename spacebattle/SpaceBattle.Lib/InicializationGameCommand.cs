using Hwdtech;

namespace SpaceBattle.Lib;

public class InicializationGameCommand : ICommand
{
    private readonly IUObject _obj;
    private readonly string _type;

    public InicializationGameCommand(IUObject obj, string type)
    {
        _obj = obj;
        _type = type;
    }

    public void Execute()
    {
        var createOperationStrategy = new CreateOperation();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operations.Create", (object[] args) => createOperationStrategy.Invoke(_obj, _type)).Execute();
    }
}