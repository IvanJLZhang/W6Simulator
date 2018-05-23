#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: W6FILE
// Author: Ivan JL Zhang    Date: 2018/5/22 14:22:37    Version: 1.0.0
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
    public struct W6_FILE_PATH
    {
        public string WORK_PATH { get; set; }
        public string BOOT_PATH { get; set; }
        public string UPDATE_PATH { get; set; }
        public string LOG_PATH { get; set; }
        public string MODEL_PATH { get; set; }
        public string WPM_PATH { get; set; }
        public string TEMP_PATH { get; set; }
    }
}
