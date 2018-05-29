#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: IZ_WHILE
// Author: Ivan JL Zhang    Date: 2018/5/24 17:42:52    Version: 1.0.0
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
using Granda.GAEE.Core.CodeDom;
using Granda.GAEE.Core.StepSettings;

namespace Granda.GAEE.Core.FlowControl
{
    public class IZ_WHILE : Step
    {
        public bool ConditionResult { get; set; } = false;
        public IList<IStep> statements = null;

        public WhileCondition whileCondition { get; set; }
        private bool ExecuteExpression()
        {
            if (String.IsNullOrEmpty(whileCondition.Expression))
            {
                throw new ArgumentNullException(nameof(whileCondition.Expression));
            }
            // 分析表达式
            // 分析逻辑表达式的情况
            var expressions = whileCondition.Expression.Split(new string[] { ">", "<", "==", ">=", "<=", "!=" }, StringSplitOptions.RemoveEmptyEntries);
            string newExpression = whileCondition.Expression;
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
            whileCondition = stepSetting as WhileCondition;
            while (ExecuteExpression() && statements != null)
            {
                foreach (var item in statements)
                {
                    item.Run();
                }
            }
        }

        public class WhileCondition : IStepSetting
        {
            public string Expression { get; set; }

        }
    }
}
