
using Hwdtech;

namespace SpaceBattle.Lib
{
    public class RemoveGameUObjectCommand : ICommand
    {
        private int _uObjectId;

        public RemoveGameUObjectCommand(int uObjectId) => _uObjectId = uObjectId;

        public void Execute()
        {
            IoC.Resolve<IDictionary<int, IUObject>>("UObject.Map").Remove(_uObjectId);
        }
    }
}
