namespace SpaceBattle.Lib.Tests;
using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCompileGameAdapter
{
    public TestCompileGameAdapter()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulAddingAssemblyAfterGameAdapterCompilation()
    {
        var testAssembly = Assembly.Load("SpaceBattle.Lib.Tests");
        var adapterAssemblyMap = new Dictionary<KeyValuePair<Type, Type>, Assembly>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Code", (object[] args) => ";").Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => testAssembly).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Assembly.Map", (object[] args) => adapterAssemblyMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Compile", (object[] args) => new CompileGameAdapterStrategy().Invoke(args)).Execute();

        Assert.Empty(adapterAssemblyMap);

        IoC.Resolve<Lib.ICommand>("Adapter.Compile", typeof(Type), typeof(Type)).Execute();

        Assert.Single(adapterAssemblyMap);
        Assert.Equal(new KeyValuePair<Type, Type>(typeof(Type), typeof(Type)), adapterAssemblyMap.First().Key);
        Assert.Equal(testAssembly, adapterAssemblyMap.First().Value);
    }
}
