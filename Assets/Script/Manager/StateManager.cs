using System;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    void Start()
    {
        ismarking = true;
        canMoving = false;
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

    private static bool ismarking;
    private static float marking_time_MAX = 1.0f;
    private static float marking_time = 1.0f;


    public static bool get_ismarking()
    {
        return ismarking;
    }
    public static void StartMarking()
    {
        ismarking = true;
        float minus = Math.Min((float)LevelManager.GetNowLevel() / 25, 0.4f);

        marking_time = marking_time_MAX - minus;
    }
    public static void EndMarking()
    {
        ismarking = false;
        LevelManager.move_select();
    }
    void Update()
    {
        if (ismarking)
        {
            marking_time -= Time.deltaTime;
            if (marking_time <= 0)
            {
                marking_time = 0;
                EndMarking();
            }
        }
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
