using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity 单例（自动创建）
/// </summary>
/// <typeparam name="T">类</typeparam>
public class SingletonMonoAuto<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;  // 私有静态实例

    /// <summary>
    /// 实例属性
    /// </summary>
    /// <value>私有静态实例</value>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                // 创建一个新的游戏物体
                GameObject obj = new GameObject();
                // 根据类型进行重命名
                obj.name = typeof(T).ToString();
                // 不可销毁
                DontDestroyOnLoad(obj);
                // 自动挂载单例组件
                instance = obj.AddComponent<T>();
            }
            // 返回实例
            return instance;
        }
    }
}
