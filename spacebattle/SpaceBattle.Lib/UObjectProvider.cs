using Hwdtech;

namespace SpaceBattle.Lib
{
    public class UObjectProvider : IStrategy
    {
        public object Invoke(params object[] args)
        {
            int objectid = (int)args[0];

            IUObject? uObject;

            if (IoC.Resolve<IDictionary<int, IUObject>>("UObject.Mapping").TryGetValue(objectid, out uObject))
            {
                return uObject;
            }
            throw new Exception();
        }
    }
}
