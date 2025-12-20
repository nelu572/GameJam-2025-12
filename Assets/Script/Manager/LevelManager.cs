using UnityEngine;

public class LevelManager : MonoBehaviour
{
    void Start()
    {
        NextLevel();
    }

    public static int GetNowLevel()
    {
        return nowLevel;
    }
    public static void SetNowLevel(int level)
    {
        nowLevel = level;
    }

    private static int nowLevel = 0;

    public static void NextLevel()
    {
        nowLevel++;
        PlayerValues.reset_Move_TimeLimit();
        InputManager.SetAllKeyCooldown();
        SnowBallMove.Instance.SpawnSnowball();
        UIManager.Update_MaxTimeLimit(Move_TimeLimit);
        UIManager.Update_level(nowLevel);
    }


    void Update()
    {

    }

    private static float Move_TimeLimit = 7f;
    private static float min_Move_TimeLimit = 0.3f;

    public static float get_Move_TimeLimit()
    {
        return Move_TimeLimit;
    }
    public static void set_Move_TimeLimit(float time)
    {
        Move_TimeLimit += time;

        if (Move_TimeLimit < min_Move_TimeLimit)
            Move_TimeLimit = min_Move_TimeLimit;
        UIManager.Update_MaxTimeLimit(Move_TimeLimit);
    }

}
