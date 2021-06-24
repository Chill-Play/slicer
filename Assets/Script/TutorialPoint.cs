using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPoint : MonoBehaviour
{
    [System.Serializable]
    public struct TutorialPointInfo
    {
        public int tapToComplete;
        public bool lastTutorialPoint;

        public TutorialPointInfo(int tapToComplete = 1, bool lastTutorialPoint = false)
        {
            this.tapToComplete = tapToComplete;
            this.lastTutorialPoint = lastTutorialPoint;
        }      
    }


    public static event System.Action<TutorialPointInfo> OnTutorialPointTrigger;

    [SerializeField] TutorialPointInfo tutorialPointInfo;

    Collider pointCollider;

    private void Awake()
    {
        pointCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null || other.gameObject.GetComponent<Knife>() != null)
        {
            pointCollider.enabled = false;
            OnTutorialPointTrigger.Invoke(tutorialPointInfo);
        }
    }
}
