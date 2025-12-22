using TMPro;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;
    void Start()
    {
        txt.text = LevelManager.GetNowLevel().ToString();
    }
}
