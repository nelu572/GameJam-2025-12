using UnityEngine;

public class StateManager : MonoBehaviour
{
    void Start()
    {
        canMoving = true;
        isdie = false;
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

    private static bool isdie;

    public static bool get_isdie()
    {
        return isdie;
    }
    public static void set_isdie(bool d)
    {
        isdie = d;
    }
}
