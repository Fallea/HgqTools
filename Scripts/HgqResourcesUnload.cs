using UnityEngine;
using System.Collections;

public class HgqResourcesUnload : MonoBehaviour {

    public bool unload = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (unload)
        {
            unload = false;
            Resources.UnloadUnusedAssets();
        }
	}
}
