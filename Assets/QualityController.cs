using UnityEngine;
using UnityEngine.UI;

public class QualityController : MonoBehaviour
{
    public Slider qualitySlider;
    public RawImage targetRawImage;

    void Start()
    {
        qualitySlider.value = 1f;
        UpdateOpacity(qualitySlider.value);
        qualitySlider.onValueChanged.AddListener(UpdateOpacity);
    }

    void UpdateOpacity(float qualityValue)
    {
        if(targetRawImage == null) return;
        
        float opacity = 1 - qualityValue;
        opacity = Mathf.Min(opacity, 0.8f);
        
        Color currentColor = targetRawImage.color;
        currentColor.a = opacity;
        targetRawImage.color = currentColor;
        
        PlayerPrefs.SetFloat("QualityLevel", qualityValue);
    }

    void OnDestroy()
    {
        PlayerPrefs.Save();
    }
}
