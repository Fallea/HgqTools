using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

public class HgqAssetBundleEditor : MonoBehaviour
{

    [MenuItem("Hgq/AssetBundle/Bulid active scene bundle")]
    static void BulidActiveSceneBundle()
    {
        string path = SceneManager.GetActiveScene().path;

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("请选择保存后的场景！");
            return;
        }

        string bundleDirPath = Application.streamingAssetsPath + "/";

        if (!Directory.Exists(bundleDirPath))
        {
            Directory.CreateDirectory(bundleDirPath);
        }

        Debug.Log(path);

        string abbPath = path.Replace("Assets/", "");


        List<AssetBundleBuild> list = new List<AssetBundleBuild>();
        AssetBundleBuild abb;

        abb = new AssetBundleBuild();
        abb.assetBundleName = abbPath.Replace("unity", "assetbundle");
        abb.assetNames = new string[] { path };
        abb.assetBundleVariant = "";
        list.Add(abb);
    }

    [MenuItem("Hgq/AssetBundle/Bulid one bundle by all selected")]
    static void BulidOneBundleByAllSelected()
    {
        List<AssetBundleBuild> list = new List<AssetBundleBuild>();
        AssetBundleBuild abb;

        string bundleDirPath = Application.streamingAssetsPath + "/";

        if (!Directory.Exists(bundleDirPath))
        {
            Directory.CreateDirectory(bundleDirPath);
        }

        for (int i = 0, length = Selection.objects.Length; i < length; i++)
        {
            string path = AssetDatabase.GetAssetPath(Selection.objects[i]);

            if (File.Exists(path))
            {
                string abbPath = path.Replace("Assets/", "");
                string end = abbPath.Substring(0, abbPath.LastIndexOf("."));

                abb = new AssetBundleBuild();
                abb.assetBundleName = end + ".assetbundle";
                abb.assetNames = new string[] { path };
                abb.assetBundleVariant = "";
                list.Add(abb);
            }
        }

        if (list.Count > 0)
        {
            BuildPipeline.BuildAssetBundles(bundleDirPath, list.ToArray(), BuildAssetBundleOptions.None | BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Hgq/AssetBundle/Bulid one bundle by one file")]
    static void BulidOneBundleByOneFile()
    {
        string bundleDirPath = Application.streamingAssetsPath + "/";

        if (!Directory.Exists(bundleDirPath))
        {
            Directory.CreateDirectory(bundleDirPath);
        }

        for (int i = 0, length = Selection.objects.Length; i < length; i++)
        {
            List<AssetBundleBuild> list = new List<AssetBundleBuild>();
            AssetBundleBuild abb;
            string path = AssetDatabase.GetAssetPath(Selection.objects[i]);

            if (File.Exists(path))
            {
                string abbPath = path.Replace("Assets/", "");
                string end = abbPath.Substring(0, abbPath.LastIndexOf("."));

                abb = new AssetBundleBuild();
                abb.assetBundleName = end + ".assetbundle";
                abb.assetNames = new string[] { path };
                abb.assetBundleVariant = "";
                list.Add(abb);
            }
            if (list.Count > 0)
            {
                BuildPipeline.BuildAssetBundles(bundleDirPath, list.ToArray(), BuildAssetBundleOptions.None | BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);
            }
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Hgq/AssetBundle/Delete manifest files by select directory")]
    static void DeleteManifestFilesBySelectDirectory()
    {
        for (int i = 0, length = Selection.objects.Length; i < length; i++)
        {
            string path = AssetDatabase.GetAssetPath(Selection.objects[i]);
            if (Directory.Exists(path))
            {
                Debug.Log(path);
                string[] files = Directory.GetFiles(path, "*.manifest", SearchOption.AllDirectories);
                foreach (string filePath in files)
                {
                    File.Delete(filePath);
                }
            }
        }
    }

}
