using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioClip dirSelect;
    [SerializeField] private AudioClip Select;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip skill;
    [SerializeField] private AudioClip frozen;

    private AudioSource source;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
        source.PlayOneShot(jump, 0f);
    }

    public void PlaydirSelect()
    {
        source.PlayOneShot(dirSelect);
    }
    public void PlaySelect()
    {
        source.PlayOneShot(Select);
    }
    public void Playjump()
    {
        source.PlayOneShot(jump);
    }
    public void Playskill()
    {
        source.PlayOneShot(skill);
    }
    public void Playfrozen()
    {
        source.PlayOneShot(frozen);
    }
}
