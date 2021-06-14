using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Knife knife = other.GetComponent<Knife>();
        Player player = other.GetComponent<Player>();
        if (knife != null || player != null)
        {
            FindObjectOfType<Player>().Finish();
        }
    }
}
