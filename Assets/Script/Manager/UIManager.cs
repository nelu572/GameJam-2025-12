using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class KeyStateIMG
{
    public KeyCode key;
    public Image ice_img;
}

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    [SerializeField] private List<KeyStateIMG> keyTexts;

    private static float max_value;
    [SerializeField] private Slider slider;

    [SerializeField] private TextMeshProUGUI levelTXT;

    private static int now_level = 0;
    private int prv_level = -1;

    [SerializeField] private TMP_Text countdownText;

    void Start()
    {
        levelTXT.text = "1";

        Sequence seq = DOTween.Sequence();

        seq.AppendCallback(() => countdownText.text = "3");
        seq.AppendInterval(1f);

        seq.AppendCallback(() => countdownText.text = "2");
        seq.AppendInterval(1f);

        seq.AppendCallback(() => countdownText.text = "1");
        seq.AppendInterval(1f);

        seq.AppendCallback(OnCountdownFinished);
    }

    void Update()
    {
        // 방향키 UI
        foreach (var item in keyTexts)
        {
            int state = InputManager.GetKeyState(item.key);
            int state_raw = InputManager.GetKeyStateRaw(item.key);

            item.ice_img.fillAmount =
                state_raw != 0 ? state / (float)state_raw : 0;
        }

        // 이동 제한 시간 슬라이더
        if (StateManager.get_canMoving())
        {
            slider.maxValue = max_value;
            slider.value = PlayerValues.get_Move_TimeLimit();
        }
        else
        {
            slider.value = 0;
        }

        // 웨이브 / 레벨 텍스트
        if (prv_level != now_level)
        {
            levelTXT.text = now_level.ToString();
            prv_level = now_level;
        }
    }

    private void OnCountdownFinished()
    {
        countdownText.text = "";
        RunGameLogic();
    }

    private void RunGameLogic()
    {
        LevelManager.GameStart();
    }

    public static void Update_MaxTimeLimit(float time)
    {
        max_value = time;
    }

    public static void Update_level(int level)
    {
        now_level = level;
    }
    [SerializeField] private GameObject ice_icon;
    public void cantMove(KeyCode key)
    {
        if (ice_icon.activeSelf) return;
        ice_icon.SetActive(true);

        DOVirtual.DelayedCall(0.5f, () =>
        {
            ice_icon.SetActive(false);
        });
    }
}
