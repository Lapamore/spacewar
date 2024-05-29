using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class TestGameQueuePopStrategy
    {
        public TestGameQueuePopStrategy()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]
        public void GameQueuePopSuccess()
        {
            var gameCommandQueueMap = new Dictionary<int, Queue<Lib.ICommand>>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Pop", (object[] args) => new GameQueuePopStrategy().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Mapping", (object[] args) => gameCommandQueueMap).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.Queue", (object[] args) => new QueueProvider().Invoke(args)).Execute();

            var mockGameCommand = new Mock<Lib.ICommand>();
            var commandQueue = new Queue<Lib.ICommand>();
            commandQueue.Enqueue(mockGameCommand.Object);
            gameCommandQueueMap.Add(0, commandQueue);

            Assert.Single(commandQueue);
            IoC.Resolve<Lib.ICommand>("Queue.Pop", 0).Execute();
            Assert.Empty(commandQueue);
        }
    }
}
