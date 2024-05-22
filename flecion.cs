using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Flection_Sharp
{
    public static class flecion
    {
        /// <summary>
        /// 获取所有命名空间
        /// </summary>
        /// <returns></returns>
        public static List<Assembly> getAll()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }
        /// <summary>
        /// 获取命名空间下的所有类型
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static List<Type> getAllTypes(string assemblyName)
        {
            var assembly = getAll().SingleOrDefault(x => x.FullName.Contains(assemblyName));
            if (assembly != null)
            {
                return assembly.GetTypes().ToList();
            }
            throw new Exception($"{assemblyName}命名空间不存在，请核对后重试~");
        }
        /// <summary>
        /// 获取指定命名空间下的第一个类型
        /// </summary>
        /// <param name="assemblyName">命名空间</param>
        /// <param name="typeName">类型名称</param>
        /// <returns></returns>
        public static Type getSingleType(string assemblyName, string typeName)
        {
            var ass = getAllTypes(assemblyName).SingleOrDefault(x => x.Name.Equals(typeName));

            return ass;
        }
        /// <summary>
        /// 创建一个实例，并且给这个类赋值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T createAndAssignment<T>(Type type, T data) where T : class
        {
            var entity = Activator.CreateInstance(type) as T;
            foreach (var item in data.GetProperties())
            {
                entity.SetValue(item.Name, data.GetValue(item.Name));
            }
            return entity;
        }
        /// <summary>
        /// 将dynamic对象的值，赋值给指定类型的实例中；
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static object dynamicToObject(Type type, dynamic data)
        {
            try
            {
                // 将json数据转换为实体
                var translate = ((JsonElement)data).Deserialize(type);
                if (translate == null)
                {
                    throw new Exception("传递的第二参数为空，请确认无误后重试！");
                }

                var entity = Activator.CreateInstance(type);
                foreach (var item in translate.GetProperties())
                {
                    var property = type.GetProperty(item.Name);
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(entity, translate.GetValue(property.Name));
                    }
                }
                return entity;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e.Message}");
            }
        }
    }
}
