
using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class FindGameAdapterStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var gameObject = (IUObject)args[0];
            var adapterInterface = (Type)args[1];

            var assemblyDictionary = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Adapter.Assembly.Map");
            var typeKey = new KeyValuePair<Type, Type>(gameObject.GetType(), adapterInterface);

            var adapterAssembly = assemblyDictionary[typeKey];
            var adapterType = adapterAssembly.GetType(IoC.Resolve<string>("Adapter.Name.Create", adapterInterface))!;

            return Activator.CreateInstance(adapterType, gameObject)!;
        }
    }
}

