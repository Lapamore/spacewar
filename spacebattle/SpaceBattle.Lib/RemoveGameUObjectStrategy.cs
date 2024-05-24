namespace SpaceBattle.Lib
{
    public class RemoveGameUObjectStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            int uObjectId = (int)args[0];

            return new RemoveGameUObjectCommand(uObjectId);
        }
    }
}

