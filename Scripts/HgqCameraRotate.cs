using UnityEngine;
using System.Collections;

public class HgqCameraRotate : MonoBehaviour
{
    private Vector3 mPointY;
    private Vector3 mPointX;
    private int mDefaultDpi = 96;
    private float mFirstRotateSpeed = 2.25f;
    private int _minX = -85;
    private int _maxX = 45;

    void LateUpdate()
    {
        UpdateRotate();
    }

    private void UpdateRotate()
    {
#if UNITY_EDITOR || UNITY_WIN
        if (Input.GetMouseButton(0))
        {

#else
        if(Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Moved))
        { 
#endif
            float mouseX = Input.GetAxis("Mouse X") * (mDefaultDpi / (float)Screen.dpi);
            float mouseY = Input.GetAxis("Mouse Y") * (mDefaultDpi / (float)Screen.dpi);

            float deltay = mouseX * mFirstRotateSpeed;
            float deltax = (-1) * mouseY * mFirstRotateSpeed;
            if (Mathf.Abs(deltax) > 0.2f || Mathf.Abs(deltay) > 0.2f)
            {
                float currentx = transform.eulerAngles.x;
                if (90f < Mathf.Abs(currentx))
                    currentx = currentx - 360f;

                if (_minX > (currentx + deltax))
                    deltax = _minX - currentx;

                if (_maxX < (currentx + deltax))
                    deltax = _maxX - currentx;

                if (false == Mathf.Approximately(0f, deltax))
                    transform.Rotate(deltax, 0f, 0f, Space.Self);

                if (false == Mathf.Approximately(0f, deltay))
                    transform.Rotate(0f, deltay, 0f, Space.World);
            }
            Debug.DrawLine(transform.position, mPointX, Color.red);
            Debug.DrawLine(transform.position, mPointY, Color.yellow);
        }
    }
}
