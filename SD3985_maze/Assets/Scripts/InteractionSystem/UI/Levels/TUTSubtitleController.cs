using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TUTSubtitleController : MonoBehaviour
{
    public float letterDelay = 0.1f; // Time delay between each letter
    public string[] subtitles; // Array of subtitles
    private int currentSubtitleIndex = 0;
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
        subtitles[0] = "You have wandered into the dark and dangerous mountain. The wind screams in your ears, and the night engulfs you. You must find your way out, before it’s too late!";
        subtitles[1] = "Thanks the god, your flashlight still works! Press L to turn it on! Use the mouse to adjust the lighting direction.";
        subtitles[2] = "Look! Here is a branch! Pick it up by pressing E! It can protect you from the wild animals!";
        subtitles[3] = "Hold the left mouse button to charge up the stick. Release the left mouse button to swing the stick. The longer you charge, the higher the damage.";
        subtitles[4] = "Here is a stone! It can protect you, too! Press E to pick it up!";
        subtitles[5] = "Press the number key 1 to switch to the stone you just picked up!";
        subtitles[6] = "Hold down the left mouse button and release to throw a stone!";
        subtitles[7] = "It's so cold, you’re about to freeze into an ice cube! As your coldness level rises, your movement will become slower and slower!";
        subtitles[8] = "Wow! Look, there is a shrine over there! Press E to offer the shrine to get blessings from the god!";
        subtitles[9] = "With the help of the god, you know where the exit is on the map! You can check it later but now the most urgent thing is to build a bonfire.";
        subtitles[10] = "To build a bonfire, you need to gather 2 branches and 2 stones together. Press E to pick them up!";
        subtitles[11] = "Continuously press Q to drop all the materials on the ground and a bonfire will be built! Bonfires can be very useful because it can clear your coldness value and light up the way!";
        subtitles[12] = "You should choose wisely where to build the bonfires because there is a limit of the total number of bonfires in each level! Normally, you cannot dismantle a bonfire already built.";
        subtitles[13] = "You are not at the best status now! Press E to pick up the potion and press the number key 3 to switch to it. It can help you recover your health!";
        subtitles[14] = "Press E to pick up the meat and press the number key 4 to switch to it. Long press to take the meat to reduce the coldness value!";
        subtitles[15] = "Now you have to face the darkness alone. Be careful of the wild animals! Good luck!";

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
            else
            {
                //进入第一关正常游戏

                subtitle_canvas.gameObject.SetActive(false);// All subtitles have been displayed, do something else here
            }
        }


        if (Input.GetMouseButtonDown(0) && isDisplaying && firstClick == false) // If player clicks while subtitle is displaying
        {
            StopCoroutine(displayCoroutine); // Stop coroutine
            GetComponent<TextMeshProUGUI>().text = subtitles[currentSubtitleIndex]; // Display full subtitle
            firstClick = true;

        }
        else if(firstClick && Input.GetMouseButtonDown(0))
        {
            GetComponent<TextMeshProUGUI>().text = "";
            transform.parent.gameObject.SetActive(false);
            isDisplaying = false;
            firstClick= false;
            currentSubtitleIndex++;

        }

    }

    IEnumerator DisplaySubtitle(string subtitle)
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
        currentSubtitleIndex++;
    }

}

