using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    struct Line
    {
        public Vector2 pointA;
        public Vector2 pointB;


        public Line(Vector2 a, Vector2 b)
        {
            pointA = a;
            pointB = b;
        }
    }

    public event System.Action OnStartSlice;

    [SerializeField] Collider sliceCollider;
    [SerializeField] LayerMask itemMask;
    [SerializeField] GameObject skin;
    [SerializeField] LayerMask groundMask;
    int sliceObjects;

    Rigidbody rigidbody;
    public bool Slicing => sliceObjects > 0;
    public Player Player { get; set; }
    public bool DestroyOnSlice { get; set; }
    public bool CanSlice { get; set; } = true;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        
    }


    public void SetRotation(Quaternion rotation)
    {
        rigidbody.rotation = rotation;
    }


    public void SetVelocity(Vector3 velocity)
    {
        rigidbody.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (itemMask == (itemMask | (1 << other.gameObject.layer)) && other.enabled)
        {
            sliceObjects++;
            OnStartSlice?.Invoke();
        }
    }


    bool sliced;
    private void OnTriggerStay(Collider other)
    {
        if(DestroyOnSlice && sliced)
        {            
            return;
        }
        ISlicable slicable = other.GetComponent<ISlicable>();
        if(slicable != null)
        {          
            //float penetration = sliceCollider.bounds.max.z - other.bounds.min.z;
            //Physics.ComputePenetration(sliceCollider, transform.position, transform.rotation, other, other.transform.position, other.transform.rotation, out Vector3 direction, out float distance);
            float penetration = ComputePenetration(other.bounds, transform.forward, transform.position + (transform.forward * sliceCollider.bounds.extents.z));
            Bounds bounds;
            if (CanSlice)
            {
                bool sliced = slicable.TryToSlice(penetration, transform.position, transform.right);
                if (sliced)
                {
                    if (DestroyOnSlice)
                    {
                        sliced = true;
                        Destroy(gameObject);
                        Kill();
                    }
                    sliceObjects--;
                }           
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGO = collision.collider.gameObject;
        if (groundMask == (groundMask | (1 << collisionGO.gameObject.layer)))
        {
            float upDot = Vector3.Dot(collision.contacts[0].normal, Vector3.up);
            float downDot = Vector3.Dot(collision.contacts[0].normal, Vector3.down);
            if (upDot < 0.3f || downDot > 0.5f)
            {
                Kill();
            }
        }
    }


    public float ComputePenetration(Bounds bounds, Vector3 direction, Vector3 point)
    {
        Vector2 point2d = To2DPlane(point);
        Vector2 direction2d = To2DPlane(direction);
        Line intersectionLine = new Line(To2DPlane(point - direction * 1000), To2DPlane(point + direction * 1000));
        bool bottom = LineToLine(new Line(new Vector2(bounds.min.x, bounds.min.z), new Vector2(bounds.max.x, bounds.min.z)), intersectionLine, out Vector2 intersectionB);
        bool right = LineToLine(new Line(new Vector2(bounds.max.x, bounds.min.z), new Vector2(bounds.max.x, bounds.max.z)), intersectionLine, out Vector2 intersectionR);
        bool top = LineToLine(new Line(new Vector2(bounds.max.x, bounds.max.z), new Vector2(bounds.min.x, bounds.min.z)), intersectionLine, out Vector2 intersectionT);
        bool left = LineToLine(new Line(new Vector2(bounds.min.x, bounds.max.z), new Vector2(bounds.min.x, bounds.min.z)), intersectionLine, out Vector2 intersectionL);

        float maxDistance = 0f;
        if(bottom)
        {
            float distance = ComputeDistanceToIntersection(point2d, intersectionB, direction2d);
            if (maxDistance < distance)
            {
                maxDistance = distance;
            }
        }
        if (right)
        {
            float distance = ComputeDistanceToIntersection(point2d, intersectionR, direction2d);
            if (maxDistance < distance)
            {
                maxDistance = distance;
            }
        }
        if (top)
        {
            float distance = ComputeDistanceToIntersection(point2d, intersectionT, direction2d);
            if (maxDistance < distance)
            {
                maxDistance = distance;
            }
        }
        if (left)
        {
            float distance = ComputeDistanceToIntersection(point2d, intersectionL, direction2d);
            if (maxDistance < distance)
            {
                maxDistance = distance;
            }
        }
        return maxDistance;
    }


    float ComputeDistanceToIntersection(Vector2 point2d, Vector2 intersection, Vector2 direction)
    {
        float distance = Vector3.Distance(point2d, intersection);
        float dot = Vector3.Dot(direction, (intersection - point2d).normalized);
        return distance * -dot;
    }


    bool LineToLine(Line lineA, Line lineB, out Vector2 intersection)
    {
        intersection = Vector2.zero;
        // calculate the direction of the lines
        float uA = ((lineB.pointB.x - lineB.pointA.x) * (lineA.pointA.y - lineB.pointA.y) - (lineB.pointB.y - lineB.pointA.y) * (lineA.pointA.x - lineB.pointA.x)) / ((lineB.pointB.y - lineB.pointA.y) * (lineA.pointB.x - lineA.pointA.x) - (lineB.pointB.x - lineB.pointA.x) * (lineA.pointB.y - lineA.pointA.y));
        float uB = ((lineA.pointB.x - lineA.pointA.x) * (lineA.pointA.y - lineB.pointA.y) - (lineA.pointB.y - lineA.pointA.y) * (lineA.pointA.x - lineB.pointA.x)) / ((lineB.pointB.y - lineB.pointA.y) * (lineA.pointB.x - lineA.pointA.x) - (lineB.pointB.x - lineB.pointA.x) * (lineA.pointB.y - lineA.pointA.y));

        // if uA and uB are between 0-1, lines are colliding
        if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
        {

            // optionally, draw a circle where the lines meet
            float intersectionX = lineA.pointA.x + (uA * (lineA.pointB.x - lineA.pointA.x));
            float intersectionY = lineA.pointA.y + (uA * (lineA.pointB.y - lineA.pointA.y));
            intersection = new Vector2(intersectionX, intersectionY);
            return true;
        }
        return false;
    }


    Vector2 To2DPlane(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }


    public void Disable()
    {
        //rigidbody.isKinematic = true;
        //rigidbody.constraints = RigidbodyConstraints.None;
        Destroy(GetComponent<Joint>());
        Destroy(rigidbody);
    }


    public void SetSkin(KnifeSkin newSkin)
    {
        Destroy(skin);
        skin = Instantiate(newSkin.Blade, transform.position + newSkin.BladeOffset, Quaternion.identity, transform);
        skin.transform.rotation = Quaternion.LookRotation(newSkin.BladeForwardAxis) * transform.rotation;
        skin.transform.localScale = newSkin.Blade.transform.localScale;
    }

    public void Kill()
    {
        Destroy(GetComponent<Joint>());
        Player.RemoveKnife(this);
        if(rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        rigidbody.constraints = RigidbodyConstraints.None;
        GetComponent<Collider>().material = null;
        Destroy(this);
    }
}
