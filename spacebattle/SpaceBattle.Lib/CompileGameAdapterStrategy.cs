namespace SpaceBattle.Lib
{
    public class CompileGameAdapterStrategy : IStrategy
    {
        public object Invoke(params object[] args)
        {
            var sourceType = (Type)args[0];
            var destinationType = (Type)args[1];

            return new CompileGameAdapterCommand(sourceType, destinationType);
        }
    }
}

