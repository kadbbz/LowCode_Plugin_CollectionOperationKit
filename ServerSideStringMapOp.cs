using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace CollectionOperationKit
{
    [Icon("pack://application:,,,/CollectionOperationKit;component/Resources/DictIcon.png")]
    [Category("对象与集合操作")]
    public class ServerSideStringMapOp : Command, ICommandExecutableInServerSide
    {

        /// <summary>
        /// 在设计器中展示的插件名称
        /// </summary>
        /// <returns>易读的字符串</returns>
        public override string ToString()
        {
            if (Operation == SupportedOperations.Create)
            {
                if (String.IsNullOrEmpty(OutParamaterName))
                {
                    return "字典操作";
                }
                else
                {
                    return "创建字典： " + OutParamaterName;
                }

            }
            else
            {
                if (String.IsNullOrEmpty(OutParamaterName))
                {
                    return "字典操作（" + Operation.ToString() + "）";
                }
                else
                {
                    return "字典操作（" + Operation.ToString() + "）： " + OutParamaterName;
                }

            }

        }

        /// <summary>
        /// 插件类型：设置为服务端命令插件
        /// </summary>
        /// <returns>插件类型枚举</returns>
        public override CommandScope GetCommandScope()
        {
            return CommandScope.ServerSide;
        }

        private Dictionary<string, object> getDictionaryParam(IServerCommandExecuteContext dataContext, object formula)
        {

            var input = getParamValue(dataContext, formula);

            if (input.GetType() != typeof(Dictionary<string, object>))
            {
                throw new ArgumentException("[" + formula + "]'s type was " + input.GetType().ToString() + ", should be a Dictionary<string,object>.");
            }

            return (Dictionary<string, object>)input;

        }

        private object getParamValue(IServerCommandExecuteContext dataContext, object formula)
        {

            var task = dataContext.EvaluateFormulaAsync(formula);
            task.Wait();
            return task.Result;
        }

        private void returnToParam(IServerCommandExecuteContext dataContext, object data)
        {

            if (!string.IsNullOrEmpty(OutParamaterName))
            {
                dataContext.Parameters[OutParamaterName] = data;
            }
        }

        public ExecuteResult Execute(IServerCommandExecuteContext dataContext)
        {
            switch (this.Operation)
            {

                case SupportedOperations.Create:
                    {
                        returnToParam(dataContext, new Dictionary<string, object>());
                        break;
                    }
                case SupportedOperations.Set:
                    {
                        var data = getDictionaryParam(dataContext, InParamater);
                        var key = getParamValue(dataContext, this.OperationParamaterKey).ToString();

                        if (data.ContainsKey(key))
                        {
                            data[key] = getParamValue(dataContext, this.OperationParamaterValue);
                        }
                        else
                        {
                            data.Add(key, getParamValue(dataContext, this.OperationParamaterValue));
                        }

                        returnToParam(dataContext, data);
                        break;
                    }

                case SupportedOperations.Keys:
                    {
                        var data = getDictionaryParam(dataContext, InParamater);
                        string[] array = new string[data.Count];
                        data.Keys.CopyTo(array, 0);

                        returnToParam(dataContext, array);
                        break;
                    }
                case SupportedOperations.Values:
                    {
                        var data = getDictionaryParam(dataContext, InParamater);
                        string[] array = new string[data.Count];
                        data.Values.CopyTo(array, 0);

                        returnToParam(dataContext, array);
                        break;
                    }
                case SupportedOperations.Has:
                    {
                        var data = getDictionaryParam(dataContext, InParamater);
                        var key = getParamValue(dataContext, this.OperationParamaterKey).ToString();

                        if (data.ContainsKey(key))
                        {
                            returnToParam(dataContext, 1);
                        }
                        else
                        {
                            returnToParam(dataContext, 0);
                        }

                        break;
                    }
                case SupportedOperations.Delete:
                    {
                        var data = getDictionaryParam(dataContext, InParamater);
                        var key = getParamValue(dataContext, this.OperationParamaterKey).ToString();

                        data.Remove(key);
                        returnToParam(dataContext, data);

                        break;
                    }
                case SupportedOperations.Clear:
                    {
                        var data = getDictionaryParam(dataContext, InParamater);

                        data.Clear();

                        returnToParam(dataContext, data);
                        break;
                    }
                case SupportedOperations.Size:
                    {
                        var data = getDictionaryParam(dataContext, InParamater);

                        returnToParam(dataContext, data.Count);
                        break;
                    }
                case SupportedOperations.Get:
                    {
                        var data = getDictionaryParam(dataContext, InParamater);
                        var key = getParamValue(dataContext, this.OperationParamaterKey).ToString();

                        if (data.ContainsKey(key))
                        {
                            returnToParam(dataContext, data[key]);
                        }
                        else
                        {
                            returnToParam(dataContext, getParamValue(dataContext, this.OperationParamaterValue));
                        }


                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return new ExecuteResult();

        }

        [DisplayName("操作")]
        [ComboProperty]
        [SearchableProperty]
        public SupportedOperations Operation { get; set; }

        [DisplayName("输入参数")]
        [FormulaProperty]
        public object InParamater { get; set; }

        [DisplayName("键（文本型）")]
        [FormulaProperty]
        public object OperationParamaterKey { get; set; }

        [DisplayName("值")]
        [FormulaProperty]
        public object OperationParamaterValue { get; set; }

        [DisplayName("将结果返回到变量")]
        [ResultToProperty]
        public String OutParamaterName { get; set; }


        public enum SupportedOperations
        {
            [Description("创建一个空的字典")]
            Create,
            [Description("Get：返回【输入参数】中键为【键（文本型）】的值，如果找不到，则返回【值】的值")]
            Get,
            [Description("Set：将【输入参数】中键为【键（文本型）】的值修改为【值】，如果不存在则创建新的键")]
            Set,
            [Description("Has：如果【输入参数】有【键（文本型）】的键，返回1，否则返回0")]
            Has,
            [Description("Delete：在【输入参数】中删除键为【键（文本型）】的元素")]
            Delete,
            [Description("Clear：清空【输入参数】中所有元素")]
            Clear,
            [Description("Size：返回【输入参数】中的元素数量")]
            Size,
            [Description("Keys：返回【输入参数】中的所有键")]
            Keys,
            [Description("Values：返回【输入参数】中的所有值")]
            Values
        }
    }
}
