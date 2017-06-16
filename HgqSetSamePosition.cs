using UnityEngine;
using System.Collections;

public class HgqSetSamePosition : MonoBehaviour {

    public Vector3 position;
    public bool seted = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (seted)
        {
            seted = false;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).position = position;
            }
        }
	}
}
