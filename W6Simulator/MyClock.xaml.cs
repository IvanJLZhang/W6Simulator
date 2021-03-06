﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using System.Windows.Threading;

namespace W6Simulator
{
    /// <summary>
    /// MyClock.xaml 的交互逻辑
    /// </summary>
    public partial class MyClock : UserControl, INotifyPropertyChanged
    {
        public MyClock()
        {
            InitializeComponent();
            this.Loaded += MyClock_Loaded;
        }

        private void MyClock_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            clockTimer = new Timer((obj) =>
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Clocker = Clocker.AddSeconds(1);
                }), DispatcherPriority.Normal, null);
            }, null, 0, 1000);
        }

        public MyClock(DateTime clocker) : this()
        {
            Clocker = clocker;
        }

        public DateTime Clocker
        {
            get => _clocker;
            set
            {
                _clocker = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Clocker)));
            }
        }

        Timer clockTimer;
        private DateTime _clocker = DateTime.Now;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Now => Clocker.ToString("yyyy/MM/dd hh:mm:ss");

        public void UpdateClock(DateTime dateTime)
        {
            this.Clocker = dateTime;
        }
    }
}
