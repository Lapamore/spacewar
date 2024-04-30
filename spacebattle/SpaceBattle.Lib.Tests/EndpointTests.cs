using System.Collections;
using System.Collections.Concurrent;
using System.Configuration;
using System.Net.Http.Json;
using Hwdtech;
using Hwdtech.Ioc;
namespace SpaceBattle.Lib
{
    public class EndpointTest
    {
        private readonly Hashtable _idgame = new Hashtable();
        public EndpointTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            var threadDict = new ConcurrentDictionary<string, ServerThread>();
            var senderDict = new ConcurrentDictionary<string, ISender>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadDictionary", (object[] _) => threadDict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SenderDictionary", (object[] _) => senderDict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SenderGetByID", (object[] id) => senderDict[(string)id[0]]).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThreadID", (object[] id) => threadDict[(string)id[0]]).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetServerThreadIdByGameId", (object[] args) => { return _idgame[(int)args[0]]; }).Execute();

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
        public void IncorrectWorkTest()
        {
            var cmd = new Mock<ICommand>();
            cmd.Setup(x => x.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CheckCommandWork", (object[] args) => { return cmd.Object; }).Execute();
            var id = Guid.NewGuid();
            _idgame.Add(111, id);
            var mre = new ManualResetEvent(false);
            var th = IoC.Resolve<ServerThread>("CreateWithStartThread", id.ToString());
            var handle = new HttpClientHandler();
            var serv = new HttpClient(handle);
            serv.BaseAddress = new Uri("http://localhost:12233");
            var prms = new Dictionary<string, object>();
            prms.Add("adsfg", false);
            prms.Add("rotation speed", 4);
            var mess = new Mess("Error", 112, prms);
            var request = System.Net.HttpStatusCode.BadRequest;
            var ns = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
            Endpoint.Run(ns);
            var rec = JsonContent.Create(mess);
            var ans = serv.PostAsync("/message", rec);
            Assert.Equal(request, ans.Result.StatusCode);
            Endpoint.Stop();
            var hardstop = IoC.Resolve<ICommand>("HardStop", id.ToString(), () => { mre.Set(); });
            var sender = IoC.Resolve<ISender>("SenderGetByID", id.ToString());
            var send = IoC.Resolve<ICommand>("SendCommand", sender, hardstop);
            send.Execute();
            mre.WaitOne();
            th.Wait();
            cmd.Verify(x => x.Execute(), Times.Never());
        }
        [Fact]
        public void CorrectWorkTest()
        {
            var cmd = new Mock<ICommand>();
            cmd.Setup(x => x.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CheckCommandWork", (object[] args) => { return cmd.Object; }).Execute();
            var id = Guid.NewGuid();
            _idgame.Add(111, id);
            var mre = new ManualResetEvent(false);
            var th = IoC.Resolve<ServerThread>("CreateWithStartThread", id.ToString());
            var handle = new HttpClientHandler();
            var server = new HttpClient(handle);
            server.BaseAddress = new Uri("http://localhost:12233");
            var prms = new Dictionary<string, object>();
            prms.Add("asdfg", false);
            prms.Add("rotation speed", 4);
            var mess = new Mess("StartServer", 111, prms);
            var request = System.Net.HttpStatusCode.OK;
            var ns = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
            Endpoint.Run(ns);
            var rec = JsonContent.Create(mess);
            var ans = server.PostAsync("/message", rec);
            Assert.Equal(request, ans.Result.StatusCode);
            Endpoint.Stop();
            var hardstop = IoC.Resolve<ICommand>("HardStop", id.ToString(), () => { mre.Set(); });
            var sender = IoC.Resolve<ISender>("SenderGetByID", id.ToString());
            var send = IoC.Resolve<ICommand>("SendCommand", sender, hardstop);
            send.Execute();
            mre.WaitOne();
            th.Wait();
            cmd.Verify(x => x.Execute(), Times.Once());
        }
    }
}