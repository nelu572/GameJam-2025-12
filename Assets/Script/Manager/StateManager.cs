using System;
using NUnit.Framework;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    private static bool isSetting = false;
    public static bool get_isSetting()
    {
        return isSetting;
    }
    public static void set_isSetting(bool set)
    {
        isSetting = set;
    }

    private static bool canMoving = true;

    public static bool get_canMoving()
    {
        return canMoving;
    }
    public static void set_canMoving(bool move)
    {
        canMoving = move;
    }
}
