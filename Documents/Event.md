事件系统

- EventManager（事件管理器单例）
- EventName（事件名枚举）
- CustomEventArgs（自定义事件参数类）



使用时，需要在 EventName.cs 中填写具体的事件名称，例如：

```c#
public enum EventName
{
    #region UI

    UI_Show_Bag,        // 显示背包界面

    #endregion
}
```



自定义事件参数提供了两个通用的参数类，后续有特殊需求也可以在 CustomEventArgs.cs 中添加新的参数类。

```c#
#region 通用事件参数

public class EventArgsItemID : EventArgsBase
{
    public int id;         // 物品 ID
}

public class EventArgsState : EventArgsBase
{
    public bool state;     // 状态
}

#endregion
```



EventManager.cs 不需要修改，它继承了 C# 泛型单例，需要结合 SingletonBase 一起使用，无需创建 GameObject。



下面是一个简单的例子：

打开 Event 场景，里面已有 BagPanel 界面，上面挂载了 EventBagPanel 脚本，并且添加了事件监听。

在接收事件参数时，需要把 EventArgsBase 基类转换成`触发事件时`发送的参数类型。

```c#
using UnityEngine;

public class EventBagPanel : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    void Start()
    {
        // 添加事件监听
        EventManager.Instance.AddListener(EventName.UI_Show_Bag, Show);
    }

    void Show(EventArgsBase e)
    {
        // 接收事件参数
        EventArgsState args = e as EventArgsState;
        if (args != null && args.state)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
```



另有一个空物体 Press B key，上面挂载了 EventShowPanel 脚本，监听键盘按键，触发事件，并携带了 EventArgsState 参数实例，所以上面的代码在接收事件参数时，将参数 e 转换成了 EventArgsState 类型。

```c#
using UnityEngine;

public class EventShowPanel : MonoBehaviour
{
    bool isShowBag;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isShowBag = !isShowBag;
            // 触发事件，携带参数
            this.TriggerEvent(EventName.UI_Show_Bag, new EventArgsState { state = isShowBag });
        }
    }
}
```