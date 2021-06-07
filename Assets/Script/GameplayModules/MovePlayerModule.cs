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


    [LifecycleEvent(typeof(UpdateEvent))]
    public void Update()
    {
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.UpdateSpeed();
    }


    [LifecycleEvent(typeof(FixedUpdateEvent))]
    public void FixedUpdate()
    {
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        float input = inputInfo.GetData().inputX;
        if (!inputInfo.GetData().inputActive)
        {
            input = 0f;
            lastInput = 0f;
        }
        player.Move(input - lastInput);
        lastInput = input;
    }
}
