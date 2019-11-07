using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRHub
{
    [HubName("TestHub")]
    public class TestHub : Hub<ISignalRClientEvents>, ISignalRServerMethods
    {
        private static int _longMethodCallNo;
        private static int _no;
        
        public override Task OnConnected()
        {
            Console.WriteLine($"Client with connectionId {Context.ConnectionId} connected to server");

            return base.OnConnected();
        }


        public async void DetermineLength(string message)
        {
            Console.WriteLine(message);

            List<string> returnMessage = new List<string>();
            returnMessage.Add($@"'{message}' has a length of {message.Length}");
            await Clients.All.ReceiveLength(returnMessage);
        }

        public int MakeANumber()
        {
            _no++;
            Console.WriteLine($"Make a number ({_no}) called");
            return _no;
        }

        public async Task<string> LongRunningMethod()
        {
            var callNo = Interlocked.Increment(ref _longMethodCallNo);
            Console.WriteLine($"Long running async method called ({callNo})");
            await Task.Delay(4000);
            Console.WriteLine($"Long running async method finished ({callNo})");
            return "We're back";
        }
    }
}