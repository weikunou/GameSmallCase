using System.Collections;
using System.Collections.Generic;
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
