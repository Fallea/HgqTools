using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Text;

public class HgqHandleFbxPosition : MonoBehaviour {

    [MenuItem("Hgq/Handle Fbx Position")]
    private static void HandleFbxPosition()
    {
        Object[] objs = Selection.objects;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < objs.Length; i++)
        {
            Object obj = objs[i];
            if (obj is GameObject)
            {
                sb.Append((obj as GameObject).transform.position.ToString());
            }
        }



    }
}
