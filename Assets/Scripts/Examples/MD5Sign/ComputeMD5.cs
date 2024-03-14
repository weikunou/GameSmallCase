using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeMD5 : MonoBehaviour
{
    void Start()
    {
        Dictionary<string, object> args = new Dictionary<string, object>
        {
            ["product_id"] = 100001,         // 产品 id
            ["product_name"] = "金币礼包",    // 产品名称
            ["amount"] = 1,                  // 数量
            ["price"] = 600                  // 价格
        };
        string md5 = MD5Sign.ComputeSign(args);
        Debug.Log($"md5 = {md5}");
    }
}
