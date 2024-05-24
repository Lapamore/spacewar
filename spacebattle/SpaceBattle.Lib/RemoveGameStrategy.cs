namespace SpaceBattle.Lib
{
    public class RemoveGameStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            string gameId = (string)args[0];

            return new RemoveGameCommand(gameId);
        }
    }
}

