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
        //public const string SPLIT_CHR = ",";
        //// 显示器信息
        //public const string MSG = "MSG";

        //// 驱动档案信息
        //public const string MODEL_INFO = "MODEL_INFO";
        //public const string IMPORT_INFO = "IMPORT_INFO";
        //public const string SCPLIST_INFO = "SCPLIST_INFO";

        //// 模块版本
        //// 网络信息
        //// 时间设置
        //// 重启
        //// 驱动档案下载
        //// TCP 连接检查
        //// KEY 遥控
        //// 电压/电流
        //// RCV LOG
        //// REMOTE
        //// FILE

        public string CommandType;
        public IList<string> ParamList;
    }
}
