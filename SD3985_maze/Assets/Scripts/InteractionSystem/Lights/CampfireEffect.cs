using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine;

public class CampfireEffect : MonoBehaviour
{
    public Transform playerTransform;
    public Transform campfireTransform;
    public PostProcessVolume postProcessVolume;
    public float maxDistance = 6f;
    public float maxIntensity = 2f;
    public float minIntensity = 0.2f;

    private float distance;
    private float intensity;

    void Update()
    {
        // Calculate the distance between the player and the campfire
/*        distance = Vector3.Distance(playerTransform.position, campfireTransform.position);*/

        // Calculate the intensity based on the distance and the max/min intensity values
        intensity = Mathf.Lerp(minIntensity, maxIntensity, 1);
        //Debug.Log(intensity);
        // Set the post-process volume's intensity property based on the calculated intensity
        if (intensity <= 0.4f)
        {
            postProcessVolume.weight = 0.4f;
        }
        else
        {
            postProcessVolume.weight = intensity;
        }
    }
}
