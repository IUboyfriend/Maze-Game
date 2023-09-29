using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatueSubtitleController : MonoBehaviour
{
    public float letterDelay = 0.1f; // Time delay between each letter
    public string[] subtitles; // Array of subtitles
    public static int currentSubtitleIndex = 0;
    private string currentText = "";
    public static bool isDisplaying = false;
    private Coroutine displayCoroutine; // Store reference to coroutine
    public GameObject subtitle_canvas;
    public static bool isPlaying = false;
    public static bool hasFinished = false;
    private bool firstClick = false;

    public static bool shouldDisplayNext = false;

    void Start()
    {
        subtitles = new string[100];
        subtitles[0] = "You got the blessings of the God! Check the map to see the position of the exit!"; //Map statue
        subtitles[1] = "The god made you become stronger. Your max health increases!"; //Elder statue
        subtitles[2] = "The god made your weapons become more powerful. The powering time has greatly reduced!"; //Elder statue
        subtitles[3] = "The god provided you with a magic bag. You can bring more objects with you now!"; //Elder statue
        subtitles[4] = "The god made you become more swift. Your movement is faster and the coldness value accumulates slower!"; //Elder statue
        subtitles[5] = "The fire god increased the range of the bonfires for you!"; //Elder statue
        subtitles[6] = "The fire god  offered you an extra bonfire at the center of the mountain!"; //Elder statue
        subtitles[7] = "The fire god taught you how to dismantle a bonfire! Get near to a bonfire and press Z to dismantle it. You only get one chance so value this ability! "; //Elder statue

        GetComponent<TextMeshProUGUI>().text = "";
    }


    void Update()
    {
        if (shouldDisplayNext)
        {
            shouldDisplayNext = false;
            if (currentSubtitleIndex < subtitles.Length)
            {
                isDisplaying = true;
                displayCoroutine = StartCoroutine(DisplaySubtitle(subtitles[currentSubtitleIndex]));

            }
        }


        if (Input.GetMouseButtonDown(0) && isDisplaying && firstClick == false) // If player clicks while subtitle is displaying
        {
            StopCoroutine(displayCoroutine); // Stop coroutine
            GetComponent<TextMeshProUGUI>().text = subtitles[currentSubtitleIndex]; // Display full subtitle
            firstClick = true;

        }
        else if (firstClick && Input.GetMouseButtonDown(0))
        {
            GetComponent<TextMeshProUGUI>().text = "";
            transform.parent.gameObject.SetActive(false);
            isDisplaying = false;
            firstClick = false;

        }
    }

    private void OnEnable()
    {
        GetComponent<AudioSource>().Play();
    }
    public IEnumerator DisplaySubtitle(string subtitle)
    {
        currentText = "";
        for (int i = 0; i < subtitle.Length; i++)
        {
            currentText += subtitle[i];
            GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(letterDelay);
        }

        while (!Input.GetMouseButtonDown(0)) // Wait for another click
        {
            yield return null;
        }
        GetComponent<TextMeshProUGUI>().text = "";
        transform.parent.gameObject.SetActive(false);
        isDisplaying = false;
    }

}
