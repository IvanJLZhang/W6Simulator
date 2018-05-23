#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Step
// Author: Ivan JL Zhang    Date: 2018/5/23 11:51:35    Version: 1.0.0
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
using Granda.GAEE.Core.StepSettings;

namespace Granda.GAEE.Core
{
    public abstract class Step : IStep
    {
        /// <summary>
        /// Step name
        /// </summary>
        public string Name { get; set; }

        public StepType stepType { get; set; }

        public IStepSetting stepSetting { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 补充说明
        /// </summary>
        public string Comment { get; set; }

        public abstract void Run();
    }
}
