using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class InitialScreen : MonoBehaviour
{
    public GameObject cloudAnimationObject;
    public Camera mainCamera;
    public float cameraShrinkSpeed = 2f;
    public float cameraFinalSize = 4f;

    private bool buttonClicked = false;
    private bool levelButtonClicked = false;
    private string levelscene1 = "Level0";
    private string levelscene2 = "Level2";
    private string levelscene3 = "Level3";

    public Image blackImage;
    public float transitionDuration = 1.0f;
    private Image ccc;
    private TextMeshProUGUI text;
    //public static int access;
    private GameObject b;
    private GameObject c;
    private GameObject d;
    //public void restartWholeGame()
    //{
    //    //if (access < 2)
    //    //{
    //    //    access += 1;
    //    //}
    //    //else
    //    //{
    //    //    access = 0;
    //    //}
    //    //SaveByJson(access);

    //    SceneManager.LoadScene("HomeScene");
    //}
    //public void SaveByJson(int level)
    //{
    //    SaveGame save = new SaveGame();
    //    save.SaveProcess(level);
    //    string jsonString = JsonUtility.ToJson(save);
    //    StreamWriter sw = new StreamWriter(Application.dataPath + "/Save/DataByJson.txt");
    //    sw.Write(jsonString);
    //    sw.Close();
    //}
    private void Start()
    {
        transform.parent.GetChild(3).GetComponent<Image>().raycastTarget = false;
        transform.parent.GetChild(4).GetComponent<Image>().raycastTarget = false;
        transform.parent.GetChild(5).GetComponent<Image>().raycastTarget = false;
        b = GameObject.Find("levelButton");
        c = GameObject.Find("levelButton (1)");
        d = GameObject.Find("levelButton (2)");
        //if (!(access == 0))// not first time enter home scene for each game play
        //{
        //    cloudAnimationObject.SetActive(false);
        //    //gameObject.SetActive(false);
        //    gameObject.GetComponent<Button>().enabled = false;
        //    gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        //    gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 0);
        //    b.GetComponent<Button>().interactable = true;
        //    if (access > 1)
        //    {
        //        c.GetComponent<Button>().interactable = true;
        //        if (access > 2)
        //        {
        //            d.GetComponent<Button>().interactable = true;
        //        }
        //    }
        //}
    }
    public void OnButtonClick()
    {
        //LoadByJson();
        transform.parent.GetChild(3).GetComponent<Image>().raycastTarget = true;
        transform.parent.GetChild(4).GetComponent<Image>().raycastTarget = true;
        transform.parent.GetChild(5).GetComponent<Image>().raycastTarget = true;
        GetComponent<Image>().raycastTarget = false;
        var cloudAnimator = cloudAnimationObject.GetComponent<Animator>();
        if (cloudAnimator != null)
        {
            //cloudAnimator.enabled = true;
            cloudAnimator.SetBool("start", true);
        }
        if (buttonClicked) return;
        buttonClicked = true;
        //GameObject b = GameObject.Find("levelButton");
        //GameObject c = GameObject.Find("levelButton (1)");
        //GameObject d = GameObject.Find("levelButton (2)");
        b.GetComponent<Button>().interactable = true;
            c.GetComponent<Button>().interactable = true;
                d.GetComponent<Button>().interactable = true;
        //if (access > 0)
        //{
        //    if (access > 1)
        //    }
        //    {
        //}

        // Hide the button

        StartCoroutine(hideButton());
        gameObject.GetComponent<Image>().raycastTarget = false;
        gameObject.GetComponent<Button>().enabled = false;

        // Start playing the cloud animation

        //// Shrink the camera
        //if (gameObject.activeInHierarchy)
        //{
        //    StartCoroutine(ShrinkCamera());
        //}
    }
    public void OnLevelButtonClick(int i)
    {
        if (levelButtonClicked) return;
        levelButtonClicked = true;
        blackImage.gameObject.SetActive(true);
        StartCoroutine(TransitionToNextScene(i));
    }
    IEnumerator hideButton()
    {
        float t = 0.0f;
        ccc = gameObject.GetComponent<Image>();
        text = gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Color cc = ccc.color;
        while (t < transitionDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, t / transitionDuration);
            cc.a = alpha;
            ccc.color = cc;
            text.color = cc;
            yield return null;
            t += Time.deltaTime;
        }
    }
    IEnumerator TransitionToNextScene(int i)
    {
        string levelname;
        float t = 0.0f;
        while (t < transitionDuration)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, t / transitionDuration);
            blackImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            yield return null;
            t += Time.deltaTime;
        }
        levelname = i == 1 ? levelscene1 : (i == 2 ? levelscene2 : levelscene3);
        //Debug.Log(levelname);
        SceneManager.LoadScene(levelname);
    }
    //public void GoLevel3()
    //{
    //    SceneManager.LoadScene("Level3");
    //}
    //public void LoadByJson()
    //{
    //    if (File.Exists(Application.dataPath + "/save/databyjson.txt"))
    //    {
    //        StreamReader sr = new StreamReader(Application.dataPath + "/save/databyjson.txt");
    //        string jsonstring = sr.ReadToEnd();
    //        sr.Close();
    //        SaveGame save = JsonUtility.FromJson<SaveGame>(jsonstring);
    //        access = save.pass_level_numner;
    //    }
    //    else
    //    {
    //        print("no save.");
    //    }
    //}
}
