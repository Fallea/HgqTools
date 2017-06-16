using UnityEngine;
using System.Collections.Generic;

public class HgqCombineSubMeshes : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CombineSubMeshes(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CombineSubMeshes(GameObject go)
    {
        MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            CombineSubMeshes(meshFilters[i]);
        }
    }

    private void CombineSubMeshes(MeshFilter meshFilter)
    {
        Mesh mesh = meshFilter.sharedMesh;
        if (mesh == null) { return; }
        if (mesh.subMeshCount < 2) { return; }
        List<CombineInstance> list = new List<CombineInstance>();
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            CombineInstance ci = new CombineInstance();
            ci.mesh = mesh;
            ci.subMeshIndex = i;
            list.Add(ci);
        }

        Mesh newMesh = new Mesh();
        newMesh.name = "Combined";
        meshFilter.sharedMesh = newMesh;
        newMesh.CombineMeshes(list.ToArray(), false, false);
        newMesh.subMeshCount = mesh.subMeshCount;
    }
}
