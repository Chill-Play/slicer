using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event System.Action OnGameStart;
    public event System.Action OnWin;
    public event System.Action OnLose;
    [SerializeField] Player player;
    [SerializeField] Transform finish;
    float levelProgress;
    bool gameStarted;

    public float LevelProgress => levelProgress;

    // Start is called before the first frame update
    void Start()
    {
        //player.enabled = false;
        player.OnDie += Player_OnDie;
    }

    private void Player_OnDie()
    {
        OnLose?.Invoke();
    }

    public void StartGame()
    {
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

        levelProgress = player.transform.position.z / finish.transform.position.z;
        if(levelProgress >= 1f && player.enabled)
        {
            player.enabled = false;
            OnWin?.Invoke();
        }
    }
}
