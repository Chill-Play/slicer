using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelLabel : MonoBehaviour
{
    [SerializeField] TMP_Text label;

    void Start()
    {
        label.text = "LEVEL " + FindObjectOfType<LevelController>().CurrentLevel;
    }

}
