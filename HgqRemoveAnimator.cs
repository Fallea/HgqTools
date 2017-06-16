using UnityEngine;
using System.Collections;

public class HgqRemoveAnimator : MonoBehaviour
{


    public bool remove = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (remove)
        {
            remove = false;
            RemoveAnimator();
        }
    }

    private void RemoveAnimator()
    {
        Animator[] animators = this.gameObject.GetComponentsInChildren<Animator>();
        for (int i = 0; i < animators.Length; i++)
        {
            GameObject.Destroy(animators[i]);
        }
    }
}
