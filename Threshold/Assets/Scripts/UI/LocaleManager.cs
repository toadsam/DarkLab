using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown localeDropdown;
    private int currentLocaleIndex;
    private bool isChangingLocale = false;
    private Coroutine changeLocaleCoroutine;
    
    private bool isLoaded = false;
    
    private void Awake()
    {
        if (localeDropdown == null)
        {
            Debug.LogError("LocaleManager: No TMP_Dropdown component found.");
            return;
        }
        
        localeDropdown.onValueChanged.AddListener(ChangeLocale);
        
        
        StartCoroutine(InitLocale());
    }

    private IEnumerator InitLocale()
    {
        yield return LocalizationSettings.InitializationOperation;
        
        Debug.Log("[LocaleManager] LocalizationSettings.InitializationOperation is done. Current locale is " + LocalizationSettings.SelectedLocale.name + " with sortOrder " + LocalizationSettings.SelectedLocale.SortOrder);

        int tempIndex = PlayerPrefs.GetInt("CurrentLocaleIndex", LocalizationSettings.SelectedLocale.SortOrder);
        
        currentLocaleIndex = tempIndex;
        localeDropdown.value = currentLocaleIndex;
        changeLocaleCoroutine = StartCoroutine(ChangeRoutine(currentLocaleIndex));
        
        isLoaded = true;
    }
    
    public void ChangeLocale(int index)
    {
        if (isChangingLocale)
            return;

        changeLocaleCoroutine = StartCoroutine(ChangeRoutine(index));
    }
    
    IEnumerator ChangeRoutine(int index)
    {
        isChangingLocale = true;
        localeDropdown.interactable = false;
        
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        
        SavePlayerData();
        isChangingLocale = false;
        localeDropdown.interactable = true;
    }
    
    private void OnDestroy()
    {
        if (changeLocaleCoroutine != null)
            StopCoroutine(changeLocaleCoroutine);
    }

    public void SavePlayerData()
    {
        if (!isLoaded) return;
        
        Debug.Log("[LocaleManager] SavePlayerData");
        
        PlayerPrefs.SetInt("CurrentLocaleIndex", LocalizationSettings.SelectedLocale.SortOrder);
    }
}
