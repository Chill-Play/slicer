using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class KnifeStorage : MonoBehaviour
{
    const string SKIN_ITEM_CATEGORY = "Knife_Skin";
    const string PROGRESS_PREFS = "G_KnifeSkinProgess";

    [SerializeField] List<KnifeSkin> skinList;
    [SerializeField] KnifeSkin defaultSkin;
    [SerializeField] float progressPerLevel = 0.02f;
    List<KnifeSkin> availableSkins = new List<KnifeSkin>();

    float progressToNext = 0.0f;

    public float ProgressToNext => progressToNext;

    public KnifeSkin CurrentSkin => availableSkins[availableSkins.Count - 1];

    private void Awake()
    {
        PlayerInfo.Instance.inventory.LoadItems();
        IEnumerable<PlayerItem> knifes = PlayerInfo.Instance.inventory.GetItemsFromCategory(SKIN_ITEM_CATEGORY);
        foreach(PlayerItem item in knifes)
        {
            KnifeSkin skin = skinList.FirstOrDefault((x) => x.Id == item.id);
            if(skin != null)
            {
                availableSkins.Add(skin);
            }
        }
        if(!availableSkins.Contains(defaultSkin))
        {
            availableSkins.Insert(0, defaultSkin);
        }
    }


    public void AddSkinProgress(out float oldProgress, out float newProgress)
    {
        progressToNext = PlayerPrefs.GetFloat(PROGRESS_PREFS, 0f);
        oldProgress = progressToNext;
        progressToNext += progressPerLevel;
        if(progressToNext >= 1f)
        {
            OpenNextSkin();
            progressToNext = 0f;
        }
        PlayerPrefs.SetFloat(PROGRESS_PREFS, progressToNext);
        newProgress = progressToNext;
    }


    public void OpenNextSkin()
    {
        AddSkin(GetNextSkin());
    }


    public void AddSkin(KnifeSkin skin)
    {
        PlayerInfo.Instance.inventory.AddItem(skin.Id, SKIN_ITEM_CATEGORY);
        PlayerInfo.Instance.inventory.SaveItems();
    }


    public KnifeSkin GetNextSkin()
    {
        for(int i = 0; i < skinList.Count; i++)
        {
            if(!availableSkins.Contains(skinList[i]))
            {
                return skinList[i];
            }
        }
        return null;
    }

   
    
}
