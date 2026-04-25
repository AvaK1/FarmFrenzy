using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("volume"))
        {
            Save(100);
        }
        else
        {
            AudioListener.volume = Load();
        }
    }

    public void ChangeVolume(Slider volumeSlider)
    {
        if (volumeSlider != null)
        {
            AudioListener.volume = volumeSlider.value;
            Save(volumeSlider.value);
        }
    }

    private void Save(float volumeValue)
    {
        PlayerPrefs.SetFloat("volume", volumeValue);
    }

    public float Load()
    {
        return PlayerPrefs.GetFloat("volume");
    }
}
