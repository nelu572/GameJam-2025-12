using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIAnimation : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private LensDistortion lensDistortion;
    private Vignette vignette;

    void Awake()
    {
        volume.profile.TryGet(out lensDistortion);
        volume.profile.TryGet(out vignette);

        lensDistortion.scale.value = 0.01f;
        vignette.intensity.value = 3;
    }
    void Start()
    {
        DOTween.To(
            () => lensDistortion.scale.value,
            v => lensDistortion.scale.value = v,
            1f,
            1f
        ).SetEase(Ease.InExpo);
    }

    void Update()
    {

    }
}
