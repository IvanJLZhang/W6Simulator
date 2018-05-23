#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: MSG_TYPE
// Author: Ivan JL Zhang    Date: 2018/5/16 17:11:43    Version: 1.0.0
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

namespace W6Simulator.Devices
{
    /// <summary>
    /// 显示器消息类型
    /// </summary>
    public enum DISPLAY_MSG_TYPE
    {
        /// <summary>
        /// 普通消息
        /// </summary>
        MSG_NOR,
        /// <summary>
        /// 
        /// </summary>
        POPUP_ALM,
        /// <summary>
        /// 
        /// </summary>
        POPUP_CLR,
    }
}
