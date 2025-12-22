using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerValues : MonoBehaviour
{
    private static float Move_TimeLimit;
    private static PlayerMove playermove;

    public static void reset_Move_TimeLimit()
    {
        Move_TimeLimit = LevelManager.get_Move_TimeLimit();
    }

    public static float get_Move_TimeLimit()
    {
        return Move_TimeLimit;
    }

    void Start()
    {
        playermove = gameObject.GetComponent<PlayerMove>();
        reset_Move_TimeLimit();
    }
    void Update()
    {
        if (!StateManager.get_canMoving())
            return;
        Move_TimeLimit -= Time.deltaTime;
        if (Move_TimeLimit <= 0)
        {
            PlayerHit.Instance.onHit(4);
            SnowBallMove.Instance.StartMove();
            playermove.ClearPreview();
            playermove.reset_move();
        }
    }

    private static int dir;
    public static void set_Dir(Vector2Int d)
    {
        if (d == Vector2Int.zero)
        {
            dir = 0;
            return;
        }
        float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
        angle = (angle + 450f) % 360f;

        int index = Mathf.RoundToInt(angle / 45f) % 8;
        dir = index + 1;
    }

    public static int get_Dir()
    {
        return dir;
    }
}
