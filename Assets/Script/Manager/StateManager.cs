using UnityEngine;

public class StateManager : MonoBehaviour
{
    void Start()
    {
        canMoving = true;
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
