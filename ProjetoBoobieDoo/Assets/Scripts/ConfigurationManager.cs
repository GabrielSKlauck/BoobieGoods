using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    [Header("Volume Geral")]
    [SerializeField]
    private Slider _generalVolumeSlider;

    [Header("Efeitos Sonoros")]
    [SerializeField]
    private Slider _soundEffectSlider;

    [Header("Volume da música")]
    [SerializeField]
    private Slider _musicVolumeSlider;

    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        if (PlayerPrefs.HasKey("SavedGeneralVolume"))
        {
            _generalVolumeSlider.value = PlayerPrefs.GetInt("SavedGeneralVolume");
        }
        if (PlayerPrefs.HasKey("SavedSoundEffect"))
        {
            _soundEffectSlider.value = PlayerPrefs.GetInt("SavedSoundEffect");
        }
        if (PlayerPrefs.HasKey("SavedMusicVolume"))
        {
            _musicVolumeSlider.value = PlayerPrefs.GetInt("SavedMusicVolume");
        }
        if (PlayerPrefs.HasKey("SavedLocalizationIndex"))
        {
            int savedIndex = PlayerPrefs.GetInt("SavedLocalizationIndex");
            
            if (savedIndex >= 0 && savedIndex < LocalizationSettings.AvailableLocales.Locales.Count)
            {
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[savedIndex];
                Debug.Log("Loaded saved language at index: " + savedIndex);
            }
            else
            {
                Debug.LogWarning("Saved locale index out of range. Using default locale.");
            }
        }
        else
        {
            Debug.Log("No saved language found. Using default locale.");
        }
    }
}
