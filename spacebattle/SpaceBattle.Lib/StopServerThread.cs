namespace SpaceBattle.Lib;

public class StopServerThread : ICommand
{
    private readonly ServerThread stopthread;
    public StopServerThread(ServerThread _stopthread)
    {
        stopthread = _stopthread;
    }
    public void Execute()
    {
        if (stopthread.Equals(Thread.CurrentThread))
        {
            stopthread.Stop();
        }
        else
        {
            throw new Exception("That's wrong");
        }
    }
}
