using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private MeshFilter meshfilter;
    private Mesh mesh;
    private Vector3[] verts;
    private Vector3[] uvs;
    private int[] tris;
    [SerializeField] private int width;
    [SerializeField] private int length;
    [SerializeField] private float spacing = 0.5f;
    [SerializeField] private float waveHeight = 1;
    [SerializeField] private float waveSpeed = 10;
    [SerializeField] private float strechX = 5;
    [SerializeField] private float strechY = 1;

    float animA = 0;
    float animB = 0;

    private void Start()
    {
        meshfilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        GeneratePlane();
    }

    private void Update()
    {
        AnimateWater(animA, animB);
        animA += waveSpeed * Time.deltaTime;
        animB += waveSpeed * Time.deltaTime;
    }

    void GeneratePlane()
    {
        //Generate vertices
        verts = new Vector3[(length + 1) * (width + 1)];
        for (int a = 0, i = 0; i <= width; i++)
        {
            for (int j = 0; j <= length; j++)
            {
                verts[a] = new Vector3(j * spacing, 0, i * spacing);
                a++;
            }
        }
        //Generate triangles
        tris = new int[length * width * 6];
        for (int vt = 0, tr = 0, j = 0; j < width; j++)
        {
            for (int i = 0; i < length; i++)
            {
                tris[tr] = vt;
                tris[tr + 1] = vt + length + 1;
                tris[tr + 2] = vt + 1;
                tris[tr + 3] = vt + 1;
                tris[tr + 4] = vt + length + 1;
                tris[tr + 5] = vt + length + 2;

                vt++;
                tr += 6;
            }
            vt++;
        }
        
        mesh.vertices = verts;
        mesh.triangles = tris;
        meshfilter.mesh = mesh;
    }

    
    private void OnDrawGizmos()
    {
        if (verts != null)
        {
            foreach (Vector3 v in verts)
            {
                Gizmos.DrawSphere(transform.position + v, 0.1f);
            }
        }       
    }

    void AnimateWater(float a, float b)
    {
        for (int i = 0; i < (length + 1) * (width + 1); i+= length + 1)
        {
            for (int j = 0; j < length + 1; j++)
            {
                verts[i + j] = new Vector3(verts[i + j].x,waveHeight * (Mathf.Sin((1 / strechY) * (verts[i + j].x + a)) + Mathf.Sin((1/strechX) * (verts[i + j].z + b))), verts[i + j].z);
            }
        }
        mesh.vertices = verts;
    }

    public Vector3[] GetVerts()
    {
        return verts;
    }

    public float GetSpacing()
    {
        return spacing;
    }

    public int GetLength()
    {
        return length;
    }
}
