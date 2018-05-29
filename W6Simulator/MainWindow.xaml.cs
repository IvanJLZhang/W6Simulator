using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AsyncTCP;
using W6Simulator.Command;
using W6Simulator.Devices;
using W6Simulator.Model;

namespace W6Simulator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Settings Settings { get; set; }
        public AsyncTcpClient tcpClient;
        W6Emulator w6Emulator;
        public BindingList<Message> MessageList { get; set; } = new BindingList<Message>();

        public event PropertyChangedEventHandler PropertyChanged;

        private static int deviceIndex = 1;
        private static Dictionary<int, Window> DeviceList = new Dictionary<int, Window>();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            Settings = new Settings();
            Settings.DeviceIndex = deviceIndex++;
            DeviceList.Add(Settings.DeviceIndex, this);
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tcpClient = new AsyncTcpClient(IPAddress.Parse(Settings.IpAddress), Settings.Port);
            tcpClient.Encoding = Encoding.UTF8;
            tcpClient.DatagramReceived += TcpClient_DatagramReceived;
            tcpClient.PropertyChanged += TcpClient_PropertyChanged;

            w6Emulator = new W6Emulator(tcpClient);
            w6Emulator.MessageSent += W6Emulator_MessageSent;
            tcpClient.Connect();
        }

        private void W6Emulator_MessageSent(object sender,  Message message)
        {
            this.Dispatcher.BeginInvoke(new Action(() => MessageList.Add(message)), System.Windows.Threading.DispatcherPriority.Normal);
        }

        private void TcpClient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(tcpClient.Connected))
            {
                Settings.Connection = tcpClient.Connected ? "Connected" : "Disconnect";
            }
        }

        private void TcpClient_DatagramReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {
            Message message = new Message(e.ReceivedBytes);
            this.Dispatcher.BeginInvoke(new Action(() => { MessageList.Add(message); }), System.Windows.Threading.DispatcherPriority.Normal);
            w6Emulator.HandleMessage(message);
        }

        private void TcpClient_ServerExceptionOccurred(object sender, TcpServerExceptionOccurredEventArgs e)
        {
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DeviceList[Settings.DeviceIndex].Close();

            foreach (var item in DeviceList)
            {
                if (item.Key != Settings.DeviceIndex)
                {
                    Thread.Sleep(1000);
                    item.Value.Close();
                }
            }
            DeviceList.Clear();
        }
        /// <summary>
        /// New a device
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //RunNewWindowAsync<MainWindow>();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }

        private Task RunNewWindowAsync<TWindow>() where TWindow : System.Windows.Window, new()
        {
            TaskCompletionSource<object> tc = new TaskCompletionSource<object>();
            Thread thread = new Thread(() =>
            {
                TWindow win = new TWindow();
                win.Closed += (d, k) =>
                {
                    System.Windows.Threading.Dispatcher.ExitAllFrames();
                };
                win.Show();
                System.Windows.Threading.Dispatcher.Run();
                tc.SetResult(null);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tc.Task;
        }
    }
}
