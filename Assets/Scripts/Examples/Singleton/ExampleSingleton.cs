using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSingleton : MonoBehaviour
{
    void Start()
    {
        BaseManager.Instance.Log();

        if (MonoManager.IsInitialized)
        {
            MonoManager.Instance.Log();
        }

        MonoAutoManager.Instance.Log();
    }
}
