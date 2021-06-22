using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity<Player>
{
    public event System.Action OnDie;
    public event System.Action OnCollision;

    [SerializeField] float speed = 6.0f;
    [SerializeField] float sliceSpeedMultiplier = 0.7f;
    [SerializeField] float rightSpeed = 100f;
    [SerializeField] Knife knifePrefab;
    [SerializeField] List<Knife> knifes;
    [SerializeField] LayerMask groundMask;
    [SerializeField] Animator animator;
    [SerializeField] bool rotateWithInput;
    [SerializeField] float additionalGravity;
    KnifeSkin skin;
    Vector3 roadDirection = Vector3.forward;
    Vector3 roadPoint;

    public bool Finished { get; set; }
    public Animator Animator => animator;

    float currentSpeed;
    float targetSpeed;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        targetSpeed = speed;
        skin = FindObjectOfType<KnifeStorage>().CurrentSkin;
        Instantiate(skin.Handle, transform.position + skin.HandleOffset, transform.rotation * Quaternion.Euler(skin.Handle.transform.eulerAngles) ,transform);
    }


    public void UpdateSpeed()
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


    private void FixedUpdate()
    {
        rigidbody.AddForce(Vector3.down * additionalGravity, ForceMode.Acceleration);
    }


    // Update is called once per frame
    Vector3 lastRoadPoint;
    Vector3 lastPosition;
    float angleSmoothVelocity;
    public void Move(float input, Vector3 direction, Vector3 roadPoint,float roadWidth)
    {
        if (!this.enabled)
        {
            return;
        }
        roadDirection = direction;
        Vector3 playerPositionOnRoad = rigidbody.position.SetY(roadPoint.y);
        Vector3 projectedRoadPoint = roadPoint + Vector3.Dot(playerPositionOnRoad - roadPoint, direction) / Vector3.Dot(direction, direction) * direction;
        Vector3 forwardVector = direction * currentSpeed;
        Vector3 leftVector = Vector3.Cross(direction, Vector3.up) * input * rightSpeed;
        rigidbody.rotation = Quaternion.RotateTowards(rigidbody.rotation, Quaternion.LookRotation(direction), Time.fixedDeltaTime * 90f);
        for (int i = 0; i < knifes.Count; i++)
        {
            knifes[i].SetRotation(rigidbody.rotation);
        }
        Vector3 velocity = forwardVector + -leftVector;//rigidbody.velocity;
        Vector3 nextPosition = playerPositionOnRoad + (velocity * Time.fixedDeltaTime);

        Vector3 directionFromRoadPoint = (nextPosition - projectedRoadPoint).normalized; 
        float distanceToRoadPoint = Vector3.Distance(nextPosition, projectedRoadPoint);

        Vector3 roadRightDirection = Vector3.Cross(direction, Vector3.up);
        float dot = Vector3.Dot(roadRightDirection, directionFromRoadPoint);
        distanceToRoadPoint *= dot;
        float offset = 1f;
        if(Mathf.Abs(distanceToRoadPoint) > roadWidth - offset)
        {
            velocity += roadRightDirection * ((roadWidth - offset) - Mathf.Abs(distanceToRoadPoint)) * Mathf.Sign(distanceToRoadPoint) / Time.fixedDeltaTime;
            
        }

        //Vector3 projection = Vector3.Dot(rigidbody.position - roadPoint, direction) / Vector3.Dot(direction, direction) * direction;
        Vector3 forward = ((rigidbody.position + velocity) - lastPosition).SetY(0f);
        if (rotateWithInput)
        {
            //rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, Quaternion.LookRotation(forward), Time.fixedDeltaTime * 25f);
            Quaternion rotation = rigidbody.rotation;

            float delta = Quaternion.Angle(rotation, Quaternion.LookRotation(forward));
            if (delta > 0f)
            {
                float t = Mathf.SmoothDampAngle(delta, 0.0f, ref angleSmoothVelocity, 0.02f, Mathf.Infinity, Time.fixedDeltaTime);
                t = 1.0f - (t / delta);
                rotation = Quaternion.Slerp(rotation, Quaternion.LookRotation(forward), t);
            }

            rigidbody.rotation = rotation;
        }

        this.roadPoint = roadPoint;
        lastRoadPoint = projectedRoadPoint;
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;
        lastPosition = rigidbody.position;
    }


    public void Finish()
    {
        if (Finished)
            return;
        Finished = true;
        FinishTarget.OnLastFinishTargetHitted += FinishTarget_OnLastFinishTargetHitted;
        FindObjectOfType<Finish>().CalculateLastTarget(knifes.Count);
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints.None;
        FindObjectOfType<CameraController>().SetRotate(false);
        transform.forward = roadDirection;
        for (int i = 0; i < knifes.Count; i++)
        {
            animator.transform.parent = null;
            knifes[i].transform.parent = transform;
            knifes[i].Disable();
            if (i > 0)
            {
                knifes[i].DestroyOnSlice = true;
            }
            else
            {
                knifes[i].CanSlice = false;
                knifes[i].OnStartSlice += Knife_OnStartSlice;
            }
        }
        StartCoroutine(FinishCoroutine());
    }

    private void FinishTarget_OnLastFinishTargetHitted()
    {
        FinishTarget.OnLastFinishTargetHitted -= FinishTarget_OnLastFinishTargetHitted;
        StartCoroutine(WinCoroutine()); //Refactor1
        knifeHittedLastTarget = true;
        for (int i = 0; i < knifes.Count; i++)
        {
            knifes[i].CanSlice = false;
        }
    }

    private void Knife_OnStartSlice()
    {
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
        Vector3 startAngle = transform.eulerAngles;
        Vector3 startOffset = animator.transform.position - startPos.SetY(0f);
        while(animator.transform.position.y > 0f)
        {
            yield return new WaitForEndOfFrame();
            playerAcceleration += Physics.gravity * Time.deltaTime * 2f;
            playerSpeed += playerAcceleration * Time.deltaTime * 6f;
            animator.transform.position += playerSpeed * Time.deltaTime;
            if(animator.transform.position.y < 0f)
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
        for(int j = 0; j < knifes.Count; j++)
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
            if(knifeHittedLastTarget)
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


    public void SpawnKnife()
    {
        Knife instance = Instantiate(knifePrefab, transform.position, Quaternion.identity);
        instance.SetSkin(skin);
        Bounds newBounds = instance.GetComponent<Collider>().bounds;
        Vector3 offset = Vector3.up * newBounds.size.y / 2f;
        instance.transform.position = transform.position;
        transform.position += offset * 2.2f;
        instance.GetComponent<Joint>().connectedBody = rigidbody;
        instance.Player = this;
        //instance.transform.parent = transform;
        knifes.Insert(0, instance);
    }


    void UpdateKnife(Knife knife)
    {
        knife.SetRotation(rigidbody.rotation);
        Vector3 velocity = rigidbody.position - knife.transform.position;
        knife.SetVelocity(velocity);
    }


    public void RemoveKnife(Knife knife)
    {
        knifes.Remove(knife);
        knife.transform.parent = null;
        Destroy(knife.gameObject, 5.0f);
    }


    public void Kill()
    {
        RagdollController ragdoll = GetComponentInChildren<RagdollController>();
        ragdoll.SetRagdollActive(true);
        ragdoll.transform.parent = null;
        for(int i = knifes.Count - 1; i >= 0; i--)
        {
            knifes[i].Kill();
        }
        rigidbody.velocity = Vector3.zero;
        OnDie?.Invoke();
        //Destroy(this);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(lastRoadPoint, Vector3.one * 0.3f);
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
}
