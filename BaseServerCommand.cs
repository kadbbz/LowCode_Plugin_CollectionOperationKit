﻿using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionOperationKit
{
    public abstract class BaseServerCommand : Command
    {
        [OrderWeight(10)]
        [DisplayName("输入参数")]
        [FormulaProperty]
        [Description("操作可能不会影响【输入参数】中用到的变量，如需对变量进行修改，可将其填写到【将结果返回到变量】。")]
        public object InParamater { get; set; }

        [OrderWeight(999)]
        [DisplayName("将结果返回到变量")]
        [ResultToProperty]
        public String OutParamaterName { get; set; }


        /// <summary>
        /// 插件类型：设置为服务端命令插件
        /// </summary>
        /// <returns>插件类型枚举</returns>
        public override CommandScope GetCommandScope()
        {
            return CommandScope.ServerSide;
        }


        protected object getParamValue(IServerCommandExecuteContext dataContext, object formula, bool shouldCheckNull = true)
        {
            if (formula == null)
            {
                throw new ArgumentException("The parameter/formula was not set.");
            }

            var task = dataContext.EvaluateFormulaAsync(formula);
            task.Wait();

            if (task.Result == null && shouldCheckNull)
            {
                throw new ArgumentException("[" + formula.ToString() + "] is null.");
            }

            return task.Result;
        }

        protected void returnToParam(IServerCommandExecuteContext dataContext, object data)
        {

            if (!string.IsNullOrEmpty(OutParamaterName))
            {
                dataContext.Parameters[OutParamaterName] = data;
            }
        }
    }
}
