using System;
using System.Collections.Generic;

/// <summary>
/// 事件委托
/// </summary>
/// <param name="e">事件参数基类</param>
public delegate void EventFunction(EventArgsBase e);

/// <summary>
/// 事件管理器
/// </summary>
public class EventManager : SingletonBase<EventManager>
{
    // 存储委托的字典
    Dictionary<int, EventFunction> handlerDic = new Dictionary<int, EventFunction>();

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="handler">绑定的方法</param>
    public void AddListener(EventName eventName, EventFunction handler)
    {
        // 枚举转 int
        int index = eventName.ToInt();
        if (handlerDic.ContainsKey(index))
        {
            handlerDic[index] += handler;
        }
        else
        {
            handlerDic.Add(index, handler);
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="handler">绑定的方法</param>
    public void RemoveListener(EventName eventName, EventFunction handler)
    {
        // 枚举转 int
        int index = eventName.ToInt();
        if (handlerDic.ContainsKey(index))
        {
            handlerDic[index] -= handler;
        }
    }

    /// <summary>
    /// 触发事件（无参数）
    /// </summary>
    /// <param name="eventName">事件名</param>
    public void TriggerEvent(EventName eventName)
    {
        // 枚举转 int
        int index = eventName.ToInt();
        if (handlerDic.ContainsKey(index))
        {
            handlerDic[index]?.Invoke(EventArgsBase.Empty);
        }
    }

    /// <summary>
    /// 触发事件（有参数）
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="args">参数类实例</param>
    public void TriggerEvent(EventName eventName, EventArgsBase args)
    {
        // 枚举转 int
        int index = eventName.ToInt();
        if (handlerDic.ContainsKey(index))
        {
            handlerDic[index]?.Invoke(args);
        }
    }

    /// <summary>
    /// 清空存储事件的字典
    /// </summary>
    public void Clear()
    {
        handlerDic.Clear();
    }
}

/// <summary>
/// 事件参数基类
/// </summary>
public class EventArgsBase
{
    // 空参数
    public static readonly EventArgsBase Empty;
}

/// <summary>
/// 事件触发扩展，调用简洁
/// </summary>
public static class EventTriggerEvent
{
    public static void TriggerEvent(this object sender, EventName eventName)
    {
        EventManager.Instance.TriggerEvent(eventName);
    }

    public static void TriggerEvent(this object sender, EventName eventName, EventArgsBase args)
    {
        EventManager.Instance.TriggerEvent(eventName, args);
    }
}

/// <summary>
/// 枚举转 int 扩展
/// </summary>
public static class ExtendEnum
{
    public static int ToInt(this Enum e)
    {
        return e.GetHashCode();
    }
}