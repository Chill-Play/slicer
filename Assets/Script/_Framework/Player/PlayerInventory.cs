using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory
{
    const string PREFS_ITEMS = "M_PlayerItems";
    List<PlayerItem> items = new List<PlayerItem>();


    public void LoadItems()
    {
        string text = PlayerPrefs.GetString(PREFS_ITEMS, "");
        List<PlayerItem> tempItems = Tiny.Json.Decode<List<PlayerItem>>(text);
        if(tempItems != null)
        {
            items = tempItems;
        }
    }

    public void SaveItems()
    {
        string json = Tiny.Json.Encode(items);
        PlayerPrefs.SetString(PREFS_ITEMS, json);
    }


    public void AddItem(string id, string category)
    {
        if(items.Any((x) => x.id == id && x.category == category))
        {
            Debug.LogError("Item already added : " + id);
            return;
        }
        PlayerItem item = new PlayerItem();
        item.id = id;
        item.category = category;
        items.Add(item);
    }


    public void RemoveItem(PlayerItem item)
    {
        if(!items.Contains(item))
        {
            Debug.LogError("Item list does not contain : " + item);
            return;
        }
        items.Remove(item);
    }


    public PlayerItem GetItem(string id, string category)
    {
        PlayerItem item = items.First((x) => x.id == id && x.category == category);
        return item;
    }


    public bool HasItem(string id, string category)
    {
        return items.Any((x) => x.id == id && x.category == category);
    }


    public IEnumerable<PlayerItem> GetItemsFromCategory(string category)
    {
        return items.Where((x) => x.category == category);
    }
}
