using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;

public class KillPlayerModule : GameplayModule
{
    bool playerKilled = false;
    public override void Initialize()
    {
        base.Initialize();
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.OnCollision += Player_OnCollision;
    }


    private void Player_OnCollision()
    {
        if (Enabled && !playerKilled)
        {
            Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
            if (!player.Finished)
            {
                player.Kill();
                playerKilled = true;
            }
        }
    }


    public override void End()
    {
        base.End();
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.OnCollision -= Player_OnCollision;
    }
}
