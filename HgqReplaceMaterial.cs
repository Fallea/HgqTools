using UnityEngine;
using System.Collections;

public class HgqReplaceMaterial : MonoBehaviour {

    private Material mMaterial = null; 

	// Use this for initialization
	void Start () {
        MeshRenderer[] mrs = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        mMaterial = mrs[0].sharedMaterials[0];

        for (int i = 0; i < mrs.Length; i++)
        {
            Material[] tmes = new Material[mrs[i].sharedMaterials.Length];
            for (int j = 0; j < tmes.Length; j++)
            {
                tmes[j] = mMaterial;
            }
            mrs[i].sharedMaterials = tmes;
        }

    }
	
}
