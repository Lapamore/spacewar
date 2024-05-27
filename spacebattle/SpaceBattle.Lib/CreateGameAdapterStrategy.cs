
using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CreateGameAdapterStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var gameObject = (IUObject)args[0];
            var targetInterface = (Type)args[1];

            var assemblyCache = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Adapter.Assembly.Map");
            var typePair = new KeyValuePair<Type, Type>(gameObject.GetType(), targetInterface);

            if (!assemblyCache.TryGetValue(typePair, out var adapterAssembly))
            {
                IoC.Resolve<ICommand>("Adapter.Compile", gameObject.GetType(), targetInterface).Execute();
            }

            return IoC.Resolve<object>("Adapter.Find", gameObject, targetInterface);
        }
    }

}
