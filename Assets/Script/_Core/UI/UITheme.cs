using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UIThemeSettings
{
    public SubjectId changingValue;
    public UINodeChanger nodeChanger;

}

[CreateAssetMenu(fileName = "ui_theme", menuName = "HCFramework/UITheme")]
public class UITheme : ScriptableObject
{
    [SerializeField] List<UIThemeSettings> themeSettings = new List<UIThemeSettings>();

    public List<UIThemeSettings> ThemeSettings => themeSettings;

}
