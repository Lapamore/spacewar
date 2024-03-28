namespace SpaceBattle.Lib.Console;

public class Program
{
    public static void Main(string[] args)
    {
        var serverCount = int.Parse(args[0]);
        var programStartCommand = new ProgramStart(serverCount);
        programStartCommand.Execute();
    }
}
