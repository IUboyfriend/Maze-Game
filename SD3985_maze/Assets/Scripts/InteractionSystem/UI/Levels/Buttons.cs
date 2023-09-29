using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;



public class Buttons : MonoBehaviour
{
    //private Button[] btn;
    public Canvas setting_canv;
    private Image blackimage;
    private AudioSource audio1;
    private AudioSource audio2;
    // Start is called before the first frame update
    void Start()
    {
        setting_canv.gameObject.SetActive(false);
        //transform.GetChild(0).GetComponent<Button>().onClick.AddListener(OnClick); 
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(OnClick);
        transform.GetChild(2).GetComponent<Button>().onClick.AddListener(OnClick);
        blackimage = transform.parent.parent.GetChild(3).GetComponent<Image>();
        audio2 = GameObject.Find("Environment").transform.GetChild(2).GetChild(0).gameObject.GetComponent<AudioSource>();
        audio1 = GameObject.Find("Environment").transform.GetChild(2).GetChild(1).gameObject.GetComponent<AudioSource>();

        //btn[1] = transform.GetChild(1).GetComponent<Button>();
    }

    public void OnClick()
    {
        var button = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (button.name != null)
        {
            if (button.name == "btn_Menu")
            {
                blackimage.gameObject.SetActive(true);
                //SceneManager.LoadScene("HomeScene");
                StartCoroutine(TransitionToNextScene(1));
                ;
            }
            if (button.name == "btn_Setting")
            {
                setting_canv.gameObject.SetActive(!setting_canv.isActiveAndEnabled);
            }
            if (button.name == "btn_Reload")
            {
                blackimage.gameObject.SetActive(true);
                StartCoroutine(TransitionToNextScene(2));
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    IEnumerator TransitionToNextScene(int i)
    {
        string levelname;
        float t = 0.0f;
        while (t < 1)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, t / 1);
            blackimage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            audio1.volume -= 0.1f;
            audio2.volume -= 0.1f;
            yield return null;
            t += Time.deltaTime;
        }
        levelname = i == 1 ? "HomeScene" : SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(levelname);
    }
}
