using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VenomEffect : MonoBehaviour
{
    public float duration = 10f;
    private float fadeDuration = 5f;
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    // Start is called before the first frame update
    private CharacterStatus characterStatus;
    void Start()
    {
        characterStatus = GameObject.Find("Player").GetComponent<CharacterStatus>();
        StartCoroutine(ScaleObjectRoutine());
    }

    void OnTriggerStay2D (Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player is in venom");
            characterStatus.AddPoisoningTime(2 * Time.deltaTime);
        }
    }


    private IEnumerator ScaleObjectRoutine(float scatterTime = 0.2f)
    {
        float timer = 0.0f;
        Vector3 targetScale = transform.localScale;
        Vector3 currentScale;
        while (timer <= scatterTime)
        {
            // Calculate the current scale based on the time elapsed
            currentScale = Vector3.Lerp(Vector3.zero, targetScale, timer / scatterTime);

            // Apply the current scale to the object
            transform.localScale = currentScale;

            // Increase the timer
            timer += Time.deltaTime;

            // Wait for the end of the frame
            yield return null;
        }

        // Make sure the object is scaled to the target scale at the end of the animation
        transform.localScale = targetScale;
        StartCoroutine(DestroyObjectOverTime());
    }

    private IEnumerator DestroyObjectOverTime()
    {
        // Debug.Log("DestroyObjectOverTime" + duration);
        yield return new WaitForSeconds(duration);
        // Get the renderer component of the object
        Renderer renderer = GetComponent<Renderer>();
        
        // Store the initial alpha value of the object's material
        float initialAlpha = renderer.material.color.a;
        // Loop until the fadeDuration has elapsed
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            // Calculate the new alpha value based on the time elapsed and fadeDuration
            float newAlpha = Mathf.Lerp(initialAlpha, 0.1f, elapsedTime / fadeDuration);

            // Set the alpha value of the object's material color
            Color newColor = renderer.material.color;
            newColor.a = newAlpha;
            renderer.material.color = newColor;

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Destroy the object
        Destroy(gameObject);
    }
}
