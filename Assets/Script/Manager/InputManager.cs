using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // 0 = 사용 가능, 1이상 = 잠김
    private static Dictionary<KeyCode, int> keyState = new();

    void Awake()
    {
        keyState.Clear();
    }

    //이동 / 일반 키
    public static bool GetKey(KeyCode key)
    {
        return GetKeyState(key) == 0 && Input.GetKey(key);
    }

    public static int GetKeyState(KeyCode key)
    {
        return keyState.TryGetValue(key, out int state) ? state : 0;
    }

    public static void SetKeyState(KeyCode key, int state)
    {
        keyState[key] = state;
    }
    public static void SetAllKeyCooldown()
    {
        foreach (var key in keyState.Keys)
        {
            keyState[key]--;
        }
    }


    //확정 키 (Enter 고정)
    public static bool GetConfirmDown()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }
}
