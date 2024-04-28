using Hwdtech;

namespace SpaceBattle.Lib
{
    public class ActionDispatcher : ICommand
    {
        IGameCommandMessage msg;

        public ActionDispatcher(IGameCommandMessage message) => msg = message;

        public void Execute()
        {
            var cmd = IoC.Resolve<ICommand>("Command.Creator", msg);

            IoC.Resolve<ICommand>("Queue.Updater", msg.GameID, cmd).Execute();
        }
    }

}
