#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: IZ_FOR
// Author: Ivan JL Zhang    Date: 2018/5/28 14:00:45    Version: 1.0.0
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

namespace Granda.GAEE.Core.FlowControl
{
    public class IZ_FOR : Step
    {
        public ForLoop forLoop = null;

        public IList<IStep> statements = null;

        public IZ_FOR(int numberOfInterations)
        {
            if (Locals["Index"] == null)
                Locals.Add(new Variables.LocalsVariable { Name = "Index", VariableType = typeof(Int32), Value = 0 });
        }

        public override void Run()
        {
            forLoop = stepSetting as ForLoop;
            if (forLoop == null)
                throw new ArgumentNullException(nameof(forLoop.NumberOfInterations));
            for (int index = 0; index < forLoop.NumberOfInterations; index++)
            {
                Locals["Index"].Value = index;
                if (statements != null)
                {
                    foreach (var item in statements)
                    {
                        item.Run();
                    }
                }
            }
        }

        public class ForLoop : IStepSetting
        {
            public int NumberOfInterations = 0;
        }
    }
}
