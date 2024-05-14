namespace SpaceBattle.Lib
{
    public class DefaultHandler : IHandler
    {
        private readonly Exception _exception;

        public DefaultHandler(Exception exception)
        {
            _exception = exception;
        }

        public void Handle()
        {
            throw _exception;
        }
    }
}