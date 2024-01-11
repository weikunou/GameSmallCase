using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : SingletonBase<BaseManager>
{
    public void Log()
    {
        Debug.Log($"C# 单例打印日志");
    }
}
