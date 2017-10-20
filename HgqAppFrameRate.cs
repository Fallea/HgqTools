using UnityEngine;

public class HgqAppFrameRate : MonoBehaviour
{
    public int targetFrameRate = 30;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
