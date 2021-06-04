using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event System.Action OnDie;

    [SerializeField] float speed = 6.0f;
    [SerializeField] float sliceSpeedMultiplier = 0.7f;
    [SerializeField] Knife knifePrefab;
    [SerializeField] List<Knife> knifes;
    [SerializeField] LayerMask groundMask;
    float currentSpeed;
    float targetSpeed;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        targetSpeed = speed;
    }


    void Update()
    {
        if(!this.enabled)
        {
            return;
        }
        UpdateSpeed();
        if(Input.GetMouseButtonDown(0))
        {
            SpawnKnife();
        }
    }

    private void UpdateSpeed()
    {
        bool slicing = false;
        for (int i = 0; i < knifes.Count; i++)
        {
            if (knifes[i].Slicing)
            {
                slicing = true;
                break;
            }
        }
        if (slicing)
        {
            targetSpeed = speed * sliceSpeedMultiplier;
        }
        else
        {
            targetSpeed = speed;
        }
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * 10f);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!this.enabled)
        {
            return;
        }
        rigidbody.velocity = rigidbody.velocity.SetZ(currentSpeed);
    }


    void SpawnKnife()
    {
        Knife instance = Instantiate(knifePrefab, transform.position, Quaternion.identity);
        Bounds newBounds = instance.GetComponent<Collider>().bounds;
        Vector3 offset = Vector3.up * newBounds.size.y / 2f;
        instance.transform.position = transform.position;
        transform.position += offset * 2.2f;
        instance.GetComponent<Joint>().connectedBody = rigidbody;
        instance.Player = this;
        //instance.transform.parent = transform;
        knifes.Add(instance);
    }


    public void RemoveKnife(Knife knife)
    {
        knifes.Remove(knife);
        knife.transform.parent = null;
        Destroy(knife.gameObject, 5.0f);
    }


    void Kill()
    {
        if(!this.enabled)
        {
            return;
        }
        RagdollController ragdoll = GetComponentInChildren<RagdollController>();
        ragdoll.SetRagdollActive(true);
        ragdoll.transform.parent = null;
        this.enabled = false;
        for(int i = knifes.Count - 1; i >= 0; i--)
        {
            knifes[i].Kill();
        }
        rigidbody.velocity = Vector3.zero;
        OnDie?.Invoke();
        //Destroy(this);
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGO = collision.collider.gameObject;
        if (groundMask == (groundMask | (1 << collisionGO.gameObject.layer)))
        {
            float backDot = Vector3.Dot(collision.contacts[0].normal, Vector3.back);
            float downDot = Vector3.Dot(collision.contacts[0].normal, Vector3.down);
            if (backDot > 0.5f || downDot > 0.5f)
            {
                Kill();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionGO = other.gameObject;
        if (groundMask == (groundMask | (1 << collisionGO.gameObject.layer)))
        {
            Kill();
        }
    }
}
