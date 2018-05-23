#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Helper
// Author: Ivan JL Zhang    Date: 2018/5/22 11:54:41    Version: 1.0.0
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
using System.Reflection;
using System.Text;
using W6Simulator.Command;

namespace W6Simulator
{
    public static class Helper
    {
        /// <summary>
        /// 将消息结构体转化为字符串消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ToMessageString(this Message message)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(message.CommandType);
            foreach (var item in message.ParamList)
            {
                stringBuilder.Append("," + item);
            }
            return stringBuilder.ToString();
        }
        /// <summary>
        /// 将字符串消息转换为消息结构体
        /// </summary>
        /// <param name="receivedMessage"></param>
        /// <returns></returns>
        public static Message ParseMessage(this string receivedMessage)
        {
            if (String.IsNullOrEmpty(receivedMessage))
                return null;
            Message message = new Message();
            var valueArr = receivedMessage.Split(',');
            if (valueArr.Length <= 0)
                return null;
            message.CommandType = valueArr[0];
            List<String> paramList = new List<string>();
            for (int index = 1; index < valueArr.Length; index++)
            {
                paramList.Add(valueArr[index]);
            }
            message.ParamList = paramList;
            return message;
        }

        /// <summary>
        /// 获取类下面所有public方法
        /// </summary>
        /// <param name="myType"></param>
        /// <returns></returns>
        public static MethodInfo[] GetMethodList(Type myType)
        {
            return myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }
    }
}
