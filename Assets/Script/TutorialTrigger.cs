using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] string text;
    bool active;
    bool revertTime;
    float timeVelocity;


    private void Update()
    {
        if (active)
        {
            Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 0.2f, ref timeVelocity, 0.2f, 1000f, Time.unscaledDeltaTime);
            Time.fixedDeltaTime = 0.016f * Time.timeScale;
        }
        else if(revertTime)
        {
            Time.timeScale = Mathf.SmoothDamp(Time.timeScale, 1f, ref timeVelocity, 0.2f, 1000f, Time.unscaledDeltaTime);
            if(Time.timeScale >= 1f - 0.016f)
            {
                Debug.Log("Test");
                revertTime = false;
                Time.timeScale = 1f;
            }
            Time.fixedDeltaTime = 0.016f * Time.timeScale;
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if(active)
        {
            return;
        }
        if(other.GetComponent<Player>() != null)
        {
            FindObjectOfType<TutorialText>().Show(text);
            active = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (!active)
        {
            return;
        }
        if (other.GetComponent<Player>() != null)
        {
            FindObjectOfType<TutorialText>().Hide();
            active = false;
            revertTime = true;
        }
    }
}
