using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public static void GameStart()
    {
        NextLevel();
        StateManager.GameStart();
    }


    private static int nowLevel;
    void Start()
    {
        nowLevel = 0;
    }

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
        if (StateManager.get_isdie()) return;

        nowLevel++;

        if (nowLevel % 4 == 0)
        {
            MapManager.Instance.SpawnTorch(nowLevel);
        }
        MapManager.Instance.CheckTorchRemove(nowLevel);

        InputManager.SetAllKeyCooldown();
        SnowBallMove.Instance.SpawnSnowball();
        SnowBallMove.Instance.allSnowBallDrawMark();
        UIManager.Update_level(nowLevel);
        MapManager.Instance.DecreaseAllPositiveStates();
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
