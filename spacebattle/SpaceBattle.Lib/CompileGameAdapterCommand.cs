using System.Reflection;
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CompileGameAdapterCommand : ICommand
    {
        private Type sourceType;
        private Type destinationType;

        public CompileGameAdapterCommand(Type objectType, Type targetType)
        {
            sourceType = objectType;
            destinationType = targetType;
        }

        public void Execute()
        {
            var adapterCode = IoC.Resolve<string>("Adapter.Code", sourceType, destinationType);
            var compiledAssembly = IoC.Resolve<Assembly>("Compile", adapterCode);

            var assemblyMap = IoC.Resolve<IDictionary<KeyValuePair<Type, Type>, Assembly>>("Adapter.Assembly.Map");
            var pair = new KeyValuePair<Type, Type>(sourceType, destinationType);

            assemblyMap[pair] = compiledAssembly;
        }
    }
}
