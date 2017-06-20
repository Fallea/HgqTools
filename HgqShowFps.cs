using UnityEngine;
using UnityEngine.UI;

public class HgqShowFps : MonoBehaviour
{

    public Text fpsTxt;

    private float mLastUpdateShowTime = 0f;//上一次更新帧率的时间;  

    private float mUpdateShowDeltaTime = 1f;//更新帧率的时间间隔;  

    private int mFrameUpdate = 0;//帧数;  

    private float mFPS = 0;

    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        //Application.targetFrameRate = 50;
    }

    void Update()
    {
        mFrameUpdate++;
        if (Time.realtimeSinceStartup - mLastUpdateShowTime >= mUpdateShowDeltaTime)
        {
            mFPS = mFrameUpdate / (Time.realtimeSinceStartup - mLastUpdateShowTime);
            mFrameUpdate = 0;
            mLastUpdateShowTime = Time.realtimeSinceStartup;
            fpsTxt.text = "fps:" + string.Format("{0:N2} ", mFPS);
        }
    }


}
