using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace CollectionOperationKit
{
    [Icon("pack://application:,,,/CollectionOperationKit;component/Resources/Icon.png")]
    public class ClientSideArrayOp : Command 
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
                    return "创建数组: " + OutParamaterName;
                }

            }
            else
            {
                if (String.IsNullOrEmpty(OutParamaterName))
                {
                    return "数组操作（" + Operation.ToString() + "）" ;
                }
                else {
                    return "数组操作（" + Operation.ToString() + "）: " + OutParamaterName;
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
            [Description("创建一个数组")]
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
            [Description("Parse：使用JSON字符串类型的【操作参数A】生成高级数组并返回")]
            FromJSON,
            [Description("Stringify：将【输入参数】转换为JSON字符串并返回")]
            ToJSON,
        }
    }
}
