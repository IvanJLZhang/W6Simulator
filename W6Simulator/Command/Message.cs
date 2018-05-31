#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: W6CMD
// Author: Ivan JL Zhang    Date: 2018/5/15 10:14:08    Version: 1.0.0
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
using System.Linq;
using System.Text;

namespace W6Simulator.Command
{
    public class Message
    {

        protected internal const byte FRAME_STX_BYTE = 0x02;
        protected internal const byte FRAME_DEST_BYTE = 0x80;
        protected internal const byte FRAME_ETX_BYTE = 0x03;

        public string CommandType { get; private set; }
        public IList<string> ParamList { get; private set; }
        public string MessageContext => ToString();
        public string MessageType { get; private set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;
        private byte[] _messageData;
        /// <summary>
        /// 解析消息时的构造方法
        /// </summary>
        /// <param name="messageData">消息的byte数组</param>
        public Message(byte[] messageData)
        {
            _messageData = messageData;
            MessageType = "RECV";
            Parse();
        }
        /// <summary>
        /// 解析消息时的构造方法
        /// </summary>
        /// <param name="messageContext">字符串消息</param>
        public Message(string messageContext)
        {
            var messageArr = messageContext.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            CommandType = messageArr[0];
            ParamList = messageArr.Skip(1).ToArray();
            MessageType = "SEND";
        }
        /// <summary>
        /// 新建消息时的构造方法
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="paramList"></param>
        public Message(string commandType, IList<string> paramList)
        {
            CommandType = commandType;
            ParamList = paramList;
            MessageType = "SEND";
            Encode();
        }
        private Message Parse()
        {
            if (_messageData == null || _messageData.Length <= 0)
                return null;
            if (_messageData[0] == FRAME_STX_BYTE &&
                _messageData[1] == FRAME_DEST_BYTE &&
                _messageData[_messageData.Length - 1] == FRAME_ETX_BYTE)
            {// 按照消息结构解析
                byte[] lengthBytes = _messageData.Skip(2).Take(4).ToArray();
                var messageLength = Convert.ToInt32(Encoding.GetString(lengthBytes), 16);// Int32.Parse("0x" + Encoding.GetString(lengthBytes));// 数据的长度
                                                                                         // 需要减去FRAME_STX_BYTE的1byte，FRAME_DEST_BYTE的1byte，FRAME_ETX_BYTE的1byte和数据长度的4byte长度
                byte[] dataBytes = _messageData.Skip(2 + 4).Take(_messageData.Length - 2 - 4 - 1).ToArray();
                var messageStr = Encoding.GetString(dataBytes);
                if (messageStr.Length == messageLength)
                {
                    var messageArr = messageStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    CommandType = messageArr[0];
                    ParamList = messageArr.Skip(1).ToArray();
                    return this;
                }
                else
                {
                    throw new InvalidOperationException("Wrong Message format");
                }
            }
            else
            {// 直接解析为文本字符串
                var messageStr = Encoding.GetString(_messageData);
                var messageArr = messageStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                CommandType = messageArr[0];
                ParamList = messageArr.Skip(1).ToArray();
                return this;
            }
        }

        public byte[] Encode()
        {
            if (CommandType == String.Empty)
                return new byte[] { };
            string messageStr = this.ToString();
            byte[] messageBytes = Encoding.GetBytes(messageStr.Length.ToString("X4") + messageStr);
            byte[] data = new byte[messageBytes.Length + 3];
            data[0] = FRAME_STX_BYTE;
            data[1] = FRAME_DEST_BYTE;
            Buffer.BlockCopy(messageBytes, 0, data, 2, messageBytes.Length);
            data[data.Length - 1] = FRAME_ETX_BYTE;
            return _messageData = data;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(CommandType);
            foreach (var item in ParamList)
            {
                stringBuilder.Append("," + item);
            }
            return stringBuilder.ToString();
        }
    }
}
