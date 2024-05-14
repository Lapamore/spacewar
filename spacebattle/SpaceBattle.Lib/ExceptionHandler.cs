namespace SpaceBattle.Lib
{
    public class ExceptionHandler : IStrategy
    {
        public object Invoke(params object[] args)
        {
            ICommand cmd = (ICommand)args[0];
            Exception exc = (Exception)args[1];

            throw exc;
        }
    }
}