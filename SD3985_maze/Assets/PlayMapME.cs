using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMapME : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<AudioSource>().Play();

    }
}
