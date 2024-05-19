namespace SpaceBattle.Lib
{
    public class ExceptionHandler : IHandler
    {
        private readonly Exception _exception;
        public ExceptionHandler(Exception exception)
        {
            _exception = exception;
        }
        public void Handle()
        {
            throw _exception;
        }
    }
}