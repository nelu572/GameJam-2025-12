using UnityEngine;

public class StateManager : MonoBehaviour
{
    void Start()
    {
        isSetting = false;
        canMoving = true;
    }
    private static bool isSetting;
    public static bool get_isSetting()
    {
        return isSetting;
    }
    public static void set_isSetting(bool set)
    {
        isSetting = set;
    }

    private static bool canMoving;

    public static bool get_canMoving()
    {
        return canMoving;
    }
    public static void set_canMoving(bool move)
    {
        canMoving = move;
    }
}
