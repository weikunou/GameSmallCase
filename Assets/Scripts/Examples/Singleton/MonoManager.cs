using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoManager : SingletonMono<MonoManager>
{
    protected override void Awake()
    {
        base.Awake();
        // 如果所有 Unity 单例都是无法销毁的，可以把这一句放到基类
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log($"Unity 单例执行 Start 方法");
    }

    void Update()
    {
        
    }

    public void Log()
    {
        Debug.Log($"Unity 单例打印日志");
    }
}
