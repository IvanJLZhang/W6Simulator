#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Statement
// Author: Ivan JL Zhang    Date: 2018/5/23 12:28:17    Version: 1.0.0
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

namespace Granda.GAEE.Core.Normal
{
    /// <summary>
    /// 基本的单条语句（以后可在需要情况下扩展为多条语句）
    /// </summary>
    public class Statement : Step
    {
        private StatementExpression statementExpression = null;
        public override void Run()
        {
            statementExpression = stepSetting as StatementExpression;
            if (String.IsNullOrEmpty(statementExpression.Expression))
            {
                throw new ArgumentNullException(nameof(statementExpression.Expression));
            }
            // 分析表达式
            // 分析逻辑表达式的情况
            var expressions = statementExpression.Expression.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            string newExpression = statementExpression.Expression;
            if (expressions.Length == 2)
            {
                var expre_left = expressions[0];
                var expre_right = expressions[1];
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

                    MyEvaluator myEvaluator = new MyEvaluator(new EvaluatorItem(variable.VariableType, expre_right, "_"));
                    variable.Value = myEvaluator.Evaluate("_");
                }
            }
        }

        public class StatementExpression : IStepSetting
        {
            public string Expression { get; set; }
        }
    }
}
