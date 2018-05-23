#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Wait
// Author: Ivan JL Zhang    Date: 2018/5/23 12:04:40    Version: 1.0.0
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
using System.Threading;
using Granda.GAEE.Core.StepSettings;

namespace Granda.GAEE.Core.Synchronization
{
    /// <summary>
    /// Wait 定时器， 让线程挂起指定时间
    /// </summary>
    public class Wait : Step
    {
        public Wait()
        {
            base.Name = "Wait";
            base.stepType = StepType.Wait;
        }
        public WaitSettings waitSettings { get; set; }

        public override void Run()
        {
            Thread.Sleep(waitSettings.Time_Interval);
        }

        public class WaitSettings : IStepSetting
        {
            public string Decsription { get; set; }
            public int Time_Interval { get; set; }
            public string Comment { get; set; }
        }
    }
}
