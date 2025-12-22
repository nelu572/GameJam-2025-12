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
        SnowBallMove.Instance.SpawnSnowball();
        SnowBallMove.Instance.allSnowBallDrawMark();
        UIManager.Update_level(nowLevel);
        mapManager.DecreaseAllPositiveStates();
        StateManager.StartMarking();
    }
    public static void move_select()
    {
        StateManager.set_canMoving(true);
        SnowBallMove.Instance.allSnowBallDeletedMark();
        PlayerValues.reset_Move_TimeLimit();
        UIManager.Update_MaxTimeLimit(Move_TimeLimit);
    }

    private static float Move_TimeLimit = 3f;

    public static float get_Move_TimeLimit()
    {
        return Move_TimeLimit;
    }

}
