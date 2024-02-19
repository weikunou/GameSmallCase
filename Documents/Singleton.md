泛型单例

- SingletonBase（C# 单例）
- SingletonMono（Unity 手动挂载单例）
- SingletonMonoAuto（Unity 自动挂载单例）



使用时，继承泛型单例基类，泛型要填入派生类的类型。

```c#
public class BaseManager : SingletonBase<BaseManager>
{
    
}
```



Unity 手动挂载单例，需要在场景中创建一个 GameObject，添加脚本组件。

```c#
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
        
    }

    void Update()
    {
        
    }
}
```



Unity 自动挂载单例，无需创建 GameObject，调用时会自动创建。

```c#
public class MonoAutoManager : SingletonMonoAuto<MonoAutoManager>
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
```

