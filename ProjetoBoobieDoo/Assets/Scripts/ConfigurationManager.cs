using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ConfigurationManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

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
