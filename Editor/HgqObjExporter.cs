using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class HgqObjExporter
{
    public static string MeshToString(Mesh mesh, Material[] materials)
    {
        Mesh m = mesh;
        Material[] mats = materials;

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append(m.name).Append("\n");
        foreach (Vector3 v in m.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.normals)
        {
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            }
        }
        return sb.ToString();
    }

    public static void MeshToFile(string filePath, Mesh mesh, Material[] materials)
    {
        using (StreamWriter streamWriter = new StreamWriter(filePath))
        {
            streamWriter.Write(MeshToString(mesh, materials));
        }
    }

    public static void MeshToFile(string filename, MeshFilter mf, float scale)
    {
        using (StreamWriter streamWriter = new StreamWriter(filename))
        {
            streamWriter.Write(MeshToString(mf, scale));
        }
    }

    public static string MeshToString(MeshFilter mf, float scale)
    {
        Mesh mesh = null;
        Material[] sharedMaterials = null;
        Vector2 textureOffset = Vector2.one;
        Vector2 textureScale = Vector2.one;
        if (Application.isPlaying)
        {
            mesh = mf.mesh;
            sharedMaterials = mf.GetComponent<Renderer>().materials;
            mf.GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
            mf.GetComponent<Renderer>().sharedMaterial.GetTextureScale("_MainTex");
        }
        else
        {
            mesh = mf.sharedMesh;
            sharedMaterials = mf.GetComponent<Renderer>().sharedMaterials;
            mf.GetComponent<Renderer>().sharedMaterial.GetTextureOffset("_MainTex");
            mf.GetComponent<Renderer>().sharedMaterial.GetTextureScale("_MainTex");
        }

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("mtllib design.mtl").Append("\n");
        stringBuilder.Append("g ").Append(mf.name).Append("\n");
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vector = vertices[i];
            stringBuilder.Append(string.Format("v {0} {1} {2}\n", vector.x * scale, vector.z * scale, vector.y * scale));
        }
        stringBuilder.Append("\n");
        Dictionary<int, int> dictionary = new Dictionary<int, int>();
        if (mesh.subMeshCount > 1)
        {
            int[] triangles = mesh.GetTriangles(1);
            for (int j = 0; j < triangles.Length; j += 3)
            {
                if (!dictionary.ContainsKey(triangles[j]))
                {
                    dictionary.Add(triangles[j], 1);
                }
                if (!dictionary.ContainsKey(triangles[j + 1]))
                {
                    dictionary.Add(triangles[j + 1], 1);
                }
                if (!dictionary.ContainsKey(triangles[j + 2]))
                {
                    dictionary.Add(triangles[j + 2], 1);
                }
            }
        }
        for (int num = 0; num != mesh.uv.Length; num++)
        {
            Vector2 vector2 = Vector2.Scale(mesh.uv[num], textureScale) + textureOffset;
            if (dictionary.ContainsKey(num))
            {
                stringBuilder.Append(string.Format("vt {0} {1}\n", mesh.uv[num].x, mesh.uv[num].y));
            }
            else
            {
                stringBuilder.Append(string.Format("vt {0} {1}\n", vector2.x, vector2.y));
            }
        }
        for (int k = 0; k < mesh.subMeshCount; k++)
        {
            stringBuilder.Append("\n");
            if (k == 0)
            {
                stringBuilder.Append("usemtl ").Append("Material_design").Append("\n");
            }
            if (k == 1)
            {
                stringBuilder.Append("usemtl ").Append("Material_logo").Append("\n");
            }
            int[] triangles2 = mesh.GetTriangles(k);
            for (int l = 0; l < triangles2.Length; l += 3)
            {
                stringBuilder.Append(string.Format("f {0}/{0} {1}/{1} {2}/{2}\n", triangles2[l] + 1, triangles2[l + 1] + 1, triangles2[l + 2] + 1));
            }
        }
        return stringBuilder.ToString();
    }
}