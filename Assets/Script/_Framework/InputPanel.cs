using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GameFramework.Core;


public struct InputInfo 
{
    public float inputX;
    public float inputY;
    public bool inputActive;
}


public class InputPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] float _sensivity = 1f;
    [SerializeField] DataId inputId;

    DataSupplier<InputInfo> inputData = new DataSupplier<InputInfo>();

    Vector2 _start;
    public Vector2 Input { get; private set; }
    Vector2 _delta;
    bool inputActive;


    public void Awake()
    {
        inputData.Id = inputId;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        Input = Vector2.zero;
        _start = eventData.position;
        inputActive = true;
        UpdateInput();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _delta = (eventData.position - _start) * _sensivity;
        Input = _delta;
        UpdateInput();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Input = Vector2.zero;
        inputActive = false;
        UpdateInput();
    }


    void UpdateInput()
    {
        InputInfo info = new InputInfo();
        info.inputX = Input.x;
        info.inputY = Input.y;
        info.inputActive = inputActive;
        inputData.SetData(info);
    }
}
