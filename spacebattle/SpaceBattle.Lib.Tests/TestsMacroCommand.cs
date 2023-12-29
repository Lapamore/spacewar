namespace SpaceBattle.Lib.Tests;

public class MacroCommandTest
{
    public MacroCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var currentScopeSetter = IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>(
                "Scopes.New",
                IoC.Resolve<object>("Scopes.Root")
            )
        );
        currentScopeSetter.Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateMacro",
            (object[] args) =>
            {
                var commands = (IEnumerable<ICommand>)args[0];
                return new MacroCommand(commands);
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Strategy.MacroCommand",
            (object[] args) =>
            {
                var nameOperation = (string)args[0];
                var uObject = (IUObject)args[1];
                return new MacroCommandStrategy().Init(nameOperation, uObject);
            }
        ).Execute();
    }

    [Fact]
    public void SuccessfulExampleOfCreatingAndRunningMacroCommand()
    {
        var movementAndRotationOperation = "MovementAndRotationOperation";

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Component." + movementAndRotationOperation,
            (object[] args) =>
                new string[] { "Game.Command.CreateMove", "Game.Command.CreateTurn" }
        ).Execute();

        var mockUObject = new Mock<IUObject>();

        var moveCommand = new Mock<ICommand>();
        moveCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateMove",
            (object[] args) => moveCommand.Object
        ).Execute();

        var turnCommand = new Mock<ICommand>();
        turnCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.CreateTurn",
            (object[] args) => turnCommand.Object
        ).Execute();

        var macroCommand = IoC.Resolve<ICommand>("Game.Strategy.MacroCommand", movementAndRotationOperation, mockUObject.Object);

        macroCommand.Execute();

        moveCommand.Verify(x => x.Execute(), Times.Once);
        turnCommand.Verify(x => x.Execute(), Times.Once);
    }

    [Fact]
    public void TryExecuteCommandsInMacroCommandThrowException()
    {
        var failingCommand = new Mock<ICommand>();
        failingCommand.Setup(x => x.Execute()).Throws(new Exception());

        var succeedingCommand = new Mock<ICommand>();
        succeedingCommand.Setup(x => x.Execute()).Verifiable();

        var commands = new List<ICommand> { failingCommand.Object, succeedingCommand.Object };

        Assert.Throws<Exception>(() => new MacroCommand(commands).Execute());
        succeedingCommand.Verify(x => x.Execute(), Times.Never);
    }
}
