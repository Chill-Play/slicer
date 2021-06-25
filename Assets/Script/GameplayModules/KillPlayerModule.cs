using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;

public class KillPlayerModule : GameplayModule
{
    bool playerKilled = false;
    public void OnEnable()
    {
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.OnCollision += Player_OnCollision;
    }


    private void Player_OnCollision()
    {
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        if (!player.Finished)
        {
            player.Kill();
            playerKilled = true;
        }
    }


    public void OnDisable()
    {
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        if (player != null)
        {
            player.OnCollision -= Player_OnCollision;
        }
    }
}
