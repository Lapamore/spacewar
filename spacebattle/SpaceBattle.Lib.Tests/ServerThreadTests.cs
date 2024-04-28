using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
namespace SpaceBattle.Lib.Tests;
public class ServerThreadTests
{
    public ServerThreadTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var threadDict = new ConcurrentDictionary<string, ServerThread>();
        var senderDict = new ConcurrentDictionary<string, ISender>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadDictionary", (object[] _) => threadDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SenderDictionary", (object[] _) => senderDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SenderGetByID", (object[] id) => senderDict[(string)id[0]]).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThreadID", (object[] id) => threadDict[(string)id[0]]).Execute();

        var startthread = new StartThread();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateWithStartThread", (object[] args) => startthread.Invoke(args)).Execute();
        var hardstop = new HardStop();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "HardStop", (object[] args) => hardstop.Invoke(args)).Execute();
        var softstop = new SoftStop();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SoftStop", (object[] args) => softstop.Invoke(args)).Execute();
        var sendcommand = new SendCommand();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SendCommand", (object[] args) => sendcommand.Invoke(args)).Execute();
    }
    [Fact]
    public void HardStopThreadTest()
    {
        var id2 = Guid.NewGuid().ToString();
        var st2 = IoC.Resolve<ServerThread>("CreateWithStartThread", id2);
        var mre = new ManualResetEvent(false);
        var hardstop = IoC.Resolve<ICommand>("HardStop", id2, () => { mre.Set(); });
        var send = IoC.Resolve<ISender>("SenderGetByID", id2);
        var sendcommand = IoC.Resolve<ICommand>("SendCommand", send, hardstop);

        sendcommand.Execute();
        mre.WaitOne(200);
        Assert.True(st2.Stopped());
        Assert.True(st2.EmptyQueue());
    }
    [Fact]
    public void HardStopCommandThreadWithAction()
    {
        var id3 = Guid.NewGuid().ToString();
        var st3 = IoC.Resolve<ServerThread>("CreateWithStartThread", id3);
        var hardstop = IoC.Resolve<ICommand>("HardStop", id3);
        Assert.NotNull(hardstop);
        var senderTrue = IoC.Resolve<ISender>("SenderGetByID", id3);
        var mre = new ManualResetEvent(false);
        IoC.Resolve<ICommand>("SendCommand", senderTrue, new ActionCommand(() => { mre.Set(); })).Execute();
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", senderTrue, hardstop);
        sendCommand.Execute();
        mre.WaitOne();
        st3.Wait();
        Assert.True(st3.EmptyQueue());
        Assert.True(st3.Stopped());
    }
    [Fact]
    public void HardStopThreadWithException()
    {
        var id4 = Guid.NewGuid().ToString();
        var id5 = Guid.NewGuid().ToString();
        var command1 = new Mock<ICommand>();
        var regStrategy1 = new Mock<IStrategy>();
        command1.Setup(_command => _command.Execute());
        regStrategy1.Setup(_strategy => _strategy.Invoke(It.IsAny<object[]>())).Returns(command1.Object);
        Action act1 = () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler", (object[] args) => regStrategy1.Object.Invoke(args)).Execute();
        };
        var th4 = IoC.Resolve<ServerThread>("CreateWithStartThread", id4, act1);
        var th5 = IoC.Resolve<ServerThread>("CreateWithStartThread", id5, act1);
        var mre1 = new ManualResetEvent(false);
        var hardStopCommand1 = IoC.Resolve<ICommand>("HardStop", id4, () => { mre1.Set(); });
        var hardStopCommand2 = IoC.Resolve<ICommand>("HardStop", id5, () => { mre1.Set(); });
        var senderFalse1 = IoC.Resolve<ISender>("SenderGetByID", id4);
        var senderFalse2 = IoC.Resolve<ISender>("SenderGetByID", id5);
        var sendCommand1 = IoC.Resolve<ICommand>("SendCommand", senderFalse1, hardStopCommand2);
        var sendCommand2 = IoC.Resolve<ICommand>("SendCommand", senderFalse2, hardStopCommand2);
        var sendCommand3 = IoC.Resolve<ICommand>("SendCommand", senderFalse1, hardStopCommand1);

        sendCommand1.Execute();
        sendCommand2.Execute();
        sendCommand3.Execute();
        mre1.WaitOne();
        th4.Wait();
        th5.Wait();
        Assert.True(th4.Stopped());
        Assert.True(th5.Stopped());
        command1.Verify();
    }
    [Fact]
    public void SoftStopThreadTest()
    {
        var mockCommand1 = new Mock<ICommand>();
        var mockCommand2 = new Mock<ICommand>();
        var mockCommand3 = new Mock<ICommand>();
        var mockCommand4 = new Mock<ICommand>();
        var mockCommand5 = new Mock<ICommand>();
        var mockCommand6 = new Mock<ICommand>();
        mockCommand1.Setup(_command => _command.Execute()).Verifiable();
        mockCommand2.Setup(_command => _command.Execute()).Verifiable();
        mockCommand3.Setup(_command => _command.Execute()).Verifiable();
        mockCommand4.Setup(_command => _command.Execute()).Verifiable();
        mockCommand5.Setup(_command => _command.Execute()).Verifiable();
        mockCommand6.Setup(_command => _command.Execute()).Verifiable();

        var mre1 = new ManualResetEvent(false);
        var id6 = Guid.NewGuid().ToString();
        var th6 = IoC.Resolve<ServerThread>("CreateWithStartThread", id6);
        Assert.True(th6.EmptyQueue());
        var softStopCommand = IoC.Resolve<ICommand>("SoftStop", id6);
        var sender = IoC.Resolve<ISender>("SenderGetByID", id6);
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand1.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand2.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand3.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand4.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand5.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand6.Object).Execute();
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", sender, softStopCommand);
        sendCommand.Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, new ActionCommand(() => { mre1.Set(); })).Execute();
        Assert.False(th6.EmptyQueue());
        mre1.WaitOne();
        th6.Wait();
        mockCommand1.Verify();
        mockCommand2.Verify();
        mockCommand3.Verify();
        mockCommand4.Verify();
        mockCommand5.Verify();
        mockCommand6.Verify();
        Assert.True(th6.EmptyQueue());
        Assert.True(th6.Stopped());
    }
    [Fact]
    public void SoftStopCommandThreadWithException()
    {
        var id8 = Guid.NewGuid().ToString();
        var id9 = Guid.NewGuid().ToString();
        var command2 = new Mock<ICommand>();
        var regStrategy2 = new Mock<IStrategy>();
        command2.Setup(_command => _command.Execute());
        regStrategy2.Setup(_strategy => _strategy.Invoke(It.IsAny<object[]>())).Returns(command2.Object);
        Action act1 = () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler", (object[] args) => regStrategy2.Object.Invoke(args)).Execute();
        };

        var th8 = IoC.Resolve<ServerThread>("CreateWithStartThread", id8, act1);
        var th9 = IoC.Resolve<ServerThread>("CreateWithStartThread", id9, act1);
        var mre1 = new ManualResetEvent(false);
        var softStopCommand1 = IoC.Resolve<ICommand>("SoftStop", id8, () => { mre1.Set(); });
        var softStopCommand2 = IoC.Resolve<ICommand>("SoftStop", id9, () => { mre1.Set(); });
        var sender1 = IoC.Resolve<ISender>("SenderGetByID", id8);
        var sender2 = IoC.Resolve<ISender>("SenderGetByID", id9);
        var sendCommand1 = IoC.Resolve<ICommand>("SendCommand", sender1, softStopCommand2);
        var sendCommand2 = IoC.Resolve<ICommand>("SendCommand", sender2, softStopCommand2);
        var sendCommand3 = IoC.Resolve<ICommand>("SendCommand", sender1, softStopCommand1);

        sendCommand1.Execute();
        sendCommand2.Execute();
        sendCommand3.Execute();
        mre1.WaitOne();
        th8.Wait();
        th9.Wait();
        Assert.True(th8.EmptyQueue());
        Assert.True(th8.Stopped());
    }
    [Fact]
    public void ThreadSoftStopTestWithAction()
    {
        var mockCommand1 = new Mock<ICommand>();
        var mockCommand2 = new Mock<ICommand>();
        var mockCommand3 = new Mock<ICommand>();
        var mockCommand4 = new Mock<ICommand>();
        var mockCommand5 = new Mock<ICommand>();
        var mockCommand6 = new Mock<ICommand>();
        mockCommand1.Setup(_command => _command.Execute()).Verifiable();
        mockCommand2.Setup(_command => _command.Execute()).Verifiable();
        mockCommand3.Setup(_command => _command.Execute()).Verifiable();
        mockCommand4.Setup(_command => _command.Execute()).Verifiable();
        mockCommand5.Setup(_command => _command.Execute()).Verifiable();
        mockCommand6.Setup(_command => _command.Execute()).Verifiable();

        var mre1 = new ManualResetEvent(false);
        var id11 = Guid.NewGuid().ToString();
        var th11 = IoC.Resolve<ServerThread>("CreateWithStartThread", id11);
        var softStopCommand = IoC.Resolve<ICommand>("SoftStop", id11, () => { mre1.Set(); });
        var sender = IoC.Resolve<ISender>("SenderGetByID", id11);
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand1.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand2.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand3.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand4.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand5.Object).Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, mockCommand6.Object).Execute();
        var sendCommand = IoC.Resolve<ICommand>("SendCommand", sender, softStopCommand);
        sendCommand.Execute();
        IoC.Resolve<ICommand>("SendCommand", sender, new ActionCommand(() => { mre1.Set(); })).Execute();
        mre1.WaitOne();
        th11.Wait();
        mockCommand1.Verify();
        mockCommand2.Verify();
        mockCommand3.Verify();
        mockCommand4.Verify();
        mockCommand5.Verify();
        mockCommand6.Verify();
        Assert.True(th11.EmptyQueue());
        Assert.True(th11.Stopped());
    }
    [Fact]
    public void EqualsNull()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var rec = new Reciever(queue);
        var id15 = Guid.NewGuid().ToString();
        var th15 = new ServerThread(rec, id15);
        var check1 = th15.Equals(null);
        Assert.False(check1);
    }
    [Fact]
    public void EqualsServerThread()
    {
        var queue1 = new BlockingCollection<ICommand>(100);
        var rec1 = new Reciever(queue1);
        var id20 = Guid.NewGuid().ToString();
        var th20 = new ServerThread(rec1, id20);
        var queue2 = new BlockingCollection<ICommand>(100);
        var rec2 = new Reciever(queue2);
        var id21 = Guid.NewGuid().ToString();
        var th21 = new ServerThread(rec2, id21);
        var check2 = th20.Equals(th21);
        Assert.False(check2);
    }
    [Fact]
    public void EqualsDifferentTypes()
    {
        var queue = new BlockingCollection<ICommand>(100);
        var rec = new Reciever(queue);
        var id25 = Guid.NewGuid().ToString();
        var th25 = new ServerThread(rec, id25);
        var check3 = th25.Equals(28);
        Assert.False(check3);
    }
}
