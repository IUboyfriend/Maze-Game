using UnityEngine;
using UnityEngine.UI;

public class StatueInteraction : MonoBehaviour
{
    private GameObject statue;
    private Statue statueScript;
    public Image powerIndicator;  
    public Image mask;  
    private float maxPowerBarValue = 1f;
    private bool isAccumulating = false;
    private float accumulatorValue = 0f;
    private float accumulatorSpeed = 0.5f;
    private bool hasPressedE = false;





    public static int currentStatueType = 0; //1 firegod, 2 elder, 3 map

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Statue"))
        {
            statue = other.gameObject.transform.parent.gameObject;
            if(statue.GetComponent<FireGodStatue>() != null)
            {
                statueScript = statue.GetComponent<FireGodStatue>();//maybe more types of status
/*                currentStatueType = 1;*/
            }
            if (statue.GetComponent<MapStatue>() != null)
            {
                statueScript = statue.GetComponent<MapStatue>();
            }

            if (statue.GetComponent<ElderStatue>() != null)
            {
                statueScript = statue.GetComponent<ElderStatue>();
            }



        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Statue"))
        {
            statue = null;
            statueScript = null;
/*            currentStatueType = 0;*/
        }
    }

    private void Update()
    {
        if(isAccumulating == false)
        {
            accumulatorValue = 0f;
            hasPressedE = false;
            StoneInteractions.allowPowering = true;
            BranchInteraction.allowPowering = true;
            MeatInteraction.allowPowering = true;
            PotionInteraction.allowPowering = true;
        }


        if(statue == null && hasPressedE)
        {
            accumulatorValue = 0f;
            isAccumulating = false;
            powerIndicator.gameObject.SetActive(false);

        }

        if (statue != null && Input.GetKeyDown(KeyCode.E) && !isAccumulating && statueScript.hasConsecrated == false)
        {
            StoneInteractions.allowPowering = false;
            BranchInteraction.allowPowering = false;
            MeatInteraction.allowPowering = false;
            PotionInteraction.allowPowering = false;
            powerIndicator.gameObject.SetActive(true);
            isAccumulating = true;
            accumulatorValue = 0f;
            hasPressedE = true;
        }
       
        


        if (isAccumulating && statue !=null)
        {
            accumulatorValue += accumulatorSpeed * Time.deltaTime;
            powerIndicator.fillAmount = maxPowerBarValue - accumulatorValue;
            mask.fillAmount = accumulatorValue;
            if (accumulatorValue >= maxPowerBarValue)
            {
                isAccumulating = false;
                accumulatorValue = maxPowerBarValue;
                powerIndicator.gameObject.SetActive(false);
                statueScript.hasConsecrated = true;
                statueScript.CreateEffect();


            }
        }



    }
}
