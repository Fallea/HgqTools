using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HgqShaderEditor : MonoBehaviour
{
    [MenuItem("Hgq/Shader/Check Cutoff")]
    static void CheckShaderByName()
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

        int count = 0;

        for (int i = 0, length = meshRenderers.Count; i < length; i++)
        {
            MeshRenderer meshRenderer = meshRenderers[i];
            Material[] materials = meshRenderer.sharedMaterials;

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];
                if (material.shader.name.Equals("Unlit/ShaderTileCutoff"))
                {
                    meshRenderer.gameObject.SetActive(false);
                    break;
                    count++;
                }
            }
        }
        Debug.Log(count);
    }

    [MenuItem("Hgq/Shader/Standard to Tile")]
    static void StandardToTile()
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


        Shader tileShader = Shader.Find("Unlit/ShaderTile");
        Shader tileCutoffShader = Shader.Find("Unlit/ShaderTileCutoff");


        Material tileMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/DefaultMat_Tile.mat");
        Material tileCutoffMaterial = AssetDatabase.LoadAssetAtPath<Material>("Assets/Resources/DefaultMat_TileCutoff.mat");


        for (int i = 0, length = meshRenderers.Count; i < length; i++)
        {
            MeshRenderer meshRenderer = meshRenderers[i];

            Material[] materials = meshRenderer.sharedMaterials;
            Material[] newMaterials = new Material[materials.Length];

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];
                string filePath = AssetDatabase.GetAssetPath(material);

                if (filePath.EndsWith("_new.mat"))
                {
                    newMaterials[j] = material;
                    if (material.shader.name.Equals("Standard"))
                    {
                        float mode = material.GetFloat("_Mode");
                        if (mode == 3)
                        {
                            material.shader = tileCutoffShader;
                        }
                        else
                        {
                            material.shader = tileShader;
                        }
                    }

                    continue;
                }

                filePath = filePath.Replace(material.name + ".mat", material.name + "_new.mat");

                Material newMaterial = null;
                if (material.shader.name.Equals("Standard"))
                {
                    newMaterial = AssetDatabase.LoadAssetAtPath<Material>(filePath);

                    if (newMaterial == null || newMaterial.shader == null || !newMaterial.shader.isSupported)
                    {
                        float mode = material.GetFloat("_Mode");

                        if (mode == 3)
                        {
                            newMaterial = Object.Instantiate(tileCutoffMaterial);
                        }
                        else
                        {
                            newMaterial = Object.Instantiate(tileMaterial);
                        }
                        newMaterial.mainTexture = material.mainTexture;

                        AssetDatabase.CreateAsset(newMaterial, filePath);
                        AssetDatabase.ImportAsset(filePath);
                        newMaterial = AssetDatabase.LoadAssetAtPath<Material>(filePath);
                    }

                    newMaterials[j] = newMaterial;
                }
                else
                {
                    newMaterials[j] = material;
                }
            }

            meshRenderer.sharedMaterials = newMaterials;
        }
        Debug.Log("Standard to Tile Completed.");
    }

    [MenuItem("Hgq/Shader/Replace with Standard")]
    static void ReplaceWithStandard()
    {
        ReplaceByName("Standard");
    }


    [MenuItem("Hgq/Shader/Replace with MobileDiffuse")]
    static void ReplaceWithMobileDiffuse()
    {
        ReplaceByName("Mobile/Diffuse");
    }


    static void ReplaceByName(string shaderName)
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

        Shader shader = Shader.Find(shaderName);


        for (int i = 0, length = meshRenderers.Count; i < length; i++)
        {
            MeshRenderer meshRenderer = meshRenderers[i];

            Material[] materials = meshRenderer.sharedMaterials;

            for (int j = 0; j < materials.Length; j++)
            {
                Material material = materials[j];
                string filePath = AssetDatabase.GetAssetPath(material);

                if (filePath.EndsWith("_new.mat"))
                {
                    if (!material.shader.name.Equals(shaderName))
                    {
                        material.shader = shader;
                    }
                }
            }
        }
        Debug.Log("Replace Completed.");
    }
}
