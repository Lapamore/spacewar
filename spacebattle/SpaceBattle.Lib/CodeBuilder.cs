namespace SpaceBattle.Lib;
using Hwdtech;
using Scriban;

public class CodeBuilder : IBuilder
{

    private string _class;
    private List<object> _wh;

    public CodeBuilder(string Class)
    {
        _class = Class;
        _wh = new List<object>();

    }
    public CodeBuilder adder(object property)
    {
        _wh.Add(property);
        return this;
    }

    public string build()
    {
        var templateText = @"using System;
public class {{name }}
{
    private object _obj;
    {{ for property in properties }}
    private {{ property.type }} {{ property.name }}{
    {{if property.set}}
    set
    {
        Hwdtech.IoC.Resolve<SpaceBattle.Lib.ICommand>(""{{property.name}}.Set"", _obj, value).Execute();
    }
    {{end}}  
    get
    {{if property.get}}
    {
        return Hwdtech.IoC.Resolve<{{property.type}}>(""{{property.name}}.Get"", _obj);
    } 
    {{ end }}
    }
    {{ end }}
    public {{ name }}(object obj)
    {
        _obj = obj;
    }
}";
        var template = Template.Parse(templateText);
        var result = template.Render(new { name = _class, properties = _wh.ToArray() });
        return result;
    }
}
