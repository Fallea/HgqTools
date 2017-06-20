using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HgqColliderEditor : MonoBehaviour
{

    [MenuItem("Hgq/Collider/Add Mesh Collider")]
    static void AddMeshCollider()
    {
        Object[] objs = Selection.objects;
        List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
        foreach (Object obj in objs)
        {
            if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                meshRenderers.AddRange(go.GetComponentsInChildren<MeshRenderer>());
            }
        }

        if (meshRenderers.Count < 1)
        {
            return;
        }

        for (int i = 0, length = meshRenderers.Count; i < length; i++)
        {
            MeshRenderer meshRenderer = meshRenderers[i];

            MeshCollider mc = meshRenderer.gameObject.GetComponent<MeshCollider>();
            if (mc == null)
            {
                meshRenderer.gameObject.AddComponent<MeshCollider>();
            }
        }
    }

    [MenuItem("Hgq/Collider/Remove Mesh Collider")]
    static void RemoveMeshCollider()
    {
        Object[] objs = Selection.objects;

        foreach (Object obj in objs)
        {
            if (obj is GameObject)
            {
                GameObject go = obj as GameObject;
                MeshCollider[] meshColliders = go.GetComponentsInChildren<MeshCollider>();
                for (int i = 0; i < meshColliders.Length; i++)
                {
                    Object.DestroyImmediate(meshColliders[i]);
                }
            }
        }
    }

}
