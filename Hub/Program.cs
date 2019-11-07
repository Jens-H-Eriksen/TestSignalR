using System;
using System.ComponentModel;
using System.Threading;
using Common;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;


namespace SignalRHub
{
    class Program
    {
        private static int _bark;

        static void Main(string[] args)
        {
            string url = @"http://localhost:8080/";

            using (WebApp.Start<OwinStartup>(url))
            {
                Console.WriteLine($"Server running at {url}");

                var worker = new BackgroundWorker();

                worker.DoWork += OnWorkerOnDoWork;
                worker.RunWorkerAsync();

                Console.ReadLine();
            }
        }

        private static void OnWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<TestHub, ISignalRClientEvents>();

                hubContext.Clients.All.WatchDogBarked(_bark++);
                Thread.Sleep(1000);
            }
        }
    }




    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
           // app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
