using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MazeSuccess : MonoBehaviour
{
    public static int col = 0;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        col = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Triggerenetered!");
        if (gameObject.name == "exitPlace")
        {
            col = 1;
        }else if (gameObject.name == "startPlaceBlock")
        {
            col = 2;
        }
    }
}
