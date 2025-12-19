using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnowBallMove : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static SnowBallMove Instance { get; private set; }

    private bool isMovable = false;
    private int snowballCount = 0;
    private List<GameObject> snowball1 = new List<GameObject>();

    [SerializeField] private GameObject SnowBall_P1; // Inspector에서 연결

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    private void Update()
    {
        if (!isMovable) return;
        UpdateMove();
    }

    // Snowball 생성
    public void SpawnSnowball()
    {
        GameObject sb = Instantiate(SnowBall_P1);
        snowball1.Add(sb);
        snowballCount++;
    }

    // Snowball 제거
    public void DestroySnowball(GameObject snowball, int num)
    {
        if (num == 1)
        {
            snowball1.Remove(snowball);
        }
        snowballCount--;
        Destroy(snowball);
    }

    // 이동 시작
    public void StartMove()
    {
        foreach (GameObject snowball in snowball1)
        {
            SnowBall1 sbScript = snowball.GetComponent<SnowBall1>();
            if (sbScript != null)
                sbScript.start_move();
        }
        isMovable = true;
    }

    // 이동 업데이트
    private void UpdateMove()
    {
        if (snowballCount <= 0)
        {
            isMovable = false;
            StateManager.set_canMoving(true);
            LevelManager.NextLevel();
        }
    }
}
