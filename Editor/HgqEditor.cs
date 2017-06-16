using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;

public class HgqEditor : MonoBehaviour
{

    [MenuItem("Hgq/Create Temp List File")]
    static void Create()
    {
        string streamingAssetsPath = Application.streamingAssetsPath + "/";
        List<string> list1 = GetAllFiles(streamingAssetsPath + "filecrc/");
        List<string> list2 = GetAllFiles(streamingAssetsPath + "materials/");

        List<string> list = new List<string>();
        list.AddRange(list1);
        list.AddRange(list2);

        StringBuilder sb = new StringBuilder();

        bool isFrist = true;

        for (int i = 0; i < list.Count; i++)
        {
            string str = list[i].Replace("\\", "/");
            if (!str.EndsWith(".meta"))
            {
                if (!isFrist)
                {
                    sb.Append("\n");
                }
                sb.Append(str.Replace(streamingAssetsPath, ""));
                isFrist = false;
            }
        }

        File.WriteAllText(Application.dataPath + "/Resources/LocalResourcesList.txt", sb.ToString());





    }

    private static List<string> GetAllFiles(string dirPth)
    {
        List<string> result = new List<string>();
        result.AddRange(Directory.GetFiles(dirPth));

        string[] dirs = Directory.GetDirectories(dirPth);
        foreach (string dp in dirs)
        {
            result.AddRange(GetAllFiles(dp));
        }
        return result;
    }
}
