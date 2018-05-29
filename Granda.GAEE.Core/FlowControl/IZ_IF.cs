#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: IZ_IF
// Author: Ivan JL Zhang    Date: 2018/5/24 12:30:01    Version: 1.0.0
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
using System.Text.RegularExpressions;
using Granda.GAEE.Core.CodeDom;
using Granda.GAEE.Core.Normal;
using Granda.GAEE.Core.StepSettings;

namespace Granda.GAEE.Core.FlowControl
{
    class IZ_IF : Step
    {
        public bool ConditionResult { get; set; } = false;
        public IList<IStep> statements = null;
        public IfCondition ifCondition { get; set; }
        private bool ExecuteExpression()
        {
            if (String.IsNullOrEmpty(ifCondition.Expression))
            {
                throw new ArgumentNullException(nameof(ifCondition.Expression));
            }
            // 分析表达式
            // 分析逻辑表达式的情况
            var expressions = ifCondition.Expression.Split(new string[] { ">", "<", "==", ">=", "<=", "!=" }, StringSplitOptions.RemoveEmptyEntries);
            string newExpression = ifCondition.Expression;
            if (expressions.Length == 2)
            {
                var expre_left = expressions[0];
                var key_value = expre_left.Split('.');
                if (key_value.Length == 2)
                {
                    Variable variable = null;
                    switch (key_value[0])
                    {
                        case "Locals":
                            variable = Locals[key_value[1]];
                            break;
                        case "Parameters":
                            variable = Parameters[key_value[1]];
                            break;
                        default:
                            break;
                    }
                    if (variable == null)
                        throw new ArgumentNullException(expre_left);
                    switch (variable.VariableType.Name)
                    {
                        case nameof(Int32):
                            newExpression = newExpression.Replace(expre_left, variable.GetValue<Int32>().ToString());
                            break;
                        case nameof(String):
                            newExpression = newExpression.Replace(expre_left, "\"" + variable.GetValueString() + "\"");
                            break;
                        case nameof(Boolean):
                            newExpression = newExpression.Replace(expre_left, variable.GetValue<Boolean>().ToString());
                            break;
                        default:
                            break;
                    }
                }
            }
            return MyEvaluator.EvaluateToBool(newExpression);
        }

        public override void Run()
        {
            ifCondition = stepSetting as IfCondition;
            ConditionResult = ExecuteExpression();// 执行表达式结果
            if (ConditionResult && statements != null)
            {
                foreach (var item in statements)
                {
                    item.Run();
                }
            }
        }

        public class IfCondition : IStepSetting
        {
            public string Expression { get; set; }

        }
    }
}
