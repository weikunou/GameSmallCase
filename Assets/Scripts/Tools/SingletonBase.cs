/// <summary>
/// C# 单例
/// </summary>
/// <typeparam name="T">类</typeparam>
public class SingletonBase<T> where T : new()
{
    static T instance;  // 私有静态实例
    static readonly object locker = new object();  // 多线程加锁对象

    /// <summary>
    /// 实例属性
    /// </summary>
    /// <value>私有静态实例</value>
    public static T Instance
    {
        get
        {
            // 第一重判断
            if (instance == null)
            {
                // 多线程加锁
                lock (locker)
                {
                    // 第二重判断
                    if (instance == null)
                    {
                        // 创建实例
                        instance = new T();
                    }
                }
            }
            // 返回实例
            return instance;
        }
    }
}
