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
    public class ClientSideStringMapOp : Command
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
                    return "字典操作"; // 命令列表中默认显示的名称
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
            return CommandScope.ClientSide;
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
