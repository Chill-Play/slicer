using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlicedObjectsGroup : MonoBehaviour
{
    [SerializeField] float minimalVelocity = 0.8f;
    [SerializeField] float step = 0.1f;
    [SerializeField] int count = 50;
    [SerializeField] Vector3 axis = Vector3.forward;
    [SerializeField] SlicedObject cardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        ISlicedObjectsGroupModifier[] modifiers = GetComponents<ISlicedObjectsGroupModifier>();
        for(int i = 0; i < count; i++)
        {
            Vector3 worldAxis = transform.TransformDirection(axis);
            Vector3 pos = transform.position + worldAxis * step * i;
            SlicedObject slicedObject = Instantiate(cardPrefab, pos, Quaternion.LookRotation(worldAxis), transform);
            float slicePower = 1.0f - ((float)i / transform.childCount);
            slicePower = Mathf.Clamp(slicePower, minimalVelocity, 1.0f);
            slicedObject.SliceVelocityPower = slicePower;
            for(int j = 0; j < modifiers.Length; j++)
            {
                SpawnInfo info = new SpawnInfo();
                info.number = i;
                info.total = count;
                info.spawnedObject = slicedObject;
                modifiers[j].OnSpawn(info);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        float axisSize = step*count;
        Vector3 size = axis * step*count + new Vector3 (3f, 1f, 0f);
        Vector3 delta = axis * step*count*0.5f;
        Gizmos.DrawWireCube(transform.position + delta, size);
    }
}
