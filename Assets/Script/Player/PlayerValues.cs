using UnityEngine;

public class PlayerValues : MonoBehaviour
{
    private static float Move_TimeLimit;
    private static PlayerMove playermove;

    public static void reset_Move_TimeLimit()
    {
        Move_TimeLimit = LevelManager.get_Move_TimeLimit();
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
            playermove.Move(ValueManager.GetPlayerGridPos());
            playermove.reset_move();
        }
    }


    public static void onHit()
    {
        
    }
}
