#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: W6Emulator
// Author: Ivan JL Zhang    Date: 2018/5/15 17:32:01    Version: 1.0.0
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncTCP;
using W6Simulator.Command;
using W6Simulator.Model;

namespace W6Simulator.Devices
{
    public class W6Emulator
    {
        #region 不错的想法， 有待继续深入研究
        static Encoding Encoding { get; set; } = Encoding.ASCII;
        private List<Statement> conditionStatements = new List<Statement>();
        private List<Statement> normalStatements = new List<Statement>();
        private static Dictionary<string, string> ConfigurationList = new Dictionary<string, string>();

        public void LoadGmlStatementArray(string filePathName)
        {
            if (!File.Exists(filePathName))
                return;
            using (var stream = File.OpenRead(filePathName))
            {
                var reader = new StreamReader(stream, Encoding);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().TrimStart();
                    if ((string.IsNullOrEmpty(line)) || (line.Length >= 1 && line[0] == '#'))
                        continue;
                    if (line.Substring(0, 2).Equals(Operator.IF.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        var statement = ReadConditionStatements(reader, line);
                        conditionStatements.Add(statement);
                    }
                    else
                    {
                        normalStatements.Add(new Statement
                        {
                            Operator = Operator.NULL,
                            Condition = String.Empty,
                            StatementList = new List<string>() { line }
                        });
                    }
                }
            }
        }
        private Statement ReadConditionStatements(StreamReader reader, string line)
        {
            Operator @operator = (Operator)Enum.Parse(typeof(Operator), line.Substring(0, 2));
            line = line.Substring(2).TrimStart();
            string condition = line;
            List<string> statements = new List<string>();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine().Trim();
                if ((string.IsNullOrEmpty(line)) || (line.Length >= 1 && line[0] == '#'))
                    continue;

                if (line.Equals(Operator.END.ToString(), StringComparison.OrdinalIgnoreCase))
                    break;
                statements.Add(ParseStatement(line));
            }

            return new Statement
            {
                Operator = @operator,
                Condition = condition,
                StatementList = statements,
            };
        }

        private string ParseStatement(string line)
        {
            if (line.IndexOf("//") != -1)
                line = line.Substring(0, line.IndexOf("//"));
            return line;
        }

        /// <summary>
        /// 从gml文件中读取出配置信息
        /// </summary>
        /// <param name="filePathName"></param>
        public static void LoadConfiguration(string filePathName)
        {
            if (!File.Exists(filePathName))
                return;
            using (var stream = File.OpenRead(filePathName))
            {
                var reader = new StreamReader(stream, Encoding);
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().TrimStart();
                    if ((string.IsNullOrEmpty(line)) || (line.Length >= 1 && line[0] == '#'))
                        continue;
                    if (line[0] == '$' && line.Contains("="))
                    {
                        line = line.Substring(1);
                        var key_value = line.Split('=');
                        if (key_value.Length == 2)
                        {
                            key_value[0] = key_value[0].Trim();
                            key_value[1] = key_value[1].Trim();
                            if (ConfigurationList.ContainsKey(key_value[0]))
                                ConfigurationList.Add(key_value[0], key_value[1]);
                            else
                                ConfigurationList[key_value[0]] = key_value[1];
                        }
                    }
                }
            }
        }
        #endregion

        protected internal readonly string MSG_REMOTE_FUNCTION_END_GOOD = "REMOTE,LINE,END,GOOD";
        protected internal readonly string MSG_REMOTE_FUNCTION_END_NG = "REMOTE,LINE,END,NG";
        private readonly AsyncTcpClient asyncTcpClient = null;

        public event Action<object, Message> MessageSent;

        public W6Emulator(AsyncTcpClient asyncTcpClient)
        {
            this.asyncTcpClient = asyncTcpClient;
            this.asyncTcpClient.PropertyChanged += AsyncTcpClient_PropertyChanged;
        }

        public bool HandleMessage(Message message)
        {
            switch (message.CommandType)
            {
                case "WPM":
                    return WPM_DOWNLOAD(message);
                case "REMOTE":
                    return REMOTE(message);
                case "KEY":
                    return KEY_CONTROL(message);
                default:
                    var replyParamList = message.ParamList.ToList();
                    replyParamList.Add("OK");
                    return SendMsgToServer(new Message(message.CommandType, replyParamList));
            }
        }

        private void AsyncTcpClient_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(this.asyncTcpClient.Connected)))
            {
                if (this.asyncTcpClient.Connected)
                    OpenSendTunnel();
            }
        }

        /// <summary>
        /// 发送显示器信息
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="message"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool SendDisplayMessage(DISPLAY_MSG_TYPE msgType, string message, long color = -1)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"MSG,{msgType},");
            if (color != -1)
                stringBuilder.Append($"{color:X8},");
            stringBuilder.Append(message);

            return SendMsgToServer(stringBuilder.ToString());
        }
        /// <summary>
        /// 发送基本信息至Server
        /// </summary>
        /// <param name="messageType">下列选项之一： IMPORT_INFO/MODEL_INFO/SCPLIST_INFO/MODULE_INFO/KERNEL_INFO/NETWORK_INFO</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SendBaseInfoToServer(string messageType, string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"{messageType},{value}");

            return SendMsgToServer(stringBuilder.ToString());
        }
        /// <summary>
        /// 驱动档案下载
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool WPM_DOWNLOAD(Message message)
        {
            if (!message.CommandType.Equals("WPM"))
                return false;
            string function = message.ParamList[0];
            switch (function)
            {
                case "LOAD":
                    break;
                case "RELOAD":
                    break;
                case "UNLOAD":
                    break;
                case "CHANGE":
                    break;
                default:
                    break;
            }
            return true;
        }
        /// <summary>
        /// KEY制服控
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool KEY_CONTROL(Message message)
        {
            if (!message.CommandType.Equals("KEY"))
                return false;

            var replyParamList = message.ParamList.ToList();
            replyParamList.Add("OK");

            return SendMsgToServer(new Message(message.CommandType, replyParamList));

            //var function = message.ParamList[0];
            //switch (function)
            //{
            //    case "RESET":
            //        break;
            //    case "ENTER":

            //        break;
            //    case "BACK":
            //        break;
            //    case "NEXT":
            //        break;
            //    case "FUNC":
            //        break;
            //    case "UP":
            //        break;
            //    case "AUTO":
            //        break;
            //    case "DOWN":
            //        break;
            //    default:
            //        break;
            //}
        }
        /// <summary>
        /// RCV LOG
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool RCV_LOG(Message message)
        {
            if (!message.CommandType.Equals("RCV"))
                return false;
            var function = message.ParamList[0];
            switch (function)
            {
                case "INIT":
                    break;
                case "WRITE":
                    break;
                case "DEINIT":
                    break;
                default:
                    break;
            }
            return true;
        }
        /// <summary>
        /// REMOTE
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool REMOTE(Message message)
        {
            if (!message.CommandType.Equals("REMOTE"))
                return false;
            Message replyMessage = new Message(MSG_REMOTE_FUNCTION_END_GOOD);
            return SendMsgToServer(replyMessage);
        }

        #region FILE

        #endregion

        private Queue<Message> msgs2send = new Queue<Message>();

        private bool SendMsgToServer(string message)
        {
            return SendMsgToServer(new Message(message));
        }

        private bool SendMsgToServer(Message message)
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
                                this.asyncTcpClient.Send(data.MessageContext);
                                MessageSent?.Invoke(this, data);
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
