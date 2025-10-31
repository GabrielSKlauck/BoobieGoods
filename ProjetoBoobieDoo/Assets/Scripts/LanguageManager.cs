using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;

    void Start()
    {
        if (languageDropdown != null)
        {
            languageDropdown.ClearOptions();
            foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
            }
            
            int savedIndex = -1;
            if (PlayerPrefs.HasKey("SavedLocalizationIndex"))
            {
                savedIndex = PlayerPrefs.GetInt("SavedLocalizationIndex");
            }

            int defaultIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
            languageDropdown.value = (savedIndex >= 0 && savedIndex < LocalizationSettings.AvailableLocales.Locales.Count)
                ? savedIndex
                : defaultIndex;

            languageDropdown.RefreshShownValue();
            languageDropdown.onValueChanged.AddListener(ChangeLanguage);
        }
    }

    void ChangeLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        PlayerPrefs.SetInt("SavedLocalizationIndex", index);
        PlayerPrefs.Save();
    }
}
