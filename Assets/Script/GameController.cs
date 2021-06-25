using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public event System.Action OnGameStart;
    public event System.Action OnWin;
    public event System.Action OnLose;
    public event System.Action<TutorialPoint.TutorialPointInfo> OnTutorialStart;
    public event System.Action OnTutorialEnd;

    [SerializeField] Player player;
    [SerializeField] Transform finish;
    [SerializeField] bool tutorialMode;

    [SerializeField] SubjectId gameStateId;
    [SerializeField] SubjectId winStateId;
    [SerializeField] SubjectId loseStateId;
    [SerializeField] SubjectId gameFinishStateId;
    [SerializeField] SubjectId tutorialStateId;
    [SerializeField] SubjectId tutorialMovementStateId;

    float levelProgress;
    bool gameStarted;

    public float LevelProgress => levelProgress;

    // Start is called before the first frame update
    void Start()
    {
        //player.enabled = false;
        player.OnDie += Player_OnDie;
        TutorialPoint.OnTutorialPointTrigger += TutorialPoint_OnTutorialPointTrigger;
        TutorialInput.OnTutorialPointComplete += TutorialInput_OnTutorialPointComplete;
    }

    private void TutorialInput_OnTutorialPointComplete(bool isLast)
    {       
        //player.FastUp();
        tutorialMode = !isLast;
        if (tutorialMode)
        {
            FindObjectOfType<GameFlowController>().MoveToState(tutorialMovementStateId);
        }
        else
        {
            FindObjectOfType<GameFlowController>().MoveToState(gameStateId);
        }
        OnTutorialEnd.Invoke();
    }

    private void TutorialPoint_OnTutorialPointTrigger(TutorialPoint.TutorialPointInfo tutorialPointInfo)
    {       
        FindObjectOfType<GameFlowController>().MoveToState(tutorialStateId);            
        player.Stop();
        OnTutorialStart.Invoke(tutorialPointInfo);
    }

    private void Player_OnDie()
    {         
        FindObjectOfType<GameFlowController>().MoveToState(loseStateId);
        OnLose?.Invoke();
    }

    public void StartGame()
    {
        if (tutorialMode)
        {
            FindObjectOfType<GameFlowController>().MoveToState(tutorialMovementStateId);
        }
        else
        {
            FindObjectOfType<GameFlowController>().MoveToState(gameStateId);
        }
        player.enabled = true;
        gameStarted = true;
        OnGameStart?.Invoke();
    }

    public void FinishGame()
    {
        FindObjectOfType<GameFlowController>().MoveToState(winStateId);
        OnWin?.Invoke();
    }


    // Update is called once per frame
    void Update()
    {
        levelProgress = player.transform.position.z / finish.transform.position.z;
        if(levelProgress >= 1f && !player.Finished)
        {
            FindObjectOfType<GameFlowController>().MoveToState(gameFinishStateId);
            player.Finish();
        }
    }

    private void OnDisable()
    {
        TutorialPoint.OnTutorialPointTrigger -= TutorialPoint_OnTutorialPointTrigger;
        TutorialInput.OnTutorialPointComplete -= TutorialInput_OnTutorialPointComplete;
    }
}
