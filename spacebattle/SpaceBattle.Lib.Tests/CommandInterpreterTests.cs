using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests
{
    public class ActionHandlingTest
    {
        Dictionary<int, Queue<ICommand>> commandQueuesByGameId = new Dictionary<int, Queue<ICommand>>();
        Dictionary<int, IUObject> uObjectsById = new Dictionary<int, IUObject>();


        public ActionHandlingTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObject.Mapping", (object[] args) => uObjectsById).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Retriever", (object[] args) => new QueueProvider().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObject.Retriever", (object[] args) => new UObjectProvider().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Creator", (object[] args) => new MessageToCommandConverter().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Updater", (object[] args) => new QueuePusher().Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue.Mapping", (object[] args) => commandQueuesByGameId).Execute();

            Mock<IUObject> mockGameUObject = new Mock<IUObject>();
            mockGameUObject.Setup(x => x.setProperty(It.IsAny<string>(), It.IsAny<object>()));

            commandQueuesByGameId.Add(1, new Queue<ICommand>());
            uObjectsById.Add(1, mockGameUObject.Object);
        }

        [Fact]
        public void TestMessageParamException()
        {
            Mock<ICommand> mockCmd = new Mock<ICommand>();

            Mock<IUObject> mockGameUObject = new Mock<IUObject>();
            mockGameUObject.Setup(x => x.setProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

            Mock<IGameCommandMessage> mockMsg = new Mock<IGameCommandMessage>();
            mockMsg.SetupGet(x => x.GameID).Throws(new Exception());
            mockMsg.SetupGet(x => x.TypeCommand).Returns("Example");
            mockMsg.SetupGet(x => x.Args).Returns(new Dictionary<string, object> { { "Example", 1 } });
            mockMsg.SetupGet(x => x.ObjectID).Returns(1);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "cmd.Example", (object[] args) => mockCmd.Object).Execute();
            var interpretationCmd = new ActionDispatcher(mockMsg.Object);
            Assert.Throws<Exception>(() => { interpretationCmd.Execute(); });
        }

        [Fact]
        public void TestPush()
        {
            Mock<ICommand> mockCmd = new Mock<ICommand>();

            Mock<IGameCommandMessage> mockMsg = new Mock<IGameCommandMessage>();
            mockMsg.SetupGet(x => x.GameID).Returns(1);
            mockMsg.SetupGet(x => x.TypeCommand).Returns("Example");
            mockMsg.SetupGet(x => x.Args).Returns(new Dictionary<string, object> { { "Example", 1 } });
            mockMsg.SetupGet(x => x.ObjectID).Returns(1);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "cmd.Example", (object[] args) => mockCmd.Object).Execute();
            var interpretationCmd = new ActionDispatcher(mockMsg.Object);
            interpretationCmd.Execute();
            Assert.True(commandQueuesByGameId[1].Count() == 1);
        }

        [Fact]
        public void TestGetUObjectException()
        {
            Mock<ICommand> mockCmd = new Mock<ICommand>();

            Mock<IUObject> mockGameUObject = new Mock<IUObject>();
            mockGameUObject.Setup(x => x.setProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

            Mock<IGameCommandMessage> mockMsg = new Mock<IGameCommandMessage>();
            mockMsg.SetupGet(x => x.GameID).Returns(1);
            mockMsg.SetupGet(x => x.TypeCommand).Returns("Example");
            mockMsg.SetupGet(x => x.Args).Returns(new Dictionary<string, object> { { "Example", 1 } });
            mockMsg.SetupGet(x => x.ObjectID).Returns(52);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "cmd.Example", (object[] args) => mockCmd.Object).Execute();
            var interpretationCmd = new ActionDispatcher(mockMsg.Object);
            Assert.Throws<Exception>(() => { interpretationCmd.Execute(); });
        }

        [Fact]
        public void TestGetGameException()
        {
            Mock<ICommand> mockCmd = new Mock<ICommand>();

            Mock<IUObject> mockGameUObject = new Mock<IUObject>();
            mockGameUObject.Setup(x => x.setProperty(It.IsAny<string>(), It.IsAny<object>())).Verifiable();

            Mock<IGameCommandMessage> mockMsg = new Mock<IGameCommandMessage>();
            mockMsg.SetupGet(x => x.GameID).Returns(52);
            mockMsg.SetupGet(x => x.TypeCommand).Returns("Example");
            mockMsg.SetupGet(x => x.Args).Returns(new Dictionary<string, object> { { "Example", 1 } });
            mockMsg.SetupGet(x => x.ObjectID).Returns(1);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "cmd.Example", (object[] args) => mockCmd.Object).Execute();
            var interpretationCmd = new ActionDispatcher(mockMsg.Object);
            Assert.Throws<Exception>(() => { interpretationCmd.Execute(); });
        }
    }
}
