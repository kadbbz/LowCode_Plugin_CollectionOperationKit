﻿using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace CollectionOperationKit
{
    [Icon("pack://application:,,,/CollectionOperationKit;component/Resources/ObjectIcon.png")]
    [Category("对象与集合操作")]
    public class ServerSideObjectOp : Command, ICommandExecutableInServerSide
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
                    return "对象操作";
                }
                else
                {
                    return "创建对象： " + OutParamaterName;
                }

            }
            else
            {
                if (String.IsNullOrEmpty(OutParamaterName))
                {
                    return "对象操作（" + Operation.ToString() + "）";
                }
                else
                {
                    return "对象操作（" + Operation.ToString() + "）： " + OutParamaterName;
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

                case SupportedOperations.Create:
                    {
                        returnToParam(dataContext, new Dictionary<string, object>());
                        break;
                    }
                case SupportedOperations.Properties:
                    {

                        var input = getParamValue(dataContext, this.InParamater);

                        if (input is IDictionary<string, object> dic)
                        {
                            // 使用命令创建的，以及设置过属性的
                            string[] props = new string[dic.Count];
                            dic.Keys.CopyTo(props, 0);
                            returnToParam(dataContext, props);
                        }
                        else if (input is JObject jObj)
                        {
                            // 刚从JSON反序列化过来的
                            List<string> propl = new List<string>();
                            foreach (JProperty pop in jObj.Properties())
                            {
                                propl.Add(pop.Name);
                            }
                            returnToParam(dataContext, propl.ToArray());
                        }
                        else
                        {
                            // 系统内置的，仅获取公有的实例属性，避免后面设置属性时出错
                            var props = input.GetType().GetProperties(System.Reflection.BindingFlags.Public|System.Reflection.BindingFlags.Instance);

                            List<string> propl = new List<string>();

                            foreach (PropertyInfo pop in props)
                            {
                                propl.Add(pop.Name);
                            }

                            returnToParam(dataContext, propl.ToArray());
                        }

                        break;
                    }
                case SupportedOperations.GetPropertyValue:
                    {

                        var input = getParamValue(dataContext, this.InParamater);
                        var name = getParamValue(dataContext, this.OperationParamaterName).ToString();
                        var value = getParamValue(dataContext, this.OperationParamaterValue);

                        if (input is IDictionary<string, object> dic)
                        {
                            if (dic.ContainsKey(name))
                            {
                                returnToParam(dataContext, dic[name]);
                            }
                            else
                            {
                                returnToParam(dataContext, value);
                            }

                        }
                        else if (input is JObject jObj)
                        {
                            JProperty prop = jObj.Property(name);

                            if (null == prop)
                            {
                                returnToParam(dataContext, value);
                            }
                            else
                            {
                                returnToParam(dataContext, prop.Value.ToObject<object>());
                            }
                        }
                        else
                        {
                            var prop = input.GetType().GetProperty(name);
                            if (prop == null)
                            {
                                returnToParam(dataContext, value);
                            }
                            else
                            {
                                returnToParam(dataContext, prop.GetValue(input));
                            }
                        }

                        break;
                    }
                case SupportedOperations.SetPropertyValue:
                    {

                        var input = getParamValue(dataContext, this.InParamater);
                        var name = getParamValue(dataContext, this.OperationParamaterName).ToString();
                        var value = getParamValue(dataContext, this.OperationParamaterValue);

                        if (input is IDictionary<string, object> dic)
                        {
                            // 使用命令创建的，直接写入
                            if (dic.ContainsKey(name))
                            {
                                dic[name] = value;
                            }
                            else
                            {
                                dic.Add(name, value);
                            }

                            returnToParam(dataContext, input);

                        }
                        else if (input is JObject jObj)
                        {
                            // 从JSON反序列化回来的，先转成和使用命令创建的一样的类型
                            var inputObj = jObj.ToDictionary();

                            if (inputObj.ContainsKey(name))
                            {
                                inputObj[name] = value;
                            }
                            else
                            {
                                inputObj.Add(name, value);
                            }

                            returnToParam(dataContext, inputObj);
                        }
                        else
                        {
                            // 内置类型不支持增加属性，但可以尝试写入
                            var prop = input.GetType().GetProperty(name);
                            if (prop == null)
                            {
                                throw new NotSupportedException("AppendProperty is NOT supported for the [" + InParamater + "], it's neither a Dictionary<string,object> created by Huozige command or a JObject deserialized from JSON.");
                            }
                            else
                            {
                                prop.SetValue(input, value);
                            }

                            returnToParam(dataContext, input);
                        }



                        break;
                    }
                case SupportedOperations.Null:
                    {

                        returnToParam(dataContext, null);

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

        [DisplayName("属性名")]
        [FormulaProperty]
        public object OperationParamaterName { get; set; }

        [DisplayName("属性值")]
        [FormulaProperty]
        public object OperationParamaterValue { get; set; }

        [DisplayName("将结果返回到变量")]
        [ResultToProperty]
        public String OutParamaterName { get; set; }


        public enum SupportedOperations
        {
            [Description("创建一个新对象")]
            Create,
            [Description("Properties：返回【输入参数】中的所有属性名")]
            Properties,
            [Description("GetPropertyValue：返回【输入参数】中名为【属性名】的属性值，如果没有该属性，则返回【属性值】的值")]
            GetPropertyValue,
            [Description("SetPropertyValue：将【输入参数】中名为【属性名】的属性值设置为【属性值】并返回")]
            SetPropertyValue,
            [Description("Null：返回%Null%")]
            Null
        }
    }
}
