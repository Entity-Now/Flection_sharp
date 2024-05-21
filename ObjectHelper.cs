using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Flection_Sharp
{
    public static class ObjectHelper
    {
        /// <summary>
        /// 获取指定属性的值
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetValue(this object? _this, string name)
        {
            return _this?.GetType()?.GetProperty(name)?.GetValue(_this);
        }
        /// <summary>
        /// 给指定的属性设置值
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetValue(this object? _this, string name, object value)
        {
            _this.GetType()?.GetProperty(name)?.SetValue(_this, value);
        }

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static PropertyInfo[] GetProperties(this object? _this)
        {
            return _this.GetType().GetProperties();
        }
    }
}
