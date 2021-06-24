using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlate : MonoBehaviour
{
    [SerializeField] Vector3 _movement;
    [SerializeField] float _speed = 5f;
    [SerializeField] bool loopMovement = false;
    [SerializeField] Rigidbody plate;

    Vector3 _targetPosition;
    bool _move = false;
    bool positiveDirection = false;

    private void Start() 
    {
        _targetPosition = plate.position + _movement;
        positiveDirection = true;
    }

    private void FixedUpdate() 
    {
        if (_move)
        {
            Vector3 dir = _targetPosition - plate.position;
            if (dir.magnitude >= 0.1f)
            {
                Debug.Log("MOVE");
                dir.Normalize();
                plate.MovePosition(plate.position + dir * _speed * Time.fixedDeltaTime);
            }
            else if (loopMovement)
            {
                if (positiveDirection)
                {
                    _targetPosition -= _movement;
                    positiveDirection = false;
                }
                else
                {
                    _targetPosition += _movement;
                    positiveDirection = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null || other.gameObject.GetComponent<Knife>() != null)
        {          
            _move = true;
        }
    }
}
