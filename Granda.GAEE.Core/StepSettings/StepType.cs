#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: StepType
// Author: Ivan JL Zhang    Date: 2018/5/23 11:51:58    Version: 1.0.0
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

namespace Granda.GAEE.Core.StepSettings
{
    public enum StepType
    {
        Action,
        MessagePopup,
        Statement,

        #region Synchronization
        Wait,
        #endregion

        #region Granda.GAEE.FlowControl
        If,
        Else,
        For,
        While,
        End,
        #endregion
    }
}
