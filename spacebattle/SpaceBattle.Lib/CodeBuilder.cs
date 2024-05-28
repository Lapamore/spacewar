namespace SpaceBattle.Lib;
using Hwdtech;
using Scriban;

public class CodeBuilder : IBuilder
{

    private string _class;
    private  List<object> _wh;

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
        var templateText = IoC.Resolve<string>("TextTemplate");
        var template = Template.Parse(templateText);
        var result = template.Render(new {name = _class, properties = _wh.ToArray()});
        return result;
    }
}
