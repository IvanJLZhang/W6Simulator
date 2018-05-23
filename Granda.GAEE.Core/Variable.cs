#region 文件说明
/*------------------------------------------------------------------------------
// Copyright © 2018 Granda. All Rights Reserved.
// 苏州广林达电子科技有限公司 版权所有
//------------------------------------------------------------------------------
// File Name: Variable
// Author: Ivan JL Zhang    Date: 2018/5/23 11:12:36    Version: 1.0.0
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
    /// 变量定义
    /// </summary>
    public class Variable
    {
        public String Name { get; set; }
        public Object Value { get; set; }
        public Type VariableType { get; set; }

        public T GetValue<T>() where T : struct
        {
            return (T)Value;
        }

        public bool GetValueBool()
        {
            if (typeof(bool).Equals(VariableType))
                return (bool)Value;
            else
                throw new InvalidCastException("Wrong variable type");
        }

        public Int32 GetValueInt()
        {
            if (typeof(Int32).Equals(VariableType))
                return (Int32)Value;
            else
                throw new InvalidCastException("Wrong variable type");
        }

        public String GetValueString()
        {
            if (typeof(String).Equals(VariableType))
                return (String)Value;
            else
                throw new InvalidCastException("Wrong variable type");
        }
    }
    /// <summary>
    /// 变量类型
    /// </summary>
    public enum VariableType
    {
        Boolean,
        Int,
        String,
    }
}
