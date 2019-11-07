using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Common;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IHubProxy _testHubProxy;
   
        public MainWindow()
        {
            InitializeComponent();

            const string url = @"http://localhost:8080/";

            var connection = new HubConnection(url)
            {
                TraceLevel = TraceLevels.All,
                TraceWriter = Console.Out
            };
            
            _testHubProxy = connection.CreateHubProxy("TestHub");

            SetupSignalREvents();

            connection.Start().Wait();
        }



        private void SetupSignalREvents()
        {
            _testHubProxy.On(SignalR.ClientEvents.ReceiveLen, (List<string> msg) =>
            {
                UpdateList(msg.FirstOrDefault());
            });

            _testHubProxy.On(SignalR.ClientEvents.WatchDogBarking, (int i) =>
            {
                UpdateWatchdog(i);
            });
        }




        private void UpdateWatchdog(int i)
        {
            Dispatcher?.BeginInvoke(new Action(() => WatchdogLabel.Content= $"WatchDog: {i}"));
        }


        void UpdateList(string msg)
        {
            Dispatcher?.BeginInvoke(new Action(() => ReceiveListBox.Items.Add(msg)));
        }

      
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            var txtLine = SendTextBox.Text;
      
            await _testHubProxy.Invoke(SignalR.ServerMethods.DetermineLen, txtLine);
        }


        private async void MakeNumberButton_Click(object sender, RoutedEventArgs e)
        {
            var nr = await _testHubProxy.Invoke<int>(SignalR.ServerMethods.MakeAnumber);

            Dispatcher?.BeginInvoke(new Action(() => ReceiveListBox.Items.Add($"You've got number {nr}")));
        }

        private async void LongRunningButton_Click(object sender, RoutedEventArgs e)
        {
            var msg =  await _testHubProxy.Invoke<string>(SignalR.ServerMethods.LongRunning);

            Dispatcher?.BeginInvoke(new Action(() => ReceiveListBox.Items.Add(msg)));
        }
    }
}
