using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FireGodStatue : Statue
{
    private List<int> effects;
    public int selectedNumber ;
    private GameObject subtitleObject;
    private GameObject canvas;
    public GameObject preBuiltBonfire;


    public static bool couldDismantleBonfire = false;

    void Start()
    {
        effects = new List<int> { 1, 2, 3 };//1 Increase the range of the bonfire. 2 Generate a bonfire at the center of the map. 3 Able to dismantle the bonfire.
        selectedNumber = SelectRandomNumber(effects);
        canvas = GameObject.Find("Canvas");
        Debug.Log("You got FireGod " + selectedNumber);
        Debug.Log("1 Increase the range of the bonfire. 2 Generate a bonfire at the center of the map. 3 Able to dismantle the bonfire.");
        statue = GameObject.Find("BigMap");
    }

    private int SelectRandomNumber(List<int> numbers)
    {
        int index = Random.Range(0, numbers.Count);
        int a = numbers[index];
        return a;
    }
    public void ShowEffect()
    {
        subtitleObject = GameObject.Find("StatueIntroduction");
        StartCoroutine(IntroductionStatue());
    }

    public override void CreateEffect()
    {
        //statue.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(3).GetChild(1).gameObject.SetActive(true);
        statue.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(3).GetChild(1).gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);

        totalNumStatue++;
        subtitleObject = GameObject.Find("StatueIntroduction");
        StartCoroutine(IntroductionStatue());
        canvas.transform.GetChild(1).GetChild(9).GetChild(0).gameObject.SetActive(true);
        if (hasConsecrated == true)
        {
            if (selectedNumber == 1)
            {
                BonfireRangeIncrease.shouldIncrease = true;
                Debug.Log(BonfireRangeIncrease.shouldIncrease);
            }
            else if (selectedNumber == 2)
            {
                preBuiltBonfire.SetActive(true);
                ShowBigMap.currentAmount++;
                ShowBigMap.currentMaxAmount++;
            }
            else if (selectedNumber == 3)
            {
                couldDismantleBonfire = true;

            }

        }
    }

    private void startDisplay()
    {
        StatueSubtitleController.currentSubtitleIndex = selectedNumber + 4;
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
