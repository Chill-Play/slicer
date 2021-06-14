using GameFramework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoadPointInfo
{
    public Vector3 point;
    public Vector3 direction;
}

public class Road : Entity<Road>
{
    [SerializeField] float roadWidth;
    List<Vector3> roadPoints = new List<Vector3>();
    public float RoadWidth => roadWidth;

    protected override void Awake()
    {
        base.Awake();
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        for(int i = 0; i < transform.childCount; i++)
        {
            meshFilters.Add(transform.GetChild(i).GetComponent<MeshFilter>());
        }
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];

        int j = 0;
        while (j < meshFilters.Count)
        {
            combine[j].mesh = meshFilters[j].sharedMesh;
            combine[j].transform = meshFilters[j].transform.localToWorldMatrix;
            //meshFilters[j].gameObject.SetActive(false);

            j++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.GetComponent<MeshCollider>().sharedMesh = CreateCollisionMesh(meshFilters);
        GenerateRoadPoints(transform.GetComponent<MeshCollider>().sharedMesh);
        transform.gameObject.SetActive(true);
    }


    public RoadPointInfo GetRoadPointInfo(Vector3 point)
    {
        RoadPointInfo info = new RoadPointInfo();
        Vector3 direction = Vector3.zero;
        float minDistance = Mathf.Infinity;
        int roadPoint = -1;
        for(int i = 0; i < roadPoints.Count; i++)
        {
            float distance = (roadPoints[i] - point).sqrMagnitude;
            if(distance < minDistance)
            {
                roadPoint = i;
                minDistance = distance;
            }
        }

        if(roadPoint < roadPoints.Count - 1)
        {
            direction = roadPoints[roadPoint + 1] - roadPoints[roadPoint];
        }
        else
        {
            direction = roadPoints[roadPoint] - roadPoints[roadPoint-1];
        }
        info.direction = direction.normalized;
        info.point = roadPoints[roadPoint];
        return info;
    }


    Mesh CreateCollisionMesh(List<MeshFilter> meshFilters)
    {
        Mesh mesh = new Mesh();
        List<int> triangles = new List<int>();
        List<Vector3> vertices = new List<Vector3>();

        for(int i = 0; i < meshFilters.Count; i++)
        {
            CreateCollisionMesh(meshFilters[i].mesh, triangles, vertices, meshFilters[i].transform);
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        return mesh;
    }


    void CreateCollisionMesh(Mesh roadMesh, List<int> triangles, List<Vector3> vertices, Transform meshTransform)
    {
        Vector3[] originalVertices = roadMesh.vertices;
        int[] originalTriangles = roadMesh.triangles;
        for (int i = 0; i < originalTriangles.Length; i += 3)
        {
            Vector3 pointA = originalVertices[originalTriangles[i]];
            Vector3 pointB = originalVertices[originalTriangles[i + 1]];
            Vector3 pointC = originalVertices[originalTriangles[i + 2]];
            pointA = meshTransform.TransformPoint(pointA);
            pointB = meshTransform.TransformPoint(pointB);
            pointC = meshTransform.TransformPoint(pointC);
            Vector3 norm = new Plane(pointA, pointB, pointC).normal;
            if (Vector3.Dot(norm, Vector3.up) > 0.5f)
            {
                int firstPoint = vertices.Count;
                triangles.Add(firstPoint);
                triangles.Add(firstPoint + 1);
                triangles.Add(firstPoint + 2);
                vertices.Add(pointA);
                vertices.Add(pointB);
                vertices.Add(pointC);
            }
        }
    }



    void GenerateRoadPoints(Mesh mesh)
    {
        roadPoints.Clear();
        Vector3[] vertices = mesh.vertices;
        for(int i = 0; i < vertices.Length; i += 6)
        {
            Vector3 middle = Vector3.zero;
            for(int j = 0; j < 6; j++)
            {
                middle += vertices[i + j];
            }
            middle /= 6f;
            roadPoints.Add(middle);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for(int i = 0; i < roadPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(roadPoints[i], roadPoints[i+1]);
        }
    }
}
