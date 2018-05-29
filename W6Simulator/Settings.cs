#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Settings
// Author: Ivan JL Zhang    Date: 2018/5/15 15:19:54    Version: 1.0.0
// Description: 
//   
// 
// Revision History: 
// <Author>  		<Date>     	 	<Revision>  		<Modification>
// 	
//----------------------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using W6Simulator.Model;

namespace W6Simulator
{
    public class Settings : INotifyPropertyChanged
    {
        private string _ipAddress = "192.168.0.45";
        private int _port = 50000;
        private bool _isTcpCommunication = true;
        private bool _isRs232 = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public string DeviceTypeStr => this.DeviceType.ToString() + " Emulator: " + DeviceIndex;
        public DeviceType DeviceType { get; private set; } = DeviceType.W6;
        /// <summary>
        /// 
        /// </summary>
        public bool IsTcpCommunication
        {
            get => _isTcpCommunication;
            set
            {
                if (value != _isTcpCommunication)
                {
                    _isTcpCommunication = value;
                    IsRs232 = !_isTcpCommunication;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsTcpCommunication)));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsRs232
        {
            get => _isRs232;
            set
            {
                if (value != _isRs232)
                {
                    _isRs232 = value;
                    IsTcpCommunication = !_isRs232;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRs232)));
                }

            }
        }

        private string _connection = "Disconnect";
        public string Connection
        {
            get => _connection;
            set
            {
                if (value != _connection)
                {
                    _connection = value;
                    Disconnected = value == "Disconnect";
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Connection)));
                }
            }
        }
        private bool _disconnected = true;
        public bool Disconnected
        {
            get => _disconnected;
            set
            {
                if (value != _disconnected)
                {
                    _disconnected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Disconnected)));
                }
            }
        }
        #region TCP 
        /// <summary>
        /// 
        /// </summary>
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                if (value != _ipAddress)
                {
                    _ipAddress = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IpAddress)));
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Port
        {
            get => _port;
            set
            {
                if (value != _port)
                {
                    _port = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Port)));
                }
            }
        }

        public int DeviceIndex { get; internal set; } = 1;
        #endregion
    }
}
