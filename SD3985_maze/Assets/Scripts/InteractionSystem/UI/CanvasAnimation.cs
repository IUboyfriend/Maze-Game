using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnimation : MonoBehaviour
{
    public static bool first;
    private void Start()
    {
        first = true;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        first = true;
    }
    public void disableMe()
    {
        if (first)
        {
            gameObject.GetComponent<Animator>().SetFloat("done", -1);
            gameObject.GetComponent<Animator>().enabled = false;
            first = false;
        }
    }
}
