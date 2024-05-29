using Hwdtech;
using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Test;

public class CodeBuilderTests
{
    [Fact]
    public void BuildString()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        Type type = typeof(IMovable);
        var builder = new CodeBuilder(Class: "MovableAdapter");
        type.GetProperties().ToList().ForEach((property) => builder.adder(new
        {
            name = property.Name,
            type = property.PropertyType.Name,
            get = property.CanRead,
            set = property.CanWrite
        }));

        var valid = @"using System;
public class MovableAdapter
{
    private object _obj;
    
        private Vector Position{
        
        set
        {
            Hwdtech.IoC.Resolve<SpaceBattle.Lib.ICommand>(""Position.Set"", _obj, value).Execute();
        }
          
        get
        
        {
            return Hwdtech.IoC.Resolve<Vector>(""Position.Get"", _obj);
        } 
        
        }
        
        private Vector Velocity{
          
        get
        
        {
            return Hwdtech.IoC.Resolve<Vector>(""Velocity.Get"", _obj);
        } 
        
        }
        
    public MovableAdapter(object obj)
    {
        _obj = obj;
    }
}";
        var result = builder.build();
        Assert.Equal(valid, result);
    }
}
