using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnima : MonoBehaviour
{
    [SerializeField] private Sprite down;
    [SerializeField] private Sprite downRight;
    [SerializeField] private Sprite right;
    [SerializeField] private Sprite upRight;
    [SerializeField] private Sprite up;
    [SerializeField] private Sprite upLeft;
    [SerializeField] private Sprite left;
    [SerializeField] private Sprite downLeft;

    // [SerializeField] private Sprite die;


    private SpriteRenderer sr;

    private int now_dir;
    private int prv_dir;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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
}
