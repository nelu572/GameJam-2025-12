using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene : MonoBehaviour
{
    private Color hover = new Color(0.62f, 0.8f, 0.99f);

    [SerializeField] private Button start;
    [SerializeField] private Button exit;


    private Image start_img;
    private Image exit_img;

    [SerializeField] private Image image;
    [SerializeField] private float duration = 0.5f;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Start()
    {
        start_img = start.GetComponent<Image>();
        exit_img = exit.GetComponent<Image>();

        now_state = "Start";
    }
    static String now_state;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (now_state == "Start")
            {
                AudioManager.Instance.PlaydirSelect();
                now_state = "Exit";
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (now_state == "Exit")
            {
                AudioManager.Instance.PlaydirSelect();
                now_state = "Start";
            }
        }
        if (now_state == "Start")
        {
            start_img.color = hover;
            exit_img.color = Color.white;
        }
        else
        {
            start_img.color = Color.white;
            exit_img.color = hover;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (now_state == "Start")
            {
                StartGame();
            }
            if (now_state == "Exit")
            {
                ExitGame();
            }
        }
    }
    private void StartGame()
    {
        AudioManager.Instance.PlaySelect();

        image.DOKill();

        image.DOFade(1f, duration)
             .OnComplete(OnFadeComplete);

    }
    private void OnFadeComplete()
    {
        SceneManager.LoadScene("Game Scene");
    }
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
