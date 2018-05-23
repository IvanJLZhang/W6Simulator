using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using W6Simulator.Model;

namespace W6Simulator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public Settings Settings { get; set; }
        AsyncTcpClient tcpClient;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Settings = new Settings();
            this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            tcpClient = new AsyncTcpClient(IPAddress.Parse(Settings.IpAddress), Settings.Port);
            //tcpClient.ServerConnected += TcpClient_ServerConnected;
            //tcpClient.ServerDisconnected += TcpClient_ServerDisconnected;
            //tcpClient.ServerExceptionOccurred += TcpClient_ServerExceptionOccurred;
            //tcpClient.DatagramReceived += TcpClient_DatagramReceived;
            tcpClient.PropertyChanged += TcpClient_PropertyChanged;
            //tcpClient.Connect();
        }

        private void TcpClient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(tcpClient.Connected))
                Settings.Connection = tcpClient.Connected ? "Connected" : "Disconnect";
        }

        private void TcpClient_DatagramReceived(object sender, TcpDatagramReceivedEventArgs<byte[]> e)
        {
            throw new NotImplementedException();
        }

        private void TcpClient_ServerExceptionOccurred(object sender, TcpServerExceptionOccurredEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
