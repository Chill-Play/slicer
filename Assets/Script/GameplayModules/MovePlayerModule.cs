using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameFramework.Core.UnityLifecycleListener;

public class MovePlayerModule : GameplayModule
{
    DataSupplier<InputInfo> inputInfo = new DataSupplier<InputInfo>();
    float lastInput;
    public MovePlayerModule(DataId inputData)
    {
        inputInfo.Id = inputData;
    }


    public override void Initialize()
    {
        base.Initialize();
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.Animator.SetTrigger("Start");
    }


    [LifecycleEvent(typeof(UpdateEvent))]
    public void Update()
    {
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.UpdateSpeed();
    }


    [LifecycleEvent(typeof(FixedUpdateEvent))]
    public void FixedUpdate()
    {
        Road road = IoCContainer.Get<EntityService>().GetFirstEntity<Road>();
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        if (player.Finished) return;
        float input = inputInfo.GetData().inputX;
        if (!inputInfo.GetData().inputActive)
        {
            input = 0f;
            lastInput = 0f;
        }
        RoadPointInfo roadPointInfo = road.GetRoadPointInfo(player.transform.position);
        player.Move(input - lastInput, roadPointInfo.direction, roadPointInfo.point, road.RoadWidth);
        lastInput = input;
    }
}
