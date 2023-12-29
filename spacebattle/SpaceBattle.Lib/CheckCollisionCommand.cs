using Hwdtech;

namespace SpaceBattle.Lib
{
    public class CheckCollisionCommand : ICommand
    {
        private readonly IUObject _objFirst, _objSecond;

        public CheckCollisionCommand(IUObject objFirst, IUObject objSecond)
        {
            _objFirst = objFirst;
            _objSecond = objSecond;
        }

        public void Execute()
        {
            var positionFirst = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _objFirst, "Position");
            var velocityFirst = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _objFirst, "Velocity");
            var positionSecond = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _objSecond, "Position");
            var velocitySecond = IoC.Resolve<List<int>>("Game.UObject.GetProperty", _objSecond, "Velocity");

            var variations = IoC.Resolve<List<int>>("Game.UObject.FindVariations", positionFirst, positionSecond, velocityFirst, velocitySecond);

            var collisionTree = IoC.Resolve<IDictionary<int, object>>("Game.Command.BuildCollisionTree");

            variations.ForEach(variation => collisionTree = (IDictionary<int, object>)collisionTree[variation]);

            IoC.Resolve<ICommand>("Game.Event.Collision", _objFirst, _objSecond).Execute();
        }
    }
}
