using Hwdtech;

namespace SpaceBattle.Lib
{
    public class MessageToCommandConverter : IStrategy
    {
        public object Invoke(params object[] args)
        {
            IGameCommandMessage msg = (IGameCommandMessage)args[0];

            var obj = IoC.Resolve<IUObject>("UObject.Retriever", msg.ObjectID);

            msg.Args.ToList().ForEach(x => obj.setProperty(x.Key, x.Value));

            return IoC.Resolve<ICommand>("cmd." + msg.TypeCommand, obj);
        }
    }
}
