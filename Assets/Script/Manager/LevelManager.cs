using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static int GetNowLevel()
    {
        return nowLevel;
    }
    public static void SetNowLevel(int level)
    {
        nowLevel = level;
    }

    private static int nowLevel = 1;

    public static void NextLevel()
    {
        nowLevel++;
    }
    void Update()
    {

    }

    private static float Move_TimeLimit = 7.5f;

    public static float get_Move_TimeLimit()
    {
        return Move_TimeLimit;
    }
    public static void set_Move_TimeLimit(float time)
    {
        Move_TimeLimit+=time;
    }
    
}
