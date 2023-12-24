using SpaceBattle.Lib;

public class StartCommandTest
{
    public StartCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void Execute_RegistersTargetAndPuchesMovingCommand_WhenCalled()
    {
        var moveCommandStartable = new Mock<IMoveCommandStartable>();
        moveCommandStartable.Setup(m=>m.target).Returns(new Mock<IUObject>().Object);
        moveCommandStartable.Setup(m=>m.property).Returns(new Dictionary<string, object>());
    }
}
