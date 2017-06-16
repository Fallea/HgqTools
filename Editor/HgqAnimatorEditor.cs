using UnityEngine;
using UnityEditor;

public class HgqAnimatorEditor
{

    [MenuItem("Hgq/Animator/Remove")]
    static void Remove()
    {
        if (Selection.activeObject != null && Selection.activeObject is GameObject)
        {
            RemoveAnimator(Selection.activeObject as GameObject);
        }
    }

    private static void RemoveAnimator(GameObject root)
    {
        Animator[] animators = root.GetComponentsInChildren<Animator>();
        for (int i = 0; i < animators.Length; i++)
        {
            GameObject.DestroyImmediate(animators[i]);
        }
    }
}