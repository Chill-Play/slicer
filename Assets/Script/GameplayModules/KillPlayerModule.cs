using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Core;

public class KillPlayerModule : GameplayModule
{

    public override void Initialize()
    {
        base.Initialize();
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.OnCollision += Player_OnCollision;
    }


    private void Player_OnCollision()
    {
        if (Enabled)
        {
            Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
            player.Kill();
        }
    }


    public override void End()
    {
        base.End();
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.OnCollision -= Player_OnCollision;
    }
}
