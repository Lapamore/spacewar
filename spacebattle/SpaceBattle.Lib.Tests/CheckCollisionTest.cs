using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class CheckCollisionTests
    {
        public CheckCollisionTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.GetProperty", (object[] args) => new List<int> { 1, 1, 1, 1, 1 }).Execute();

            new InitFindVariationsCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.CheckCollision", (object[] args) => new CheckCollisionCommand((IUObject)args[0], (IUObject)args[1])).Execute();
        }

        [Fact]

        public void CollisionTestPositive()
        {
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(c => c.Execute()).Verifiable();

            var mockDictionary = new Mock<IDictionary<int, object>>();
            mockDictionary.SetupGet(d => d[It.IsAny<int>()]).Returns(mockDictionary.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.BuildCollisionTree", (object[] args) => mockDictionary.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Event.Collision", (object[] args) => mockCommand.Object).Execute();

            var mockUObject = new Mock<IUObject>();

            var checkCollisionCommand = IoC.Resolve<ICommand>("Game.Command.CheckCollision", mockUObject.Object, mockUObject.Object);

            checkCollisionCommand.Execute();

            mockCommand.Verify();
        }

        [Fact]
        public void TryGetNewTreeThrowsException()
        {
            var mockCommand = new Mock<ICommand>();
            mockCommand.Setup(c => c.Execute()).Verifiable();

            var mockDictionary = new Mock<IDictionary<int, object>>();
            mockDictionary.SetupGet(d => d[It.IsAny<int>()]).Throws(new System.Collections.Generic.KeyNotFoundException()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.BuildCollisionTree", (object[] args) => mockDictionary.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Event.Collision", (object[] args) => mockCommand.Object).Execute();

            var mockUObject = new Mock<IUObject>();

            var checkCollisionCommand = IoC.Resolve<ICommand>("Game.Command.CheckCollision", mockUObject.Object, mockUObject.Object);

            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => checkCollisionCommand.Execute());
            mockDictionary.Verify(d => d[It.IsAny<int>()], Times.Once());
            mockCommand.Verify(command => command.Execute(), Times.Never());
        }
    }
}
