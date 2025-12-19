using System.Collections.Generic;
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

    void Update()
    {
        foreach (var item in keyTexts)
        {
            int state = InputManager.GetKeyState(item.key);
            int state_raw = InputManager.GetKeyStateRaw(item.key);
            item.ice_img.fillAmount = state_raw != 0 ? state / (float)state_raw : 0;
        }
    }
}
