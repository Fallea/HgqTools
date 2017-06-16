using UnityEngine;
using System.Collections.Generic;
using System.Runtime;

public class HgqCheckMaterial : MonoBehaviour
{

    public bool checkMaterial = false;
    public bool checkStandardShder = false;
    public bool setNullTexture = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (checkMaterial)
        {
            checkMaterial = false;
            CheckMaterialInstance();
        }

        if (checkStandardShder)
        {
            checkStandardShder = false;
            CheckStandardShader();
        }

        if (setNullTexture)
        {
            setNullTexture = false;
            SetMatrialNullTexture();
        }
    }


    private void CheckMaterialInstance()
    {
        Dictionary<Material, int> dic = new Dictionary<Material, int>();
        Dictionary<string, int> strIntDic = new Dictionary<string, int>();

        List<MeshFilter> meshFilters = new List<MeshFilter>();
        meshFilters.AddRange(this.gameObject.GetComponentsInChildren<MeshFilter>());

        for (int i = 0, length = meshFilters.Count; i < length; i++)
        {
            MeshFilter meshFilter = meshFilters[i];
            Renderer renderer = meshFilter.gameObject.GetComponent<Renderer>();

            Material[] materials = renderer.sharedMaterials;

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];
                if (dic.ContainsKey(material))
                {
                    dic[material] = dic[material] + 1;
                }
                else
                {
                    dic.Add(material, 1);
                }

                string materialName = material.name;
                if (strIntDic.ContainsKey(materialName))
                {
                    strIntDic[materialName] = strIntDic[materialName] + 1;
                }
                else
                {
                    strIntDic.Add(materialName, 1);
                }
            }
        }

        foreach (KeyValuePair<Material, int> kv in dic)
        {
            Debug.Log(kv.Key.name + " == " + kv.Key.GetInstanceID() + " == " + kv.Value);
        }
        Debug.Log("=====================================================================");
        foreach (KeyValuePair<string, int> kv in strIntDic)
        {
            Debug.Log(kv.Key + " == " + kv.Value);
        }
    }

    private void CheckStandardShader()
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        meshFilters.AddRange(this.gameObject.GetComponentsInChildren<MeshFilter>());

        for (int i = 0, length = meshFilters.Count; i < length; i++)
        {
            MeshFilter meshFilter = meshFilters[i];
            Renderer renderer = meshFilter.gameObject.GetComponent<Renderer>();

            Material[] materials = renderer.sharedMaterials;

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];
                if (material.shader.name.Equals("Standard"))
                {
                    Debug.Log(GetPath(meshFilter.transform));
                }
            }
        }
    }

    private string GetPath(Transform transform)
    {
        string result = transform.name;
        if (transform.parent != this.transform)
        {
            result = GetPath(transform.parent) + "/" + result;
        }
        return result;
    }

    private void SetMatrialNullTexture()
    {
        List<MeshFilter> meshFilters = new List<MeshFilter>();
        meshFilters.AddRange(this.gameObject.GetComponentsInChildren<MeshFilter>());

        for (int i = 0, length = meshFilters.Count; i < length; i++)
        {
            MeshFilter meshFilter = meshFilters[i];
            Renderer renderer = meshFilter.gameObject.GetComponent<Renderer>();

            Material[] materials = renderer.materials;

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];
                material.mainTexture = null;
            }
        }
    }
}
