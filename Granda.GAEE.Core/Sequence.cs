#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Sequence
// Author: Ivan JL Zhang    Date: 2018/5/28 14:35:16    Version: 1.0.0
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

namespace Granda.GAEE.Core
{
    /// <summary>
    /// 相当于类， 完成一个特定功能的Step集合
    /// </summary>
    public class Sequence : Step
    {
        /// <summary>
        /// 启动主函数之前的设定step集合
        /// </summary>
        public IList<IStep> Setup { get; set; }
        /// <summary>
        /// 执行主函数
        /// </summary>
        public IList<IStep> Main { get; set; }
        /// <summary>
        /// 执行结束后的动作
        /// </summary>
        public IList<IStep> Cleanup { get; set; }


        public override void Run()
        {
            for (int index = 0; index < Setup.Count; index++)
            {
                Setup[index].Run();
            }

            for (int index = 0; index < Main.Count; index++)
            {
                Main[index].Run();
            }

            for (int index = 0; index < Cleanup.Count; index++)
            {
                Cleanup[index].Run();
            }
        }
    }
}
