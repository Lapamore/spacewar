using Hwdtech;

namespace SpaceBattle.Lib;

public class SoftStop : IStrategy
{
    public object RunStrategy(params object[] args)
    {
        var id = args[0];
        var st = IoC.Resolve<ServerThread>("ServerThreadID", id);
        if (args.Length > 1)
        {
            var action1 = (Action)args[1];
            var newaction = new Action(() =>
            {
                if (!st.EmptyQueue())
                {
                    st.ExceptionHandler();
                }
                else
                {
                    new StopServerThread(st).Execute();
                    action1();
                }
            });
            var softstop = new UpdateBehavior(st, newaction);
            return softstop;
        }
        else
        {
            var newaction = new Action(() =>
            {
                if (!st.EmptyQueue())
                {
                    st.ExceptionHandler();
                }
                else
                {
                    new StopServerThread(st).Execute();
                }
            });
            var softstop = new UpdateBehavior(st, newaction);
            return softstop;
        }
    }
}
