using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
                // 将 dynamic 转换为 JObject
                JObject jsonObject = JObject.FromObject(data);

                if (jsonObject == null)
                {
                    throw new Exception("传递的第二参数为空，请确认无误后重试！");
                }

                // 创建指定类型的实例
                var entity = Activator.CreateInstance(type);

                // 遍历 JObject 中的属性并赋值给实例
                foreach (var item in jsonObject.Properties())
                {
                    var property = type.GetProperty(item.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (property != null && property.CanWrite)
                    {
                        var value = item.Value.ToObject(property.PropertyType);
                        property.SetValue(entity, value);
                    }
                }
                return entity;
            }
            catch (Exception e)
            {
                throw new Exception($"Error: {e.Message}");
            }
        }

        public static dynamic InvokeGenericityFunc(this Type source, Type useType, object obj, string name, params object[] para)
        {
            // 获取所有符合方法名的重载方法
            var methods = source.GetMethods().Where(m => m.Name == name);

            // 获取与参数匹配的方法
            var method = methods.FirstOrDefault(m =>
            {
                var parameters = m.GetParameters();
                if (parameters.Length != para.Length)
                    return false;
                return true;
            });

            if (method == null)
            {
                throw new Exception("传入的方法名称在程序集中找不到！");
            }

            // 创建泛型方法
            var genericMethod = method.MakeGenericMethod(useType);
            var result = genericMethod.Invoke(obj, para);

            // 检查结果是否为Task并获取同步结果
            if (result is Task task)
            {
                // 等待任务完成
                task.Wait();

                // 如果任务是泛型任务，获取其结果
                var taskResultProperty = task.GetType().GetProperty("Result");
                return taskResultProperty?.GetValue(task);
            }

            return result;
        }


        /// <summary>
        /// 调用泛型方法
        /// </summary>
        /// <returns></returns>
        public static async Task<dynamic> InvokeGenericityFuncAsync(this Type source, Type useType, object obj, string name, params object[] para)
        {
            // 获取所有符合方法名的重载方法
            var methods = source.GetMethods().Where(m => m.Name == name);

            // 获取与参数匹配的方法
            var method = methods.FirstOrDefault(m =>
            {
                var parameters = m.GetParameters();
                if (parameters.Length != para.Length)
                    return false;
                return true;
            });

            if (method == null)
            {
                throw new Exception("传入的方法名称在程序集中找不到！");
            }

            // 创建泛型方法
            var genericMethod = method.MakeGenericMethod(useType);
            var result = genericMethod.Invoke(obj, para);

            // 检查结果是否为Task并进行异步等待
            if (result is Task task)
            {
                await task;
                return ((dynamic)task).Result;
            }
            return result;
        }
    }
}
