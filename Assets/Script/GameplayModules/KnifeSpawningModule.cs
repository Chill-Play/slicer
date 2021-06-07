using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameFramework.Core.UnityLifecycleListener;

public class KnifeSpawningModule : GameplayModule
{

    [LifecycleEvent(typeof(UpdateEvent))]
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
            player.SpawnKnife();
        }
    }
}
