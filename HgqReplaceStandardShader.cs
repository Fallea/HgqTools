using UnityEngine;
using System.Collections.Generic;

public class HgqReplaceStandardShader : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Replace();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Replace()
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        meshFilters.AddRange(this.gameObject.GetComponentsInChildren<MeshFilter>());

        Shader mobileShader = Shader.Find("Mobile/Diffuse");
        Shader standardShader = Shader.Find("Standard");

        for (int i = 0, length = meshFilters.Count; i < length; i++)
        {
            MeshFilter meshFilter = meshFilters[i];
            Renderer renderer = meshFilter.gameObject.GetComponent<Renderer>();

            Material[] materials = renderer.sharedMaterials;

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];

                Debug.Log(material.shader.name);
                if (material.shader.name.Equals("Mobile/Diffuse"))
                {
                    material.shader = standardShader;
                }
            }
        }
    }
}
