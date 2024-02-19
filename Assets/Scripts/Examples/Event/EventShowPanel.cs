using System.Collections;
using System.Collections.Generic;
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
