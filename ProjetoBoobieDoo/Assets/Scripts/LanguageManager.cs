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
            languageDropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
            languageDropdown.onValueChanged.AddListener(ChangeLanguage);
        }
    }

    void ChangeLanguage(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
