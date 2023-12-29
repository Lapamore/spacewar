namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
public class StartMovementTest
{
    public StartMovementTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        var queueMock = new Mock<IQueue>();
        var mockCommand = new Mock<SpaceBattle.Lib.ICommand>();
        var InjectCommand = new Mock<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        mockCommand.Setup(x => x.Execute());

        IoC.Resolve<ICommand>("IoC.Register", "Object.SetProperty", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Commands.MoveCommand", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Queue.Push", (object[] args) => queueMock.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Commands.Inject", (object[] args) => InjectCommand.Object).Execute();
    }

    [Fact]
    public void PositiveStartMoveCommand()
    {
        var mockUObject = new Mock<IUObject>();
        var mockQueue = new Mock<IQueue>();
        var mockStartable = new Mock<IMoveCommandStartable>();

        mockStartable.SetupGet(x => x.UObject).Returns(mockUObject.Object).Verifiable();
        mockStartable.SetupGet(x => x.Parameters).Returns(
            new Dictionary<string, object> { { "Velocity", new Vector(2, 1) } }
        ).Verifiable();

        var StartMoveCommand = new StartMoveCommand(mockStartable.Object);
        StartMoveCommand.Execute();
        mockStartable.VerifyAll();
    }

    [Fact]
    public void StartMoveCommandNoneUObject()
    {
        var mockStartable = new Mock<IMoveCommandStartable>();
        mockStartable.SetupGet(x => x.UObject).Throws(new Exception()).Verifiable();
        mockStartable.SetupGet(x => x.Parameters).Returns(
            new Dictionary<string, object> { { "Velocity", new Vector(1, 1) } }
        ).Verifiable();

        var StartMoveCommand = new StartMoveCommand(mockStartable.Object);

        Assert.Throws<Exception>(() => StartMoveCommand.Execute());
        mockStartable.VerifyAll();
    }

    [Fact]
    public void StartMoveCommandNoneParameters()
    {
        var mockUObject = new Mock<IUObject>();

        var mockStartable = new Mock<IMoveCommandStartable>();
        mockStartable.SetupGet(x => x.UObject).Returns(mockUObject.Object);
        mockStartable.SetupGet(x => x.Parameters).Throws(new Exception());

        var StartMoveCommand = new StartMoveCommand(mockStartable.Object);

        Assert.Throws<Exception>(() => StartMoveCommand.Execute());
        mockStartable.VerifyGet(x => x.UObject, Times.Never());
        mockStartable.VerifyGet(x => x.Parameters, Times.Once());
    }
}
