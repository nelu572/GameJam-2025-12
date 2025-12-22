using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static MapManager mapManager;
    void Start()
    {
        mapManager = gameObject.GetComponent<MapManager>();
        NextLevel();
    }

    private static int nowLevel = 0;

    public static int GetNowLevel()
    {
        return nowLevel;
    }
    public static void SetNowLevel(int level)
    {
        nowLevel = level;
    }


    public static void NextLevel()
    {
        if (StateManager.get_isdie())
        {
            SceneManager.LoadScene("Ending Scene");
            return;
        }
        nowLevel++;
        InputManager.SetAllKeyCooldown();
        PlayerValues.reset_Move_TimeLimit();
        SnowBallMove.Instance.SpawnSnowball();
        UIManager.Update_MaxTimeLimit(Move_TimeLimit);
        UIManager.Update_level(nowLevel);
        mapManager.DecreaseAllPositiveStates();
        set_Move_TimeLimit(-0.15f);
    }

    private static float Move_TimeLimit = 4f;
    private static float min_Move_TimeLimit = 1.25f;

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
