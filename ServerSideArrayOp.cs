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
    [Icon("pack://application:,,,/CollectionOperationKit;component/Resources/Icon.png")]
    public class ServerSideArrayOp : Command, ICommandExecutableInServerSide
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
                    return "数组操作"; // 命令列表中默认显示的名称
                }
                else
                {
                    return "创建数组： " + OutParamaterName;
                }

            }
            else
            {
                if (String.IsNullOrEmpty(OutParamaterName))
                {
                    return "数组操作（" + Operation.ToString() + "）";
                }
                else
                {
                    return "数组操作（" + Operation.ToString() + "）： " + OutParamaterName;
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

        private ArrayList getArrayListParam(IServerCommandExecuteContext dataContext, object formula)
        {

            var input = getParamValue(dataContext, formula);

            // 输入参数仅支持ArrayList
            if (input.GetType() != typeof(ArrayList))
            {
                throw new ArgumentException("[" + formula + "]'s type was " + input.GetType().ToString() + ", should be an ArrayList. Array can be converted using [FormArray] operation.");
            }

            return (ArrayList)input;

        }

        private object getParamValue(IServerCommandExecuteContext dataContext, object formula)
        {
            var task = dataContext.EvaluateFormulaAsync(formula);
            task.Wait();
            return task.Result;
        }


        private void returnToParam(IServerCommandExecuteContext dataContext, object data)
        {
            // 只有设置了返回参数，才能执行参数写入，避免出错
            if (!string.IsNullOrEmpty(OutParamaterName))
            {
                dataContext.Parameters[OutParamaterName] = data;
            }
        }

        public ExecuteResult Execute(IServerCommandExecuteContext dataContext)
        {
            switch (this.Operation)
            {
                case SupportedOperations.FromArray:
                    {
                        var al = new ArrayList();
                        var rawData = getParamValue(dataContext, this.OperationParamaterAName);
                        if (rawData != null)
                        {
                            if (rawData.GetType() == typeof(Array))
                            {
                                al.AddRange((Array)rawData);
                            }
                            else
                            {
                                al.Add(rawData);
                            }
                        }

                        dataContext.Parameters[OutParamaterName] = al;

                        break;
                    }
                case SupportedOperations.ToArray:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);

                        returnToParam(dataContext, data.ToArray());
                        break;
                    }
                case SupportedOperations.Create:
                    {
                        returnToParam(dataContext, new ArrayList());
                        break;
                    }
                case SupportedOperations.Set:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        int index = int.Parse(getParamValue(dataContext, this.OperationParamaterAName).ToString());
                        data[index] = getParamValue(dataContext, this.OperationParamaterBName);
                        returnToParam(dataContext, data);
                        break;
                    }
                case SupportedOperations.Get:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        int index = int.Parse(getParamValue(dataContext, this.OperationParamaterAName).ToString());

                        returnToParam(dataContext, data[index]);
                        break;
                    }
                case SupportedOperations.Length:
                    {

                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        returnToParam(dataContext, data.Count);
                        break;
                    }
                case SupportedOperations.Push:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        data.Add(getParamValue(dataContext, this.OperationParamaterAName));
                        returnToParam(dataContext, data);
                        break;
                    }
                case SupportedOperations.Pop:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        var last = data[data.Count - 1];
                        data.RemoveAt(data.Count - 1);
                        returnToParam(dataContext, last);
                        break;
                    }
                case SupportedOperations.Unshift:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        data.Insert(0, getParamValue(dataContext, this.OperationParamaterAName));
                        returnToParam(dataContext, data);
                        break;
                    }
                case SupportedOperations.Shift:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        var first = data[0];
                        data.RemoveAt(0);
                        returnToParam(dataContext, first);
                        break;
                    }
                case SupportedOperations.Concat:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        ArrayList subData = getArrayListParam(dataContext, this.OperationParamaterAName);
                        var result = (ArrayList)data.Clone(); // 行为与JS Array的conact行为一致，不能影响原有数组
                        result.AddRange(subData);
                        returnToParam(dataContext, result);
                        break;
                    }
                case SupportedOperations.Slice:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        int start = int.Parse(getParamValue(dataContext, this.OperationParamaterAName).ToString());
                        int end = int.Parse(getParamValue(dataContext, this.OperationParamaterBName).ToString());
                        returnToParam(dataContext, data.GetRange(start, end - start));
                        break;
                    }
                case SupportedOperations.InsertRange:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        ArrayList subData = getArrayListParam(dataContext, this.OperationParamaterAName);
                        int index = int.Parse(getParamValue(dataContext, this.OperationParamaterBName).ToString());
                        data.InsertRange(index, subData);
                        returnToParam(dataContext, data);
                        break;
                    }
                case SupportedOperations.RemoveRange:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        int index = int.Parse(getParamValue(dataContext, this.OperationParamaterBName).ToString());
                        int count = int.Parse(getParamValue(dataContext, this.OperationParamaterAName).ToString());

                        Array sub = data.GetRange(index, count).ToArray();
                        data.RemoveRange(index, count);

                        returnToParam(dataContext, sub);
                        break;
                    }
                case SupportedOperations.IndexOf:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        int index = data.IndexOf(getParamValue(dataContext, this.OperationParamaterAName));
                        returnToParam(dataContext, index);
                        break;
                    }
                case SupportedOperations.LastIndexOf:
                    {
                        ArrayList data = getArrayListParam(dataContext, this.InParamaterName);
                        int index = data.LastIndexOf(getParamValue(dataContext, this.OperationParamaterAName));
                        returnToParam(dataContext, index);
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
        public object InParamaterName { get; set; }

        [DisplayName("操作参数A")]
        [FormulaProperty]
        public object OperationParamaterAName { get; set; }

        [DisplayName("操作参数B")]
        [FormulaProperty]
        public object OperationParamaterBName { get; set; }

        [DisplayName("将结果返回到变量")]
        [ResultToProperty]
        public String OutParamaterName { get; set; }


        public enum SupportedOperations
        {
            [Description("创建一个空的高级数组")]
            Create,
            [Description("Set：将【输入参数】中索引为【操作参数A】的元素设置为【操作参数B】")]
            Set,
            [Description("Get：返回【输入参数】中索引为【操作参数A】的元素")]
            Get,
            [Description("Length：返回【输入参数】的长度")]
            Length,
            [Description("Push：将【操作参数A】添加到【输入参数】的结尾处")]
            Push,
            [Description("Pop：返回【输入参数】的最后一个元素，并将其移除")]
            Pop,
            [Description("Unshift：将【操作参数A】添加到【输入参数】的开头")]
            Unshift,
            [Description("Shift：返回【输入参数】的第一个元素，并将其移除")]
            Shift,
            [Description("Concat：返回一个新数组，顺次包含【输入参数】和【操作参数A】的所有元素")]
            Concat,
            [Description("Slice：返回【输入参数】中从【操作参数A】开始到【操作参数B】之前的元素")]
            Slice,
            [Description("InsertRange：类似splice，将【操作参数A】的所有元素添加到【输入参数】的【操作参数B】位置")]
            InsertRange,
            [Description("RemoveRange：类似splice，返回【输入参数】中从【操作参数A】位置开始的【操作参数B】个元素，并将其移除")]
            RemoveRange,
            [Description("IndexOf：返回【操作参数A】在【输入参数】中第一次出现的位置")]
            IndexOf,
            [Description("LastIndexOf：返回【操作参数A】在【输入参数】中最后一次出现的位置")]
            LastIndexOf,
            [Description("FromArray：使用【操作参数A】创建高级数组并返回")]
            FromArray,
            [Description("ToArray：将【输入参数】转换为数组并返回")]
            ToArray
        }
    }
}
