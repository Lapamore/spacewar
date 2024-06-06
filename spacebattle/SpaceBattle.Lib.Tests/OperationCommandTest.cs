using Hwdtech;
using Moq;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class OperationCommandTests
{

    public OperationCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void SuccessfulCreateShootCommandTest()
    {
        var obj = new Mock<Lib.IUObject>();
        var shootable = new Mock<Lib.IShootable>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter", (object[] args) => shootable.Object).Execute();
        var createShootCommand = new CreateShootCommand();
        var cmd = createShootCommand.Invoke(obj.Object);

        Assert.NotNull(cmd);
        Assert.True(cmd.GetType() == typeof(ShootCommand));
    }


    [Fact]
    public void SuccessfulCreateTurnCommandTest()
    {
        var obj = new Mock<Lib.IUObject>();
        var rotatable = new Mock<Lib.ITurn>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter", (object[] args) => rotatable.Object).Execute();
        var createTurnCommand = new CreateTurnCommand();
        var cmd = createTurnCommand.Invoke(obj.Object);

        Assert.NotNull(cmd);
        Assert.True(cmd.GetType() == typeof(TurnCommand));
    }

    [Fact]
    public void SuccessfulCreateStartMoveCommandTest()
    {
        var obj = new Mock<Lib.IUObject>();
        var moveable = new Mock<Lib.IMoveCommandStartable>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter", (object[] args) => moveable.Object).Execute();
        var createStartMoveCommand = new CreateStartMoveCommand();
        var cmd = createStartMoveCommand.Invoke(obj.Object);

        Assert.NotNull(cmd);
        Assert.True(cmd.GetType() == typeof(StartMoveCommand));
    }

    [Fact]
    public void SuccessfulCreateEndMoveCommandTest()
    {
        var obj = new Mock<Lib.IUObject>();
        var endable = new Mock<Lib.IMoveCommandEndable>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter", (object[] args) => endable.Object).Execute();
        var createEndMoveCommand = new CreateEndMoveCommand();
        var cmd = createEndMoveCommand.Invoke(obj.Object);

        Assert.NotNull(cmd);
        Assert.True(cmd.GetType() == typeof(EndMoveCommand));
    }
}
