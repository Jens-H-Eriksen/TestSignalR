using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNet.SignalR;

namespace SignalRHub
{
    public class TestHub : Hub<ISignalRClientEvents>, ISignalRServerMethods
    {
        private static int _no;
        
        public override Task OnConnected()
        {
            Console.WriteLine($"Client with connectionId {Context.ConnectionId} connected to server");

            return base.OnConnected();
        }


        public void DetermineLength(string message)
        {
            Console.WriteLine(message);

            List<string> returnMessage = new List<string>();
            returnMessage.Add($@"'{message}' has a length of {message.Length}");
            Clients.All.ReceiveLength(returnMessage);
        }

        public int MakeANumber()
        {
            _no++;
            Console.WriteLine($"Make a number ({_no}) called");
            return _no;
        }

    }
}