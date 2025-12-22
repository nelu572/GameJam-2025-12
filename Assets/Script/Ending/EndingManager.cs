using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txt;
    void Start()
    {
        txt.text = LevelManager.GetNowLevel().ToString();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main Scene");
        }
    }
}