using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireRangeIncrease : MonoBehaviour
{

    public static bool shouldIncrease = false;
    void Start()
    {
        shouldIncrease = false;
    }
    void Update()
    {
        if (shouldIncrease)
        {
            GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = 50f;

        }
    }
}
