using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;


public class ShowBigMap : MonoBehaviour, IPointerClickHandler
{
    public GameObject bigMapCanvas;
    public TextMeshProUGUI showtext;
    public static int[] maxAmount= {5,8,10 };
    public static int currentMaxAmount;
    public static int currentAmount;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
    }
    void Start()
    {


        if (SceneManager.GetActiveScene().name == "Level1")
        {
            currentMaxAmount = maxAmount[0];
        }
        else if(SceneManager.GetActiveScene().name == "Level2")
        {
            currentMaxAmount = maxAmount[1];

        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {

            currentMaxAmount = maxAmount[2];
        }
    }
    
    public void EnableBigMap()
    {
        bigMapCanvas.SetActive(true);
    }

    public void DisableBigMap()
    {
        bigMapCanvas.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Level0")
        {
            return;
        }
        else
        {
            if (showtext != null)
            {
                currentAmount = GameObject.FindGameObjectsWithTag("Bonfire").Length;
                showtext.text = currentAmount.ToString() + "/" + currentMaxAmount.ToString();
            }
            else
            {
                Debug.Log("humm?");
            }
            EnableBigMap();
        }
       
    }

}
