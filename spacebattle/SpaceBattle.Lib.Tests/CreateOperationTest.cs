using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class CreateOperationTest
{

    public CreateOperationTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void SuccessfulRegisteringCreatingOperatinonTest()
    {
        var ruleList = new List<string>(){
            "Rotate"
        };

        var rotateCommand = new Mock<Lib.ICommand>();
        rotateCommand.Setup(c => c.Execute()).Verifiable();

        var obj = new Mock<IUObject>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Rules.Get.Rotate", (object[] args) => ruleList).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Rotate", (object[] args) => rotateCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Macro.Create", (object[] args) => rotateCommand.Object).Execute();

        var initialization = new InicializationGameCommand(obj.Object, "Rotate");
        initialization.Execute();

        var result = IoC.Resolve<Lib.ICommand>("Operations.Create", obj.Object, "Rotate");
        result.Execute();

        rotateCommand.VerifyAll();
    }

}