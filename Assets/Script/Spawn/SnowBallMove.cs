using UnityEngine;

public class SnowBallMove : MonoBehaviour
{

    private static bool IsMovable = false;

    private static float temptime = 5;

    void Update()
    {
        if (!IsMovable)
            return;
        UpdateMove();

    }
    public static void start_move()
    {
        IsMovable = true;
        temptime = 5;
    }
    private void UpdateMove()
    {
        Debug.Log(temptime);
        temptime -= Time.deltaTime;
        if (temptime <= 0)
        {
            IsMovable = false;
            StateManager.set_canMoving(true);
            LevelManager.NextLevel();
        }
    }
}
