using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameFramework.Core.UnityLifecycleListener;

public class MovePlayerModule : GameplayModule
{
    [SerializeField] DataId inputId;
    DataSupplier<InputInfo> inputInfo = new DataSupplier<InputInfo>();
    float lastInput;


    void OnEnable()
    {
        inputInfo.Id = inputId;
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.Animator.SetTrigger("Start");
    }


    void Update()
    {
        Player player = IoCContainer.Get<EntityService>().GetFirstEntity<Player>();
        player.UpdateSpeed();
    }


    void FixedUpdate()
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
        RoadPointInfo cameraRoadPointInfo = road.GetRoadPointInfo(player.transform.position + roadPointInfo.direction * 10f);
        player.Move(input - lastInput, roadPointInfo.direction, roadPointInfo.point, road.RoadWidth);
        Object.FindObjectOfType<CameraController>().SetRotation(Quaternion.LookRotation(cameraRoadPointInfo.direction));
        lastInput = input;
    }
}
