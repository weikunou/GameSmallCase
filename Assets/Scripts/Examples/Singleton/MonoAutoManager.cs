using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoAutoManager : SingletonMonoAuto<MonoAutoManager>
{
    void Start()
    {
        Debug.Log($"Unity 自动挂载单例执行 Start 方法");
    }

    void Update()
    {
        
    }

    public void Log()
    {
        Debug.Log($"Unity 自动挂载单例打印日志");
    }
}
