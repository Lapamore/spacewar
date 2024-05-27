namespace SpaceBattle.Lib.Tests;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Hwdtech;
using Hwdtech.Ioc;

public class TestCompileStrategy
{
    public TestCompileStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCompiling()
    {
        Assembly? testAssembly = null;
        var references = new List<MetadataReference> {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(Assembly.Load("SpaceBattle.Lib").Location)
        };
        var adapterCode = @"namespace SpaceBattle.Lib;
public class TestFoo {
    public TestFoo() {}
}
";

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Assembly.Name.Create", (object[] args) => Guid.NewGuid().ToString()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile.References", (object[] args) => references).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Compile", (object[] args) => new CompileStrategy().Invoke(args)).Execute();

        try
        {
            testAssembly = IoC.Resolve<Assembly>("Compile", adapterCode);
        }
        catch (Exception e) { Assert.Fail(e.Message); }

        var foo = Activator.CreateInstance(testAssembly.GetType("SpaceBattle.Lib.TestFoo")!)!;

        Assert.Equal("SpaceBattle.Lib.TestFoo", foo.GetType().ToString());
    }
}
