using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity 单例（需要手动挂载）
/// </summary>
/// <typeparam name="T">类</typeparam>
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;  // 私有静态实例
    public static T Instance { get { return instance; } }  // 实例属性
    public static bool IsInitialized { get { return instance != null; } }  // 是否已经初始化属性，应对脚本执行顺序、未手动挂载问题

    protected virtual void Awake()
    {
        // 场景切换时，可能有多个单例游戏物体，如果已有单例，就销毁其他游戏物体
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this as T;
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
