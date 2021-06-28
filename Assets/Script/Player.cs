﻿using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : Entity<Player>
{
    public event System.Action OnDie;
    public event System.Action OnCollision;

    [SerializeField] float speed = 6.0f;
    [SerializeField] float sliceSpeedMultiplier = 0.7f;
    [SerializeField] float rightSpeed = 100f;
    [SerializeField] Animator animator;
    [SerializeField] Knife knifePrefab;
    [SerializeField] List<Knife> knifes;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float onKillPushBackPower = 5f;
    [SerializeField] float additionalGravity = 10f;
    [SerializeField] float maxSlowDownSpeed = 0.5f;
    [SerializeField] float slowDownSpeedScaleingTime = 0.7f;
    [SerializeField] float speedRecoveryTime = 1f;



    Vector3 roadDirection = Vector3.forward;

    float currentSpeed;
    float targetSpeed;
    Rigidbody body;
    KnifeSkin skin;
    bool updateSpeed = true;   
    float slowDownBeginSpeed;

    public bool Finished { get; set; }
    public Animator Animator => animator;

    // Start is called before the first frame update
    void Start()
    {       
        body = GetComponent<Rigidbody>();
        skin = FindObjectOfType<KnifeStorage>().CurrentSkin;
        Instantiate(skin.Handle, transform.position + skin.HandleOffset, transform.rotation * Quaternion.Euler(skin.Handle.transform.eulerAngles), transform);
        targetSpeed = speed;
    }


    public void UpdateSpeed()
    {
        if (updateSpeed)
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
    }


    // Update is called once per frame
    public void Move(float input)
    {
        if (!this.enabled || Finished)
        {
            return;
        }
        Vector3 forwardVector = roadDirection * currentSpeed;
        Vector3 leftVector = Vector3.Cross(roadDirection, Vector3.up) * input * rightSpeed;
        //rigidbody.rotation = Quaternion.LookRotation(globalDirection);
        Vector3 velocity = forwardVector + -leftVector;//rigidbody.velocity;
        velocity.y = body.velocity.y;
        body.velocity = velocity;
    }

    public void Stop()
    {
        currentSpeed = 0f;
        body.velocity = Vector3.zero;
        for (int i = 0; i < knifes.Count; i++)
        {
            knifes[i].Stop();
        }
    }

    public void SlowDown()
    {

        updateSpeed = false;
        slowDownBeginSpeed = currentSpeed;
        DOTween.To(() => currentSpeed, (x) => currentSpeed = x, maxSlowDownSpeed, slowDownSpeedScaleingTime); 
    }
    public void FastUp()
    {
        DOTween.To(() => currentSpeed, (x) => currentSpeed = x, slowDownBeginSpeed, speedRecoveryTime).OnComplete(() => updateSpeed = true);       
    }

    private void FixedUpdate()
    {
        body.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
    }


    public void SpawnKnife()
    {        
            Knife instance = Instantiate(knifePrefab, transform.position, Quaternion.identity);
            instance.SetSkin(skin);
            Bounds newBounds = instance.GetComponent<Collider>().bounds;
            Vector3 offset = Vector3.up * newBounds.size.y / 2f;
            instance.transform.position = transform.position;
            transform.position += offset * 2.2f;
            instance.GetComponent<Joint>().connectedBody = body;
            instance.Player = this;
            instance.CanSlice = true;
            //instance.transform.parent = transform;
            knifes.Insert(0, instance);       
    }


    public void RemoveKnife(Knife knife)
    {
        knifes.Remove(knife);
        knife.transform.parent = null;
        Destroy(knife.gameObject, 5.0f);
    }


    public void Kill()
    {
        //enabled = false;
        Finished = true;
        //TestGameSetup.instance.RemoveGameplayModule<KnifeSpawningModule>();
        RagdollController ragdoll = GetComponentInChildren<RagdollController>();
        ragdoll.SetRagdollActive(true);
        ragdoll.transform.parent = null;
        for(int i = knifes.Count - 1; i >= 0; i--)
        {
            knifes[i].Kill();
        }
        body.velocity = Vector3.zero;
        ragdoll.Push(Vector3.back * onKillPushBackPower, ragdoll.transform.position);
        body.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
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
                OnCollision?.Invoke();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionGO = other.gameObject;
        if (groundMask == (groundMask | (1 << collisionGO.gameObject.layer)))
        {
            OnCollision?.Invoke();
        }
    }

    public void Finish()
    {
        
        if (Finished)
            return;

    

        Finished = true;      
        FinishTarget.OnLastFinishTargetHitted += FinishTarget_OnLastFinishTargetHitted;       
        FindObjectOfType<Finish>().CalculateLastTarget(knifes);
        body.isKinematic = true;
        body.constraints = RigidbodyConstraints.None;
        FindObjectOfType<CameraController>().SetRotate(false);
        transform.forward = roadDirection;
        for (int i = 0; i < knifes.Count; i++)
        {
            animator.transform.parent = null;
            knifes[i].transform.parent = transform;
            knifes[i].Disable();
            knifes[i].transform.localPosition = knifes[i].transform.localPosition.SetY(-(i + 1) * knifes[i].KnifeLength);
            knifes[i].CanSlice = false;
        }
        if (knifes.Count > 0)
        {
            StartCoroutine(FinishCoroutine());
        }
        else
        {
            StartCoroutine(NoKnifesCoroutine());
        }
    }


    IEnumerator NoKnifesCoroutine()
    {
        float t = 0;
        while (t < 1f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            transform.position += roadDirection * Mathf.Lerp(speed, 0f, t) * Time.deltaTime;
        }

        t = 0;
        Vector3 startPos = animator.transform.localPosition;
        Quaternion startRot = animator.transform.localRotation;
        while (t < 1f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime * 4f;
            animator.transform.localPosition = Vector3.Lerp(startPos, new Vector3(-0.672f, -0.3f, -0.06f), t);
            animator.transform.localRotation = Quaternion.Lerp(startRot, Quaternion.identity, t);
        }
        animator.SetTrigger("Defeated");
        yield return new WaitForSeconds(1f);
        StartCoroutine(WinCoroutine());
    }


    private void FinishTarget_OnLastFinishTargetHitted()
    {
        FinishTarget.OnLastFinishTargetHitted -= FinishTarget_OnLastFinishTargetHitted;
        knifeHittedLastTarget = true;       
    }


    bool knifeHittedLastTarget;
    IEnumerator FinishCoroutine()
    {
        Debug.Log("Finish Coroutine");
        yield return new WaitForEndOfFrame();
        Vector3 startPos = transform.position;
        Vector3 jumpSpeed = Vector3.up * 0.1f + roadDirection * 0.45f;
        jumpSpeed *= knifes.Count;
        Vector3 playerSpeed = jumpSpeed + (Vector3.forward * speed);
        Vector3 playerAcceleration = Vector3.zero;
        Quaternion animatorStartRotation = animator.transform.localRotation;
        Vector3 startOffset = animator.transform.position - startPos.SetY(0f);
        while (animator.transform.position.y > 0f)
        {
            yield return new WaitForEndOfFrame();
            playerAcceleration += Physics.gravity * Time.deltaTime * 2f;
            playerSpeed += playerAcceleration * Time.deltaTime * 6f;
            animator.transform.position += playerSpeed * Time.deltaTime;
            if (animator.transform.position.y < 0f)
            {
                animator.transform.position = animator.transform.position.SetY(0f);
            }
            Vector3 directionToAnimator = (animator.transform.position + (Vector3.up * 0.7f)) - startPos.SetY(0);
            directionToAnimator.Normalize();
            float angle = Vector3.SignedAngle(Vector3.up, directionToAnimator, transform.right);
            transform.position = startPos + Vector3.up * (angle) / 90f;
            Vector3 currentOffset = startPos.SetY(0) - animator.transform.position;
            animator.transform.position += (currentOffset.normalized) * (currentOffset.magnitude - startOffset.magnitude);
            transform.RotateAround(startPos.SetY(0f), Vector3.right, angle);
            animator.transform.localRotation = Quaternion.Lerp(animatorStartRotation, Quaternion.identity, (startPos.y - transform.position.y) / startPos.y - 5f);
            transform.up = transform.position - startPos.SetY(0f);
        }
        animator.GetComponentInChildren<InverseKinematics>().enabled = false;
        yield return new WaitForEndOfFrame();
        Transform rightHand = animator.GetComponentInChildren<Character>().RightHand;
        transform.SetParent(rightHand, true);
        float t = 0f;
        transform.up = transform.position - startPos.SetY(0f);
        Quaternion startLocalRotation = transform.localRotation;
        Vector3 startLocalPosition = transform.localPosition;

        while (t < 1f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime * 5f;
            transform.localRotation = Quaternion.Lerp(startLocalRotation, Quaternion.LookRotation(Vector3.right) * Quaternion.LookRotation(Vector3.forward) * Quaternion.Euler(0f, -90f, 0f), t);
            transform.localPosition = Vector3.Lerp(startLocalPosition, new Vector3(0f, -0.01151f, 0f), t);
        }
        animator.SetTrigger("Throw");
        yield return new WaitForSeconds(0.45f);
        transform.parent = null;
        t = 0f;
        startPos = transform.position;
        Vector3 targetPos = FindObjectOfType<TargetPoint>().transform.position;
        startLocalRotation = transform.rotation;
        CameraController cameraController = FindObjectOfType<CameraController>();
        cameraController.Target = transform;
        for (int j = 0; j < knifes.Count; j++)
        {
            Rigidbody rb = knifes[j].gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
        while (t < 1f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime * 0.8f;
            transform.rotation = Quaternion.Lerp(startLocalRotation, Quaternion.LookRotation(transform.position - targetPos) * Quaternion.LookRotation(Vector3.down), t * 5f);
            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, t);
            nextPos += Vector3.up * Mathf.Sin(t * Mathf.PI);
            cameraController.ApplyShakeOffset(new Vector3(Mathf.PerlinNoise(t * 10f, t * 10f),
                Mathf.PerlinNoise(t * 100f, t * 100f),
                Mathf.PerlinNoise(t * 1000f, t * 1000f)
                * 0.15f));
            if (knifeHittedLastTarget)
            {
                StartCoroutine(WinCoroutine());
                break;
            }
            //transform.up = transform.position - nextPos;
            transform.position = nextPos;
        }
    }


    IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<GameController>().FinishGame();
    }
}
