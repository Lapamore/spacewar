using System.Collections;
using Hwdtech;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SpaceBattle.Lib
{
    public class Endpoint
    {
        private static WebApplication? _app;
        public static void Run(object _newscope)
        {
            var webappbuild = WebApplication.CreateBuilder();
            _app = webappbuild.Build();
            _app.UseHttpsRedirection();
            _app.Map("/message", (Mess _mess) =>
            {
                try
                {
                    IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _newscope).Execute();
                    var id = _mess.id;
                    var stid = IoC.Resolve<Guid>("GetServerThreadIdByGameId", id);
                    var cmd = IoC.Resolve<ICommand>("CheckCommandWork");
                    return Results.Ok(_mess);
                }
                catch
                {
                    return Results.BadRequest();
                }
            });
            _app.RunAsync("http://localhost:12233");
        }
        public static void Stop()
        {
            if (_app != null)
            {
                _app.StopAsync();
            }
        }
    }
    public record Mess(string cmd, int id, Hashtable prms);
}