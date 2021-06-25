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
    Vector3 localVelocity;
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
    Vector3 smoothedVelocity;
    Vector3 smoothedVelocityVelocity;
    Vector3 lastVelocity;
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
        localVelocity = transform.InverseTransformVector(velocity);
        Vector3 animationVelocity = (rigidbody.position - lastPosition) / Time.fixedDeltaTime;
        smoothedVelocityVelocity = -(smoothedVelocity + (animationVelocity - lastVelocity) * 2f);
        smoothedVelocityVelocity = Vector3.ClampMagnitude(smoothedVelocityVelocity, 4f);
        Vector3 damping = -(0.7f) *smoothedVelocityVelocity;
        smoothedVelocity += (smoothedVelocityVelocity + damping) * Time.fixedDeltaTime * 20f;
        animator.SetFloat("VelocityY", smoothedVelocity.y);
        animator.SetFloat("VelocityX", smoothedVelocity.x);
        lastPosition = rigidbody.position;
        lastVelocity = animationVelocity;
    }


    public void Finish()
    {
        if (Finished)
            return;
        Finished = true;
        FinishTarget.OnLastFinishTargetHitted += FinishTarget_OnLastFinishTargetHitted;
        FindObjectOfType<Finish>().CalculateLastTarget(knifes);
        rigidbody.isKinematic = true;
        rigidbody.constraints = RigidbodyConstraints.None;
        FindObjectOfType<CameraController>().SetRotate(false);
        transform.forward = roadDirection;
        animator.SetLayerWeight(1, 0f);
        for (int i = 0; i < knifes.Count; i++)
        {
            animator.transform.parent = null;
            knifes[i].transform.parent = transform;
            knifes[i].Disable();
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

    private void FinishTarget_OnLastFinishTargetHitted()
    {
        FinishTarget.OnLastFinishTargetHitted -= FinishTarget_OnLastFinishTargetHitted;
        StartCoroutine(WinCoroutine()); //Refactor1
        knifeHittedLastTarget = true;
    }

    private void Knife_OnStartSlice()
    {
        knifeHittedLastTarget = true;
    }



    IEnumerator NoKnifesCoroutine()
    {
        float t = 0;
        while(t < 1f)
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
            animator.transform.localRotation = Quaternion.Lerp(animatorStartRotation, Quaternion.identity, (startPos.y - transform.position.y) / startPos.y - 5f);
            transform.up = transform.position - startPos.SetY(0f);
        }
        animator.GetComponentInChildren<InverseKinematics>().enabled = false;
        yield return new WaitForEndOfFrame();
        Transform rightHand = animator.GetComponentInChildren<Character>().RightHand;
        transform.SetParent(rightHand, true);
        float t = 0f;
        transform.up = transform.position - startPos.SetY(0f);
        Quaternion startRotation = transform.rotation;
        Vector3 startLocalPosition = transform.localPosition;
       
        while (t < 0.3f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime * 5f;
        }
        animator.SetTrigger("Throw");
        t = 0f;
        while (t < 0.6f)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime * 1.5f;
            float normalizedT = (t / 0.6f);
            Quaternion targetRotation = Quaternion.Euler((normalizedT * normalizedT * normalizedT) * 150f, 0f, 0f) * startRotation;
            transform.rotation = targetRotation;
        }
        transform.parent = null;
        t = 0f;
        startPos = transform.position;
        Vector3 targetPos = FindObjectOfType<TargetPoint>().transform.position;
        startRotation = transform.rotation;
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
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(transform.position - targetPos, Vector3.down) * Quaternion.LookRotation(Vector3.down), t * 5f);
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
            if (Vector3.Dot(collision.contacts[0].normal, -transform.forward) > 0.5f)
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
            OnCollision?.Invoke();
        }
    }
}
