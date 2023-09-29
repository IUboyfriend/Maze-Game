using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneInitializeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Transform playerStartPosition;
    public float playerWalkDistance = 2f;
    public GameObject player;
    private bool scene_initialized = false;
    
    //private SimulatedInput simulatedInput;
    private void Start()
    {
        fadeImage.gameObject.SetActive(true);
        player.transform.Find("Light_2D_torch").gameObject.SetActive(false);
        MazeSuccess.col = 0;
        // Set the alpha value of the fade image to 1
        Color startColor = fadeImage.color;
        startColor.a = 1f;
        fadeImage.color = startColor;

        // Start the fade-in coroutine
        StartCoroutine(FadeIn());
    }
    private IEnumerator FadeIn()
    {
        // Initialize the scene if it hasn't been initialized yet
        if (!scene_initialized)
        {
            InitializeScene();
            scene_initialized = true;
        }
        // Wait for a short delay before starting the fade-in
        yield return new WaitForSeconds(0.5f);

        // Gradually decrease the alpha value of the fade image
        float elapsedTime = 0f;
        Color currentColor = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            currentColor.a = alpha;
            fadeImage.color = currentColor;
            yield return null;
        }
        // Set the alpha value of the fade image to 0
        currentColor.a = 0f;
        fadeImage.color = currentColor;
        fadeImage.gameObject.SetActive(false);
    }

    private void InitializeScene()
    {
        // Set the player's position to the start position
        if (playerStartPosition != null)
        {
            var playerTransform = player.transform;
            playerTransform.position = playerStartPosition.position;
            playerTransform.rotation = playerStartPosition.rotation;
        }
    }
}
