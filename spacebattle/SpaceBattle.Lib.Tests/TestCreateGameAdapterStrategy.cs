namespace SpaceBattle.Lib.Tests;
using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCreateGameAdapterStrategy
{
    public TestCreateGameAdapterStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreatingGameAdapter()
    {
        var mockGameObject = new Mock<IUObject>();
        var adapterInterface = typeof(Type);
        var testAssembly = Assembly.Load("SpaceBattle.Lib.Tests");

        var adapterAssemblyMap = new Dictionary<KeyValuePair<Type, Type>, Assembly>();
        adapterAssemblyMap[new KeyValuePair<Type, Type>(mockGameObject.Object.GetType(), adapterInterface)] = testAssembly;

        var mockCompileCommand = new Mock<Lib.ICommand>();
        mockCompileCommand.Setup(x => x.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Assembly.Map", (object[] args) => adapterAssemblyMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Compile", (object[] args) => mockCompileCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Find", (object[] args) => (object)0).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Create", (object[] args) => new CreateGameAdapterStrategy().Invoke(args)).Execute();

        Assert.NotEmpty(adapterAssemblyMap);
        mockCompileCommand.Verify(x => x.Execute(), Times.Never());

        var adapter = IoC.Resolve<object>("Adapter.Create", mockGameObject.Object, adapterInterface);

        Assert.Equal(0, adapter);
        mockCompileCommand.Verify(x => x.Execute(), Times.Never());
    }

    [Fact]
    public void SuccessfulCreatingGameAdapterWithCompilation()
    {
        var mockGameObject = new Mock<IUObject>();
        var adapterInterface = typeof(Type);
        var testAssembly = Assembly.Load("SpaceBattle.Lib.Tests");

        var adapterAssemblyMap = new Dictionary<KeyValuePair<Type, Type>, Assembly>();

        var mockCompileCommand = new Mock<Lib.ICommand>();
        mockCompileCommand.Setup(x => x.Execute()).Callback(
            () => adapterAssemblyMap[new KeyValuePair<Type, Type>(mockGameObject.Object.GetType(), adapterInterface)] = testAssembly
        ).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Assembly.Map", (object[] args) => adapterAssemblyMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Compile", (object[] args) => mockCompileCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Find", (object[] args) => (object)1).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Create", (object[] args) => new CreateGameAdapterStrategy().Invoke(args)).Execute();

        Assert.Empty(adapterAssemblyMap);
        mockCompileCommand.Verify(x => x.Execute(), Times.Never());

        var adapter = IoC.Resolve<object>("Adapter.Create", mockGameObject.Object, adapterInterface);

        Assert.Single(adapterAssemblyMap);
        Assert.Equal(1, adapter);
        mockCompileCommand.Verify(x => x.Execute(), Times.Once());
    }
}
