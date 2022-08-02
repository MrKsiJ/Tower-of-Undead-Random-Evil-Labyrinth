using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    [SerializeField] internal string[] TranslateWords_RU;
    [SerializeField] internal string[] TranslateWords_ENG;
    [SerializeField] internal string[] TranslateWords_Xindy;
   
    internal string LanguageUpdate(int index)
    {
        string text = "";
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Russian:
                text = TranslateWords_RU[index];
                break;
            case SystemLanguage.English:
                text = TranslateWords_ENG[index];
                break;
            case SystemLanguage.Unknown:
                text = TranslateWords_ENG[index];
                break;
            case SystemLanguage.Indonesian:
                text = TranslateWords_Xindy[index];
                break;
        }
        return text;
    }
    internal void LanguageUpdate(Text text,int index)
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Russian:
                text.text = TranslateWords_RU[index];
                break;
            case SystemLanguage.English:
                text.text = TranslateWords_ENG[index];
                break;
            case SystemLanguage.Unknown:
                text.text = TranslateWords_ENG[index];
                break;
            case SystemLanguage.Indonesian:
                text.text = TranslateWords_Xindy[index];
                break;
        }
    }
}
