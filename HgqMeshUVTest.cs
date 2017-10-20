using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshPoint
{
    /// <summary>
    /// MESH中顶点的序号
    /// </summary>
    public int index;
    /// <summary>
    /// 未纠正的UV可能大于1或者为负
    /// </summary>
    public Vector2 uv;
    /// <summary>
    /// 顶点
    /// </summary>
    public Vector3 vertex;

    /// <summary>
    /// 是否纠正UV标识
    /// </summary>
    public bool isCorrectUV = false;
    /// <summary>
    /// 纠正后的UV
    /// </summary>
    public Vector2 correctUV;
    /// <summary>
    /// 距离计算点的长度，为float*10000 获取的整数，用于点的排序
    /// </summary>
    public int length;

}

public class MeshTriangle
{
    public MeshPoint p0;
    public MeshPoint p1;
    public MeshPoint p2;
}

public class HgqMeshUVTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        MeshFilter mf = this.GetComponentInChildren<MeshFilter>();
        Vector2[] uvs = mf.mesh.uv;

        Debug.Log(mf.mesh.uv.Length);
        Debug.Log(mf.mesh.vertices.Length);

        for (int i = 0; i < uvs.Length; i++)
        {
            Debug.Log(uvs[i]);
        }
        //uvs[0] = new Vector2(0f, 0f);
        //uvs[1] = new Vector2(4f, 5f);
        //uvs[2] = new Vector2(4f, 0);
        //uvs[3] = new Vector2(0, 5f);

        //uvs[0] = new Vector2(2f, 0f);
        //uvs[1] = new Vector2(0f, 2f);
        //uvs[2] = new Vector2(0f, 0f);
        //uvs[3] = new Vector2(2f, 2f);

        uvs[0] = new Vector2(0f, 0f);
        uvs[1] = new Vector2(2f, 2f);
        uvs[2] = new Vector2(2f, 0f);
        uvs[3] = new Vector2(0f, 2f);
        mf.mesh.uv = uvs;
        //================================================
        Debug.Log(">> ===========================================");
        int[] triangles = mf.mesh.triangles;
        for (int i = 0; i < triangles.Length; i++)
        {
            Debug.Log(triangles[i]);
        }
        //================================================
        Debug.Log(">> ===========================================");
        Vector3[] vertices = mf.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Debug.Log(vertices[i]);
        }
        //vertices[2] = new Vector3(0.5f, 0, 0);
        //mf.mesh.vertices = vertices;

        return;

        Debug.Log(">> ===========================================");
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        Debug.Log(mf.mesh.tangents.Length);



        GameObject go = new GameObject("Temp");
        go.transform.position = new Vector3(2, 2, 2);
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = mf.GetComponent<MeshRenderer>().sharedMaterial;



        //Vector2[] uvs = mf.mesh.uv;
        //int[] triangles = mf.mesh.triangles;
        //Vector3[] vertices = mf.mesh.vertices;
        int vIndex = 0;
        for (; vIndex < triangles.Length; vIndex += 3)
        {
            int i0 = triangles[vIndex];
            int i1 = triangles[vIndex + 1];
            int i2 = triangles[vIndex + 2];

            if (IsUVGreater(uvs[i0]) || IsUVGreater(uvs[i1]) || IsUVGreater(uvs[i2]))
            {

            }

            MeshPoint mp0 = new MeshPoint();
            mp0.index = i0;
            mp0.uv = uvs[i0];
            mp0.vertex = vertices[i0];


            MeshPoint mp1 = new MeshPoint();
            mp1.index = i1;
            mp1.uv = uvs[i1];
            mp1.vertex = vertices[i1];


            MeshPoint mp2 = new MeshPoint();
            mp2.index = i2;
            mp2.uv = uvs[i2];
            mp2.vertex = vertices[i2];

            MeshTriangle meshTriangle = new MeshTriangle();
            meshTriangle.p0 = mp0;
            meshTriangle.p1 = mp1;
            meshTriangle.p2 = mp2;
            HandleTriangle(meshTriangle);
            return;
        }

    }

    private bool IsUVGreater(Vector2 v)
    {
        return Mathf.Abs(v.x) > 1 || Mathf.Abs(v.y) > 1;
    }

    /// <summary>
    /// 纠正UV
    /// </summary>
    private void CorrectMeshPoint(MeshPoint meshPoint)
    {
        meshPoint.isCorrectUV = true;
        meshPoint.correctUV = new Vector2(CorrectUV(meshPoint.uv.x), CorrectUV(meshPoint.uv.y));
    }

    /// <summary>
    /// 纠正UV的一个值
    /// </summary>
    private float CorrectUV(float n)
    {
        float result = n;
        if (n < 0)
        {
            result = n + Mathf.FloorToInt(-n) + 1;
        }
        else
        {
            result = n + Mathf.FloorToInt(n);
        }
        Debug.Log("result = " + result);
        return result;
    }


    private void HandleTriangle(MeshTriangle meshTriangle)
    {

        Vector2 side1UV = meshTriangle.p1.uv - meshTriangle.p0.uv;
        Vector2 side2UV = meshTriangle.p2.uv - meshTriangle.p1.uv;
        Vector2 side3UV = meshTriangle.p2.uv - meshTriangle.p0.uv;

        float side1UVX = Mathf.Abs(side1UV.x);
        float side1UVY = Mathf.Abs(side1UV.y);

        float side3UVX = Mathf.Abs(side3UV.x);
        float side3UVY = Mathf.Abs(side3UV.y);

        Vector2 side1UVNormalize = side1UV / side1UV.x;
        Debug.Log(side1UVNormalize);
        //打印边1上的X点

        float length = 0;
        float gap = 0;
        List<MeshPoint> side1Points = new List<MeshPoint>();
        side1Points.Add(meshTriangle.p0);
        while (length >= side1UVX)
        {
            //gap = 1 - 
        }






    }

    private List<MeshPoint> HandlePointByX()
    {
        return null;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
