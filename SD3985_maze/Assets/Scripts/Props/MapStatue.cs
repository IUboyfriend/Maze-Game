using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapStatue : Statue
{
    public GameObject destination;
    private GameObject subtitleObject;
    private GameObject map_canvas;
    private GameObject canvas;

    private void Start()
    {
        statue = GameObject.Find("BigMap");
        map_canvas = GameObject.Find("BigMap");


        canvas = GameObject.Find("Canvas");
        if(SceneManager.GetActiveScene().name == "Level1")
        {
            hasConsecrated = true;
            CreateEffect();
        }
    }

    public override void CreateEffect()
    {
        //statue.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true);
        statue.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        totalNumStatue++;
        if (SceneManager.GetActiveScene().name == "Level0")
        {
            canvas.transform.GetChild(1).GetChild(12).GetChild(2).gameObject.SetActive(true);
            destination.gameObject.SetActive(true);
            FirstLevelTutorial.hasOffered = true;
        }
        else
        {
            canvas.transform.GetChild(1).GetChild(9).GetChild(2).gameObject.SetActive(true);
            subtitleObject = GameObject.Find("StatueIntroduction");
            StartCoroutine(IntroductionStatue());
            destination.gameObject.SetActive(true);
        }

    }

    public void ShowEffect()
    {
        map_canvas.transform.GetChild(1).gameObject.SetActive(true);
    }
    private void startDisplay()
    {
        StatueSubtitleController.currentSubtitleIndex = 0;
        StatueSubtitleController.shouldDisplayNext = true;
        StatueSubtitleController.isDisplaying = true;
        subtitleObject.transform.GetChild(0).gameObject.SetActive(true);
        
    }

    private IEnumerator IntroductionStatue()
    {
        startDisplay();
        while (StatueSubtitleController.isDisplaying)
        {
            yield return null;
        }
        StoneInteractions.allowPowering = true;
        BranchInteraction.allowPowering = true;
    }
}
