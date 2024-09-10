using System;
using UnityEngine;

public class ActionTester : MonoBehaviour
{
    public static Action<int> myAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myAction?.Invoke(7);

    }

}
