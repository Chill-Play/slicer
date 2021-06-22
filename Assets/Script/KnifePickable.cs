using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifePickable : MonoBehaviour, IPickable
{
    [SerializeField] GameObject skin;
    bool picked;

    void Start()
    {
        SetSkin(FindObjectOfType<KnifeStorage>().CurrentSkin);
    }


    public void Pick()
    {
        if(picked)
        {
            return;
        }
        picked = true;
        FindObjectOfType<Player>().SpawnKnife();
        Destroy(gameObject);
    }


    public void SetSkin(KnifeSkin newSkin)
    {
        Destroy(skin);
        skin = Instantiate(newSkin.Blade, transform.position + newSkin.BladeOffset, Quaternion.identity, transform);
        skin.transform.rotation = transform.rotation * Quaternion.LookRotation(newSkin.BladeForwardAxis);
        skin.transform.localScale = newSkin.Blade.transform.localScale;
    }
}
