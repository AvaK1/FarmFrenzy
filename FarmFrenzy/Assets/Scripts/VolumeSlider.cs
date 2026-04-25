using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = SoundManager.Instance.Load();
    }

    public void OnVolumeChanged()
    {
        SoundManager.Instance.ChangeVolume(slider);
    }
}
