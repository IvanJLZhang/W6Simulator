#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: SerialPortCommulation
// Author: Ivan JL Zhang    Date: 2018/5/31 16:42:31    Version: 1.0.0
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
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace Granda.ATTS.SPP
{
    /// <summary>
    /// 串口通信的封装类
    /// </summary>
    public class SerialPortCommunication : INotifyPropertyChanged
    {
        private bool _connected = false;

        public SerialPort serialPort { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public event Action<byte[]> RaiseDataReceieved;

        public bool Connected
        {

            get => _connected;
            set
            {
                if (_connected != value)
                {
                    _connected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Connected)));
                }
            }
        }

        private SerialPortCommunication()
        {
            this.serialPort = new SerialPort();
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="portName">端口号</param>
        /// <param name="baudRate">波特率，默认为38400</param>
        /// <param name="parity">奇偶校验检查协议，默认为偶校验</param>
        /// <param name="dataBits">每个字节的标准数据位长度，默认为7</param>
        /// <param name="stopBits">每个字节的标准停止位数， 默认为2</param>
        /// <param name="rtsEnable">是否启用请求发送（RTS）信号， 默认为false</param>
        public SerialPortCommunication(string portName,
            int baudRate = 38400,
            Parity parity = Parity.Even,
            int dataBits = 7,
            StopBits stopBits = StopBits.Two,
            bool rtsEnable = false) : this()
        {
            this.serialPort.PortName = portName;// 端口号
            this.serialPort.BaudRate = baudRate;// 波特率
            this.serialPort.Parity = parity;// 奇偶校验检查协议
            this.serialPort.DataBits = dataBits;// 每个字节的标准数据位长度
            this.serialPort.StopBits = stopBits;// 每个字节的标准停止位数
            this.serialPort.RtsEnable = rtsEnable;// 是否启用请求发送（RTS）信号
            this.serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (this.serialPort.BytesToRead > 0)
            {
                byte[] buffer = new byte[this.serialPort.BytesToRead];
                this.serialPort.Read(buffer, 0, buffer.Length);
                RaiseDataReceieved?.Invoke(buffer.ToArray());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 打开Com口，侦听数据传输
        /// </summary>
        public void Open()
        {
            Close();
            try
            {
                this.serialPort.Encoding = this.Encoding;
                this.serialPort.Open();
                Connected = this.serialPort.IsOpen;
            }
            catch (Exception)
            {
                Connected = this.serialPort.IsOpen;
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            if (!Connected)
                return;
            try
            {
                this.serialPort.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Connected = this.serialPort.IsOpen;
            }
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            if (!Connected)
                return;
            try
            {
                this.serialPort.Write(message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Connected = this.serialPort.IsOpen;
            }
        }

        public void Close()
        {
            if (this.serialPort != null && Connected)
            {
                this.serialPort.Close();
                Connected = this.serialPort.IsOpen;
            }
        }
    }
}
