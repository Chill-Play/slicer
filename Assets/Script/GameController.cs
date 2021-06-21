using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event System.Action OnGameStart;
    public event System.Action OnWin;
    public event System.Action OnLose;

    [SerializeField] SubjectId gameStateId;
    [SerializeField] SubjectId winStateId;
    [SerializeField] SubjectId loseStateId;

    Player player;
    bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        FindObjectOfType<CameraController>().Target = player.transform;
        //player.enabled = false;
        player.OnDie += Player_OnDie;
    }

    private void Player_OnDie()
    {
        OnLose?.Invoke();
        FindObjectOfType<GameFlowController>().MoveToState(loseStateId);
    }

    public void StartGame()
    {
        FindObjectOfType<GameFlowController>().MoveToState(gameStateId);
        player.enabled = true;
        gameStarted = true;
        OnGameStart?.Invoke();
    }



    // Update is called once per frame
    void Update()
    {
        if(!gameStarted)
        {
            if(Input.GetMouseButtonDown(0))
            {
                StartGame();
            }
        }

        //levelProgress = player.transform.position.z / finish.transform.position.z;
        //if(levelProgress >= 1f && player.enabled)
        //{
        //    player.enabled = false;
        //    OnWin?.Invoke();
        //}
    }
}
