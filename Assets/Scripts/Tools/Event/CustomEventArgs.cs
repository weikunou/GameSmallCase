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

#region 扩展事件参数

public class EventArgsAttack : EventArgsBase
{
    public int senderID;   // 发起者的 ID
    public int damage;     // 伤害值
}

#endregion