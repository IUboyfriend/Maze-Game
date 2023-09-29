using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ElderStatue : Statue
{
    private List<int> effects;
    public int selectedNumber ;
    private GameObject subtitleObject;
    private GameObject canvas;
    void Start()
    {
        statue = GameObject.Find("BigMap");
        effects = new List<int> { 1, 2, 3, 4 };
        selectedNumber = SelectRandomNumber(effects);
        canvas = GameObject.Find("Canvas");

        Debug.Log("You got ElderStatue " + selectedNumber);
        Debug.Log("1 Increase max health value. 2 Weapon power accumulating speeds up. 3 BackPack capacity +2. 4 Slow Down the coldness value increasing speed and move fast.");

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

        //statue.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(4).GetChild(1).gameObject.SetActive(true);
        statue.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(4).GetChild(1).gameObject.GetComponent<Image>().color=new Color(1,1,1,1);
        totalNumStatue++;
        subtitleObject = GameObject.Find("StatueIntroduction");
        StartCoroutine(IntroductionStatue());
        canvas.transform.GetChild(1).GetChild(9).GetChild(1).gameObject.SetActive(true);

        if (hasConsecrated == true)
        {
            if (selectedNumber == 1)
            {
                CharacterStatus.maxHealth +=20 ;
                CharacterStatus.currentHealth += 20;
            }
            else if (selectedNumber == 2)
            {
                BranchInteraction.maxHoldTime -= 1f;
                StoneInteractions.maxHoldTime -= 1f;

            }
            else if (selectedNumber == 3)
            {
                StoneInteractions.maxHoldingNumber += 2;
                MeatInteraction.maxHoldingNumber += 2;
                BranchInteraction.maxHoldingNumber += 2;
                PotionInteraction.maxHoldingNumber += 2;

                GameObject.Find("Player").GetComponent<StoneInteractions>().UpdateNumberText(GameObject.Find("Player").GetComponent<StoneInteractions>().holdingNumber);
                GameObject.Find("Player").GetComponent<MeatInteraction>().UpdateNumberText(GameObject.Find("Player").GetComponent<MeatInteraction>().holdingNumber);
                GameObject.Find("Player").GetComponent<BranchInteraction>().UpdateNumberText(GameObject.Find("Player").GetComponent<BranchInteraction>().holdingNumber);
                GameObject.Find("Player").GetComponent<PotionInteraction>().UpdateNumberText(GameObject.Find("Player").GetComponent<PotionInteraction>().holdingNumber);

            }
            else if (selectedNumber == 4)
            {
                CharacterStatus.coldnessIncrease -= 0.5f;
                CharacterController.max_speed += 2f;

            }

        }
    }




    public void startDisplay()
    {
        StatueSubtitleController.currentSubtitleIndex = selectedNumber;
        StatueSubtitleController.shouldDisplayNext = true;
        StatueSubtitleController.isDisplaying = true;
        subtitleObject.transform.GetChild(0).gameObject.SetActive(true);

    }

    public IEnumerator IntroductionStatue()
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
