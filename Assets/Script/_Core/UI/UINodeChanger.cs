using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UINodeChanger : ScriptableObject
{
    public abstract void Apply(IEnumerator<UINode> nodes);   

}
