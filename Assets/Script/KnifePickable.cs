using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifePickable : MonoBehaviour, IPickable
{
    bool picked;
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
}
