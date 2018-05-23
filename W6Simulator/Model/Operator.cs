#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Operator
// Author: Ivan JL Zhang    Date: 2018/5/15 10:43:22    Version: 1.0.0
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

namespace W6Simulator.Model
{
    /// <summary>
    /// 支持的运算符种类
    /// </summary>
    public enum Operator
    {
        NULL,
        /// <summary>
        /// if 运算符
        /// </summary>
        IF,
        /// <summary>
        /// else 运算符，必需与if配套使用
        /// </summary>
        ELSE,
        /// <summary>
        /// for 运算符
        /// </summary>
        FOR,
        /// <summary>
        /// while 运算符
        /// </summary>
        WHILE,
        /// <summary>
        /// 等待运算
        /// </summary>
        WAIT,
        /// <summary>
        /// 结束运算符，必需与IF配套使用
        /// </summary>
        END
    }
}
