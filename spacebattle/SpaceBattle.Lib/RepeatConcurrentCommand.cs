
using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RepeatConcurrentCommand : ICommand
    {
        private ICommand _command;

        public RepeatConcurrentCommand(ICommand command) => _command = command;

        public void Execute() => IoC.Resolve<BlockingCollection<ICommand>>("Server.Thread.Queue").Add(_command);
    }
}
