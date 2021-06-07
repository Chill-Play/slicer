using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] float joystickSize = 100;
    Vector2 initialPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - initialPosition;
        delta /= joystickSize;
        delta = Vector2.ClampMagnitude(delta, 1f);
        DataVault.Instance.Push<Vector2>(delta, 1488);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DataVault.Instance.Push<Vector2>(Vector2.zero, 1488);
    }
}
