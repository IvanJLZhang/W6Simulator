#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Model_Info
// Author: Ivan JL Zhang    Date: 2018/5/16 17:08:45    Version: 1.0.0
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
    public struct Model_Info
    {
        /// <summary>
        /// 驱动器档案信息 IMPORT Info
        /// </summary>
        public String IMPORT_INFO { get; set; }
        /// <summary>
        /// 驱动器档案信息： Model Info
        /// </summary>
        public String MODEL_INFO { get; set; }
        /// <summary>
        /// 驱动器档案信息： Scplist info
        /// </summary>
        public String SCPLIST_INFO { get; set; }
        /// <summary>
        /// 模块版本： device info
        /// </summary>
        public String DEVICE_INFO { get; set; }
        /// <summary>
        /// 模块版本： kernel info
        /// </summary>
        public String KERNEL_INFO { get; set; }
        /// <summary>
        /// 网络信息： network info
        /// </summary>
        public String NETWORK_INFO { get; set; }

    }
}
