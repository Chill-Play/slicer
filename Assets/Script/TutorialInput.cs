using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInput : MonoBehaviour
{
    public static event System.Action<bool> OnTutorialPointComplete;



    int tapToComplete = 0;
    int currentTaps = 0;
    bool setuped = false;
    bool isLast = false;

    public void Setup(TutorialPoint.TutorialPointInfo tutorialPointInfo)
    {
        this.tapToComplete = tutorialPointInfo.tapToComplete;
        currentTaps = 0;
        isLast = tutorialPointInfo.lastTutorialPoint;
        setuped = true;
    }

    public void OnTap()
    {
        if (setuped)
        {
            currentTaps++;
            if (currentTaps >= tapToComplete)
            {
                currentTaps = 0;
                setuped = false;               
                OnTutorialPointComplete.Invoke(isLast);
                isLast = false;
            }
        }
    }


}
