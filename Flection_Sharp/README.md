# Flection_sharp

## 简介

Flection_sharp 是一个基于.net stranded 2.1的Flection库的C#封装。

## 安装

```
Install-Package Flection_sharp
```

## 使用

```cs
using Flection_sharp;

# 获取所有命名空间
flection.getAll();

# 获取命名空间下的所有类型
flection.getAllTypes()

# 获取指定命名空间下的第一个类型
flection.getSingleType();

# 创建一个实例，并且给这个类赋值
flection.createAndAssignment();

# 将dynamic对象的值，赋值给指定类型的实例中；
flection.dynamicToObject();
```

### object拓展方法

```cs
# 获取指定属性的值
object.GetValue();

# 给指定的属性设置值
object.SetValue();

# 获取所有属性
object.GetProperties();
```