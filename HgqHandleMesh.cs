using UnityEngine;
using System.Collections.Generic;

public class HgqHandleMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        meshFilters.AddRange(this.gameObject.GetComponentsInChildren<MeshFilter>());

        for (int i = 0, length = meshFilters.Count; i < length; i++)
        {
            MeshFilter meshFilter = meshFilters[i];
            if (meshFilter.mesh != null)
            {
                Renderer renderer = meshFilter.gameObject.GetComponent<Renderer>();
                if (renderer.sharedMaterials.Length > 1)
                {
                    meshFilter.mesh.subMeshCount = renderer.sharedMaterials.Length;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
