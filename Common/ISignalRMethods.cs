using System.Collections.Generic;

namespace Common
{
    public interface ISignalRClientEvents
    {
        void ReceiveLength(List<string> message);
        void WatchDogBarked(int barkNo);
    }


    public interface ISignalRServerMethods
    {
        void DetermineLength(string message);
        int MakeANumber();
    }


    public static class SignalR
    {
        public static class ServerMethods
        {
            public static string DetermineLen => nameof(ISignalRServerMethods.DetermineLength);
            public static string GivEtNr => nameof(ISignalRServerMethods.MakeANumber);
        }

        public static class ClientEvents
        {
            public static string WatchDogBarking => nameof(ISignalRClientEvents.WatchDogBarked);
            public static string ReceiveLen => nameof(ISignalRClientEvents.ReceiveLength);
        }
    }
}
