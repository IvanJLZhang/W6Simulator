#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Statement
// Author: Ivan JL Zhang    Date: 2018/5/15 14:39:48    Version: 1.0.0
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
using W6Simulator.Model;

namespace W6Simulator.Command
{
    /// <summary>
    /// 执行语句结构体
    /// </summary>
    public struct Statement
    {
        /// <summary>
        /// 操作符
        /// </summary>
        public Operator Operator { get; set; }
        /// <summary>
        /// 判断条件
        /// </summary>
        public string Condition { get; set; }
        /// <summary>
        /// 执行语句列表
        /// </summary>
        public IList<string> StatementList { get; set; }
    }
}
