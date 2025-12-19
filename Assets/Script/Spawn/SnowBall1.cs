using UnityEngine;

public class SnowBall1 : MonoBehaviour
{
    private bool move;
    [SerializeField] private float speed = 8;
    void Start()
    {
        move = false;
    }
    void Update()
    {
        if (!move)
            return;
        transform.position += transform.right * speed * Time.deltaTime;

        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
        {
            SnowBallMove.Instance.DestroySnowball(gameObject, 1);
            Destroy(gameObject);
        }
    }
    public void start_move()
    {
        move = true;
    }
}
