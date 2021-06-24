using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlate : MonoBehaviour
{
    [SerializeField] private Vector3 _movement;
    [SerializeField] private float _speed = 5;
        
    private Vector3 _targetPosition;
    private bool _move = false;

    private void Start() 
    {
        _targetPosition = gameObject.transform.position + _movement;
    }
    private void FixedUpdate() 
    {
        if (_move && transform.position != _targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.fixedDeltaTime);
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        _move = true;
    }
}
