using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HgqTestCreateInfo
{
    public string name = null;
    public float startTime = 0;
    public float endTime = 0;
    public int repeatCount = 0;
    public List<string> urls = new List<string>();

    public void DebugUrls()
    {
        foreach (string url in urls)
        {
            Debug.LogWarning(url);
        }
    }
}

public class HgqTestUrlDownInfo
{
    public string url = null;
    public float startTime = 0;
    public float endTime = 0;
    public int repeatCount = 0;
    public List<float> repeatInterval = new List<float>();
}

public class HgqTest : MonoBehaviour
{
    private static Dictionary<string, HgqTestCreateInfo> createInfos = new Dictionary<string, HgqTestCreateInfo>();
    private static Dictionary<string, HgqTestUrlDownInfo> downInfos = new Dictionary<string, HgqTestUrlDownInfo>();

    private static HgqTestCreateInfo tempCreateInfo = null;
    private static HgqTestUrlDownInfo tempUrlDownInfo = null;

    public static void AddCreate(string name)
    {
        if (createInfos.TryGetValue(name, out tempCreateInfo))
        {
            tempCreateInfo.repeatCount++;
        }
        else
        {
            tempCreateInfo = new HgqTestCreateInfo();
            tempCreateInfo.name = name;
            tempCreateInfo.startTime = Time.realtimeSinceStartup;
            createInfos.Add(tempCreateInfo.name, tempCreateInfo);
            Debug.LogWarning(">> Create > " + tempCreateInfo.name);
        }
    }

    public static void EndCreate(string name)
    {
        if (createInfos.TryGetValue(name, out tempCreateInfo))
        {

            tempCreateInfo.endTime = Time.realtimeSinceStartup;
            Debug.LogWarning(">> Create > " + tempCreateInfo.name + " > " + (tempCreateInfo.endTime - tempCreateInfo.startTime) + " > urls = " + tempCreateInfo.urls.Count);
            //Debug.LogWarning(">> Create > " + tempCreateInfo.name + " > urls = " + tempCreateInfo.urls.Count);
            //tempCreateInfo.DebugUrls();


        }
    }

    public static void AddCreateUrl(string name, string url)
    {
        if (createInfos.TryGetValue(name, out tempCreateInfo))
        {
            if (!tempCreateInfo.urls.Contains(url))
            {
                tempCreateInfo.urls.Add(url);
            }
        }
    }


    public static void AddUrlDown(string url)
    {
        if (downInfos.TryGetValue(url, out tempUrlDownInfo))
        {
            tempUrlDownInfo.repeatCount++;
            tempUrlDownInfo.startTime = Time.realtimeSinceStartup;
            //tempUrlDownInfo.repeatInterval.Add(Time.realtimeSinceStartup);
            //Debug.LogWarning(">> AddUrlDown > " + tempUrlDownInfo.url + " > " + tempUrlDownInfo.startTime);
        }
        else
        {
            tempUrlDownInfo = new HgqTestUrlDownInfo();
            tempUrlDownInfo.url = url;
            tempUrlDownInfo.startTime = Time.realtimeSinceStartup;
            downInfos.Add(tempUrlDownInfo.url, tempUrlDownInfo);
            //Debug.LogWarning(">> AddUrlDown > " + tempUrlDownInfo.url + " > " + tempUrlDownInfo.startTime);
        }
    }

    public static void EndUrlDown(string url)
    {
        if (downInfos.TryGetValue(url, out tempUrlDownInfo))
        {
            tempUrlDownInfo.endTime = Time.realtimeSinceStartup;
            Debug.LogWarning(">> EndUrlDown > " + tempUrlDownInfo.url + " > " + (tempUrlDownInfo.endTime - tempUrlDownInfo.startTime));

            //if (tempUrlDownInfo.repeatCount < 1)
            //{
            //    tempUrlDownInfo.endTime = Time.realtimeSinceStartup;
            //    Debug.LogWarning(">> EndUrlDown > " + tempUrlDownInfo.url + " > " + (tempUrlDownInfo.endTime - tempUrlDownInfo.startTime));
            //    //Debug.LogWarning(">> " + tempUrlDownInfo.startTime + " - " +  tempUrlDownInfo.endTime);
            //}
            //else
            //{
            //    Debug.LogWarning(">> EndUrlDown > " + tempUrlDownInfo.url + " > " + tempUrlDownInfo.repeatCount);
            //}
        }
    }


    //================================================================
    public Text fpsTxt;
    public Text urlCountTxt;

    private float m_LastUpdateShowTime = 0f;    //上一次更新帧率的时间;  

    private float m_UpdateShowDeltaTime = 1f;//更新帧率的时间间隔;  

    private int m_FrameUpdate = 0;//帧数;  

    private float m_FPS = 0;

    private static HgqTest mInstance;
    private static Dictionary<string, string> urls = new Dictionary<string, string>();

    public static void AddHttpUrl(string url)
    {
        if (!urls.ContainsKey(url))
        {
            urls.Add(url, url);
            //mInstance.urlCountTxt.text = "urls:" + urls.Count;
        }
    }

    public static void ShowTxt(string txt)
    {
        mInstance.urlCountTxt.text = txt;
    }

    void Awake()
    {
        mInstance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        Application.targetFrameRate = 50;
    }

    void Start()
    {

    }



    void Update()
    {

        m_FrameUpdate++;
        if (Time.realtimeSinceStartup - m_LastUpdateShowTime >= m_UpdateShowDeltaTime)
        {
            m_FPS = m_FrameUpdate / (Time.realtimeSinceStartup - m_LastUpdateShowTime);
            m_FrameUpdate = 0;
            m_LastUpdateShowTime = Time.realtimeSinceStartup;
            fpsTxt.text = "fps:" + string.Format("{0:N2} ", m_FPS);

            //urlCountTxt.text = "loading:" + SceneStrategyManager.Instance.loadingTotalTime.ToString();
        }
    }






}
