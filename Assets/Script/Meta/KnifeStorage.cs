using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class KnifeStorage : MonoBehaviour
{
    const string SKIN_ITEM_CATEGORY = "Knife_Skin";

    [SerializeField] List<KnifeSkin> skinList;
    [SerializeField] KnifeSkin defaultSkin;
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
