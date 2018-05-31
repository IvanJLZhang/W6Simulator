#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Emulator
// Author: Ivan JL Zhang    Date: 2018/5/31 10:39:42    Version: 1.0.0
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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncTCP;
using W6Simulator.Command;

namespace W6Simulator.Devices
{
    /// <summary>
    /// Emlator Device的基类，主要处理TCP消息的发送
    /// </summary>
    public abstract class TCPCOMMODE: IEmulator
    {
        protected Encoding Encoding { get; set; } = Encoding.UTF8;

        protected readonly AsyncTcpClient asyncTcpClient = null;

        private Queue<byte[]> msgs2send = new Queue<byte[]>();

        public event Action<object, Message> MessageSent;

        public TCPCOMMODE(AsyncTcpClient asyncTcpClient)
        {
            this.asyncTcpClient = asyncTcpClient;
            this.asyncTcpClient.PropertyChanged += AsyncTcpClient_PropertyChanged;
        }

        public abstract bool HandleMessage(Message message);

        private void AsyncTcpClient_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(this.asyncTcpClient.Connected)))
            {
                if (this.asyncTcpClient.Connected)
                    OpenSendTunnel();
            }
        }

        protected bool SendMsgToServer(string message)
        {
            return SendMsgToServer(new Message(message));
        }

        protected bool SendMsgToServer(Message message)
        {
            return SendMsgToServer(message.Encode());
        }

        protected bool SendMsgToServer(byte[] message)
        {
            msgs2send?.Enqueue(message);
            return true;
        }

        /// <summary>
        /// 打开发送信息通道
        /// </summary>
        private void OpenSendTunnel() => Task.Factory.StartNew(() => SendThreadFunc());
        /// <summary>
        /// 发送信息线程方法
        /// </summary>
        private void SendThreadFunc()
        {
            Trace.WriteLine("Open send data tunnel");
            lock (this.asyncTcpClient)
            {
                while (this.asyncTcpClient.Connected)
                {
                    if (msgs2send.Count > 0)
                    {
                        try
                        {
                            var data = msgs2send.Dequeue();
                            if (data != null)
                            {
                                this.asyncTcpClient.Send(data);
                                MessageSent?.Invoke(this, new Message(this.Encoding.GetString(data)));
                            }
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(ex);
                        }
                    }
                    else
                        Thread.Sleep(200);
                }
            }
        }
        public void Reset()
        {
            msgs2send.Clear();
        }
    }
}
