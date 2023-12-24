namespace SpaceBattle.Lib.Tests;

public class EndMoveCommandExecutionTest
{
    public EndMoveCommandExecutionTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void EndMoveTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var moveCommandEndable = new Mock<IMoveCommandEndable>();
        var commandInput = new Mock<ICommand>().Object;
        var commandBridgeInput = new BridgeCommand(commandInput);
        var targetInput = new Mock<IUObject>();
        var propertyInput = new List<string>();

        moveCommandEndable.Setup(m => m.property).Returns(propertyInput);
        moveCommandEndable.Setup(m => m.target).Returns(targetInput.Object);
        moveCommandEndable.Setup(m => m.command).Returns(commandBridgeInput);

        var endCommand = new EndMoveCommand(moveCommandEndable.Object);
        var deletePropertyCommand = new Mock<ICommand>().Object;

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Target.DeleteProperty",
            (object[] args) =>
            {
                return deletePropertyCommand;
            }
        ).Execute();

        endCommand.Execute();

        Assert.NotEqual(commandInput, moveCommandEndable.Object.command.GetCommand());
    }
}

public class BridgeCommandExecutionTest
{
    [Fact]
    public void BridgeCommandTest()
    {
        var emptyCommandTest = new EmptyCommand();
        var internalCommand = new Mock<ICommand>();
        internalCommand.Setup(i => i.Execute());
        var bridge = new BridgeCommand(internalCommand.Object);
        bridge.Inject(emptyCommandTest);
        bridge.Execute();
        internalCommand.Verify(i => i.Execute(), Times.Never());
    }
}
