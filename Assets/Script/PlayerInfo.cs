using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : Singleton<PlayerInfo>
{
    public PlayerInventory inventory = new PlayerInventory();
}
