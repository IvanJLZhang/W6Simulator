#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: V6Emulator
// Author: Ivan JL Zhang    Date: 2018/5/31 10:55:13    Version: 1.0.0
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
using System.Text.RegularExpressions;
using AsyncTCP;
using W6Simulator.Command;

namespace W6Simulator.Devices
{
    public class V6Emulator : TCPCOMMODE
    {
        #region Protected Properties

        protected readonly string CMD_V6_On = "PG,ON\n";
        protected readonly string CMD_V6_Off = "PG,OFF\n";
        protected readonly string CMD_Pattern_Next = "PG,NEXT\n";
        protected readonly string CMD_Pattern_Back = "PG,BACK\n";

        protected readonly string CMD_V6_State = "PG,STATE\n";
        protected readonly string CMD_V6_Model = "PG,MODEL\n";

        #endregion

        public V6Emulator(AsyncTcpClient asyncTcpClient) : base(asyncTcpClient)
        {
        }

        public override bool HandleMessage(Message message)
        {
            switch (message.CommandType)
            {
                case "PG":
                    return handlePGCommand(message);
                default:
                    break;
            }
            return false;
        }

        #region Private Motheds

        bool handlePGCommand(Message message)
        {
            var paramList = message.ParamList.ToList();
            paramList.Add("OK");
            var replyMessage = new Message(message.CommandType, paramList);
            SendCmdToPgV6(replyMessage.MessageContext);
            return true;
        }

        /// <summary>
        /// 向 V6 PG发送命令（不等待响应）
        /// </summary>
        /// <param name="cmd"></param>
        private void SendCmdToPgV6(string cmd)
        {
            byte[] cmdFrame = GetPgFrameV6(cmd);
            SendMsgToServer(cmdFrame);
        }

        /// <summary>
        /// 将命令字符串封装成字节数组形式的 V6 PG通讯数据帧
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private byte[] GetPgFrameV6(string cmd)
        {
            byte[] frame = new byte[250];
            byte[] bts = Encoding.UTF8.GetBytes(cmd);
            Buffer.BlockCopy(bts, 0, frame, 0, bts.Length);
            return frame;
        }

        #endregion
    }
}
