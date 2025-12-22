using System.Data.Common;
using UnityEngine;

public class PlayerAnima : MonoBehaviour
{
    public static PlayerAnima Instance { get; private set; }

    [SerializeField] private Sprite down;
    [SerializeField] private Sprite downRight;
    [SerializeField] private Sprite right;
    [SerializeField] private Sprite upRight;
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite upLeft;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite downLeft;

    [SerializeField] private Sprite hit;


    private SpriteRenderer sr;

    private float hit_Max_time = 1.5f;
    private float hit_time;

    private int now_dir;
    private int prv_dir;

    void Awake()
    {
        Instance = this;
        sr = GetComponent<SpriteRenderer>();
        hit_time = 0;
    }

    void Update()
    {
        if (StateManager.get_isdie()) return;
        if (hit_time > 0)
        {
            sr.sprite = hit;

            hit_time -= Time.deltaTime;
            if (hit_time <= 0)
                hit_time = 0;

            prv_dir = -1;
            return;
        }
        now_dir = PlayerValues.get_Dir();

        if (now_dir != prv_dir)
        {
            switch (now_dir)
            {
                case 0:
                    sr.sprite = down;
                    break;
                case 1:
                    sr.sprite = down;
                    break;
                case 2:
                    sr.sprite = downRight;
                    break;
                case 3:
                    sr.sprite = right;
                    break;
                case 4:
                    sr.sprite = upRight;
                    break;
                case 5:
                    sr.sprite = up;
                    break;
                case 6:
                    sr.sprite = upLeft;
                    break;
                case 7:
                    sr.sprite = left;
                    break;
                case 8:
                    sr.sprite = downLeft;
                    break;
            }
        }

        prv_dir = now_dir;
    }
    public void Hit_start()
    {
        hit_time = hit_Max_time;
    }
    public bool get_ishitTime()
    {
        return hit_time > 0;
    }
    public void StartDieAnima()
    {
        sr.sprite = hit;
        transform.eulerAngles = new Vector3(0, 0, 90);
    }

}
