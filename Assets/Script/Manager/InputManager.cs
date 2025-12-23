using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private List<KeyCode> keys;

    // 현재 상태값 (감소됨)
    private static Dictionary<KeyCode, int> keyState = new();

    // SetKeyState로 받은 원본 상태값
    private static Dictionary<KeyCode, int> keyStateRaw = new();

    private static int locked_keys;

    void Start()
    {
        Reset();
    }

    private void Reset()
    {
        keyState.Clear();
        keyStateRaw.Clear();
        locked_keys = 0;

        if (keys == null) return;

        foreach (var key in keys)
        {
            keyState[key] = 0;
            keyStateRaw[key] = 0;
        }
    }

    // 이동 / 일반 키
    public static bool GetKey(KeyCode key)
    {
        if (GetKeyState(key) != 0 && Input.GetKey(key))
        {
            UIManager.Instance.cantMove(key);
        }
        return GetKeyState(key) == 0 && Input.GetKeyDown(key);
    }

    // 현재 감소된 상태값
    public static int GetKeyState(KeyCode key)
    {
        return keyState.TryGetValue(key, out int state) ? state : 0;
    }

    // SetKeyState 시 전달받은 원본 상태값
    public static int GetKeyStateRaw(KeyCode key)
    {
        return keyStateRaw.TryGetValue(key, out int state) ? state : 0;
    }

    // 상태 설정
    public static void SetKeyState(KeyCode key, int state)
    {
        if (keyState[key] != state)
        {
            if (state > 0)
                locked_keys += 1;
            else
                locked_keys -= 1;
            keyState[key] = state;
            keyStateRaw[key] = state - 1;
        }
    }

    // 모든 키 쿨다운 감소
    public static void SetAllKeyCooldown()
    {
        var keyList = new List<KeyCode>(keyState.Keys);

        foreach (var key in keyList)
        {
            if (keyState[key] > 0)
            {
                keyState[key]--;
                if (keyState[key] == 0)
                {
                    locked_keys -= 1;
                }
            }
        }
    }
    public static KeyCode GetRandomLockedKey()
    {
        List<KeyCode> locked = new List<KeyCode>();

        foreach (var pair in keyState)
        {
            if (pair.Value > 0) // 잠겨있는 키
            {
                locked.Add(pair.Key);
            }
        }

        if (locked.Count == 0)
            return KeyCode.None;

        return locked[Random.Range(0, locked.Count)];
    }


    public static int GetLockedKeys()
    {
        return locked_keys;
    }

    // Enter키
    public static bool GetConfirmDown()
    {
        return Input.GetKeyDown(KeyCode.Return);
    }

    // Shift 고정
    public static bool GetShiftDown()
    {
        return Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift);
    }
}
