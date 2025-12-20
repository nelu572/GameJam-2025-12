using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private List<KeyStateIMG> keyTexts;

    private static float max_value;
    [SerializeField] private Slider slider;

    [SerializeField] private TextMeshProUGUI levelTXT;

    private static int now_level = 0;
    private int prv_level = -1;
    void Update()
    {
        foreach (var item in keyTexts)
        {
            int state = InputManager.GetKeyState(item.key);
            int state_raw = InputManager.GetKeyStateRaw(item.key);
            item.ice_img.fillAmount = state_raw != 0 ? state / (float)state_raw : 0;
        }

        if (StateManager.get_canMoving())
        {
            slider.maxValue = max_value;
            slider.value = PlayerValues.get_Move_TimeLimit();
        }
        else
        {
            slider.value = 0;
        }
        if (prv_level != now_level)
            levelTXT.text = now_level.ToString();
        prv_level = now_level;
    }

    public static void Update_MaxTimeLimit(float time)
    {
        max_value = time;
    }

    public static void Update_level(int level)
    {
        now_level = level;
    }
}
