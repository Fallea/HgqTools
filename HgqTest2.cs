using UnityEngine;
using System.Collections;

public class HgqTest2 : MonoBehaviour {

    public static bool isDebug = false;

    public bool _isDebug = false;
    private bool mIsDebug = false;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (_isDebug != mIsDebug)
        {
            mIsDebug = _isDebug;
            isDebug = _isDebug;
        }
	}
}
