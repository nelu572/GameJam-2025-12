using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHit : MonoBehaviour
{
    public static PlayerHit Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void CheckHit(Vector2Int snowballGridPos)
    {
        Vector2Int playerPos = ValueManager.GetPlayerGridPos();
        if (playerPos == snowballGridPos)
        {
            if (!PlayerAnima.Instance.get_ishitTime())
                onHit(3);
        }
    }

    public void onHit(int delay)
    {
        if (InputManager.GetLockedKeys() >= 4)
        {
            die();
            return;
        }

        KeyCode lock_key;

        // 잠기지 않은 키가 나올 때까지 반복
        do
        {
            int temp = Random.Range(0, 4);
            lock_key = temp switch
            {
                0 => KeyCode.W,
                1 => KeyCode.A,
                2 => KeyCode.S,
                3 => KeyCode.D,
                _ => KeyCode.W
            };
        }
        while (InputManager.GetKeyState(lock_key) > 0);

        InputManager.SetKeyState(lock_key, delay);
        PlayerAnima.Instance.Hit_start();

        if (InputManager.GetLockedKeys() >= 4)
        {
            die();
        }
    }
    private void die()
    {
        PlayerAnima.Instance.StartDieAnima();
        SnowBallMove.Instance.set_allSnowball_SortingLayer("BackBullet");
        StateManager.set_isdie(true);
    }
}