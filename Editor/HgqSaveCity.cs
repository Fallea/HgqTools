using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class HgqPathUtil
{
    public static string appRootPath = Application.dataPath;

    public static string cityRootPath = Application.dataPath + "/CityTest/";
    public static string prefabsPath = cityRootPath + "Prefabs/";
    public static string materialsPath = cityRootPath + "Materials/";
    public static string texturesPath = cityRootPath + "Textures/";
    public static string meshsPath = cityRootPath + "Meshs/";

    public static Texture2D RGBATexture;
    public static Texture2D RGBTexture;

    static HgqPathUtil()
    {
        appRootPath = appRootPath.Replace("Assets", "");

        if (!Directory.Exists(cityRootPath))
        {
            Directory.CreateDirectory(cityRootPath);
        }
        if (!Directory.Exists(prefabsPath))
        {
            Directory.CreateDirectory(prefabsPath);
        }
        if (!Directory.Exists(materialsPath))
        {
            Directory.CreateDirectory(materialsPath);
        }
        if (!Directory.Exists(texturesPath))
        {
            Directory.CreateDirectory(texturesPath);
        }
        if (!Directory.Exists(meshsPath))
        {
            Directory.CreateDirectory(meshsPath);
        }

        cityRootPath = "Assets/CityTest/";
        prefabsPath = cityRootPath + "Prefabs/";
        materialsPath = cityRootPath + "Materials/";
        texturesPath = cityRootPath + "Textures/";
        meshsPath = cityRootPath + "Meshs/";

        RGBATexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HgqTest/rgba.png");
        RGBTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HgqTest/rgb.png");
    }
}


public class TempMapped
{
    public Transform root;
    public Transform newRoot;

    public TempMapped(Transform root, Transform newRoot)
    {
        this.root = root;
        this.newRoot = newRoot;
    }
}


public class HgqSaveCity : EditorWindow
{

    static float WIDTH = 500;
    static float HEIGHT = 300;
    static float BUTTON_WIDTH = 260;

    private static List<TempMapped> mappeds = new List<TempMapped>();


    private static Transform FindAndCreate(string name, Transform parent)
    {
        GameObject go = GameObject.Find(name);
        if (go == null)
        {
            go = new GameObject(name);
            go.transform.parent = parent;
            go.transform.position = Vector3.zero;
        }
        return go.transform;
    }

    [MenuItem("Hgq/Tool Window")]
    static void Init()
    {
        Rect wr = new Rect(0, 0, WIDTH, HEIGHT);

        HgqSaveCity window = GetWindowWithRect<HgqSaveCity>(wr, true, "生成当前城市Prefab");

        //SceneToolEditorWindow window = GetWindow<SceneToolEditorWindow>("处理LOD资源");
        window.position = new Rect((Screen.currentResolution.width - WIDTH) / 2, (Screen.currentResolution.height - HEIGHT) / 2, WIDTH, HEIGHT);

        window.Show();
    }

    [MenuItem("Hgq/Test")]
    private static void Test()
    {
        GameObject[] gos = Selection.gameObjects;
        if (gos.Length > 0)
        {
            CopyGameObject(gos[0].transform, null);
            Debug.Log(gos[0].name);
        }
    }

    [MenuItem("Hgq/See Mesh")]
    private static void SeeMesh()
    {
        GameObject[] gos = Selection.gameObjects;
        if (gos.Length > 0)
        {
            List<MeshFilter> meshFilters = new List<MeshFilter>();
            meshFilters.AddRange(gos[0].GetComponentsInChildren<MeshFilter>());
            if (meshFilters.Count > 0)
            {
                Mesh mesh = meshFilters[0].mesh;
                Debug.Log(mesh.subMeshCount);
            }
        }
    }

    static void MappedRoot()
    {
        mappeds.Clear();

        Transform cityRoot = FindAndCreate("CityRoot", null);

        Transform cityBlockRoot = FindAndCreate("CityBlockRoot", cityRoot);
        Transform cityGreenMeshRoot = FindAndCreate("CityGreenMeshRoot", cityRoot);
        Transform cityComponentRoot = FindAndCreate("CityComponentRoot", cityRoot);
        Transform cityWallRoot = FindAndCreate("CityWallRoot", cityRoot);
        Transform cityBuildingRoot = FindAndCreate("CityBuildingRoot", cityRoot);
        Transform cityShopRoot = FindAndCreate("CityShopRoot", cityRoot);


        GameObject blockRoot = GameObject.Find("BlockRoot");
        GameObject greenMeshRoot = GameObject.Find("GreenMeshRoot");
        GameObject componentRoot = GameObject.Find("ComponentRoot");
        GameObject wallRoot = GameObject.Find("WallRoot");
        GameObject buildingRoot = GameObject.Find("BuildingRoot");
        GameObject shopRoot = GameObject.Find("ShopRoot");


        if (greenMeshRoot != null)
        {
            mappeds.Add(new TempMapped(greenMeshRoot.transform, cityGreenMeshRoot));
        }
        if (componentRoot != null)
        {
            mappeds.Add(new TempMapped(componentRoot.transform, cityComponentRoot));
        }
        if (wallRoot != null)
        {
            mappeds.Add(new TempMapped(wallRoot.transform, cityWallRoot));
        }
        if (buildingRoot != null)
        {
            mappeds.Add(new TempMapped(buildingRoot.transform, cityBuildingRoot));
        }
        if (shopRoot != null)
        {
            mappeds.Add(new TempMapped(shopRoot.transform, cityShopRoot));
        }
        if (blockRoot != null)
        {
            mappeds.Add(new TempMapped(blockRoot.transform, cityBlockRoot));
        }
    }

    void OnGUI()
    {
        //GUILayout.Space(50);
        float y = 40;
        float gapY = 45;

        if (GUI.Button(new Rect((WIDTH - BUTTON_WIDTH) / 2, y, BUTTON_WIDTH, 30), "生成Prefab"))
        {
            MappedRoot();
            isStart = true;
        }
        y += gapY;
    }

    int total = 0;
    int count = 0;
    bool isStart = false;
    Transform root;
    Transform newRoot;
    Queue<Transform> node = new Queue<Transform>();
    int frameCount = 0;

    private void Update()
    {
        if (!isStart) { return; }
        if (node.Count < 1)
        {
            if (mappeds.Count > 0)
            {
                TempMapped tempMapped = mappeds[mappeds.Count - 1];
                mappeds.RemoveAt(mappeds.Count - 1);

                root = tempMapped.root;
                newRoot = tempMapped.newRoot;
                Debug.Log("=========================================================");
                Debug.Log(root.name);
                Debug.Log("=========================================================");

                count = 0;
                total = root.childCount;
                for (int i = 0; i < root.childCount; i++)
                {
                    node.Enqueue(root.GetChild(i));
                }
            }
            else
            {
                Debug.Log("完成");
                isStart = false;
            }
        }
        else
        {
            if (frameCount > 2)
            {
                frameCount = 0;
                CopyGameObject(node.Dequeue(), newRoot);
                count++;
                Debug.Log(count + "/" + total);
            }
            else
            {
                frameCount++;
            }
        }
    }

    //==============================================================================
    //==============================================================================

    private static void CopyGameObject(Transform child, Transform parent)
    {
        GameObject newGo = GameObject.Instantiate(child.gameObject);
        newGo.name = child.name;
        newGo.transform.parent = parent;

        List<MeshFilter> meshFilters = new List<MeshFilter>();
        meshFilters.AddRange(child.GetComponentsInChildren<MeshFilter>());

        List<MeshFilter> newMeshFilters = new List<MeshFilter>();
        newMeshFilters.AddRange(newGo.GetComponentsInChildren<MeshFilter>());

        for (int i = 0; i < newMeshFilters.Count; i++)
        {
            MeshFilter meshFilter = meshFilters[i];
            Renderer renderer = meshFilter.gameObject.GetComponent<Renderer>();

            MeshFilter newMeshFilter = newMeshFilters[i];
            Renderer newRenderer = newMeshFilter.gameObject.GetComponent<Renderer>();

            Mesh mesh = null;
            Material[] materials = null;

            if (Application.isPlaying)
            {
                mesh = meshFilter.mesh;
                materials = renderer.materials;
            }
            else
            {
                mesh = meshFilter.sharedMesh;
                materials = renderer.sharedMaterials;
            }


            Material[] newMaterials = CopyAndSaveMatrtials(materials);

            string filePath = HgqPathUtil.meshsPath + mesh.name + ".obj";

            HgqObjExporter.MeshToFile(HgqPathUtil.appRootPath + filePath, mesh, materials);

            mesh = AssetDatabase.LoadAssetAtPath<Mesh>(filePath);
            if (mesh != null)
            {
                mesh.subMeshCount = newMaterials.Length;
            }
            else
            {
                Debug.LogWarning(child.name);
                Debug.LogWarning(filePath);
            }

            newMeshFilter.sharedMesh = mesh;
            newRenderer.sharedMaterials = newMaterials;
        }

    }

    private static Material[] CopyAndSaveMatrtials(Material[] sources)
    {
        Material[] result = new Material[sources.Length];

        for (int i = 0; i < sources.Length; i++)
        {
            result[i] = CopyAndSaveMatrtial(sources[i]);
        }
        return result;
    }

    private static Material CopyAndSaveMatrtial(Material source)
    {
        string filePath = HgqPathUtil.materialsPath + source.name + ".mat";
        Material result = AssetDatabase.LoadAssetAtPath<Material>(filePath);
        if (result != null && result.GetTexture("_MainTex") != null)
        {
            return result;
        }

        //Object[] textures = EditorUtility.CollectDependencies(new UnityEngine.Object[] { source });
        Texture texture = source.GetTexture("_MainTex");
        Texture newTexture = null;
        if (texture != null)
        {
            newTexture = CopyAndSaveTexture(texture as Texture2D);
        }
        else
        {
            Debug.LogWarning("Material > " + source.name);
        }

        if (newTexture == null)
        {
            Debug.LogWarning("texture.name = " + texture.name);
        }

        result = new Material(source);
        result.SetTexture("_MainTex", newTexture);
        AssetDatabase.CreateAsset(result, filePath);
        //if (Application.isPlaying)
        //{
        //    GameObject.Destroy(result);
        //}
        //else
        //{
        //    GameObject.DestroyImmediate(result);
        //}


        result = AssetDatabase.LoadAssetAtPath<Material>(filePath);

        return result;
    }


    private static Texture2D CopyAndSaveTexture(Texture2D source)
    {
        string filePath = HgqPathUtil.texturesPath + source.name + ".png";

        Texture2D result = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
        if (result != null)
        {
            return result;
        }

        try
        {
            result = new Texture2D(source.width, source.height, HgqPathUtil.RGBATexture.format, source.mipmapCount > 0);

            Color[] colors = HgqPathUtil.RGBATexture.GetPixels(0, 0, source.width, source.height);
            result.SetPixels(colors);

            byte[] bytes = result.EncodeToPNG();

            File.WriteAllBytes(filePath, bytes);

            if (Application.isPlaying)
            {
                GameObject.Destroy(result);
            }
            else
            {
                GameObject.DestroyImmediate(result);
            }

            //TextureImporter ti = TextureImporter.GetAtPath(filePath) as TextureImporter;
            //if (source.format == TextureFormat.ETC_RGB4)
            //{
            //    ti.textureFormat = TextureImporterFormat.ETC_RGB4;
            //}
            //else
            //{
            //    Debug.Log(source.format);
            //}
            //ti.textureType = TextureImporterType.Image;

            //AssetDatabase.ImportAsset(filePath, ImportAssetOptions.ForceUpdate);

            result = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
            return result;
        }
        catch (System.Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }

    }
}
