using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class CharacterController : MonoBehaviour
{
    public static float max_speed = 6.5f;
    public float run_speed;
    public float speed;
    float horizontalInput;
    float verticalInput;
    Animator animator;
    Animator branch_animator;
    Rigidbody2D rigidbody2d;
    AudioSource audio_player;
    //private float current_speed;
    private bool shift_pressed = false;
    public Image fadeImage;
    private float fadeDuration = 1f;
    bool wholeCorStarted = false;
    bool corStarted00 = false;
    bool corStarted0 = false;
    bool corStarted1 = false;
    private CharacterStatus status;
    //private SwitchWeapon weaponSwitcher;
    void Start()
    {
        animator = GetComponent<Animator>();
        branch_animator = transform.GetChild(4).gameObject.GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audio_player = GetComponent<AudioSource>();
        status = GetComponent<CharacterStatus>();
        speed = max_speed;
        run_speed = 0;
        //weaponSwitcher = GetComponent<SwitchWeapon>();
        if (!(SceneManager.GetActiveScene().name == "Level0"))
        {
            FirstLevelTutorial.completed = true;
        }
    }

    void Update()
    {
        if (MazeSuccess.col != 0)
        {
            if (MazeSuccess.col == 2)//start
            {
                gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);//question mark
                gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(false);//like mark
            }
            else
            {
                //string n = SceneManager.GetActiveScene().name;
                //InitialScreen.access = n == "Level1" ? 1 : 2;
                //SaveByJson(InitialScreen.access);
                gameObject.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);//like mark
                gameObject.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(false);//question mark
            }
            if (!wholeCorStarted)
            {
                StartCoroutine(ExitMaze());
            }
        }
        else
        {
            wholeCorStarted = false;
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            speed = (1 - CharacterStatus.currentColdness / CharacterStatus.maxColdness) * max_speed;// change the speed according to the coldness value
            speed = Mathf.Max(speed, 3.5f);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject light = GameObject.Find("Player").transform.Find("Light_2D_torch").gameObject;
            light.SetActive(!light.activeInHierarchy);
        }
    }

    void FixedUpdate()
    {

        if ((horizontalInput != 0f || verticalInput != 0f))
        {
            if (!audio_player.isPlaying)
            {
                audio_player.Play();
            }
            animator.SetBool("isWalking", true);
            animator.SetFloat("Horizontal", horizontalInput);
            animator.SetFloat("Vertical", verticalInput);

            shift_pressed = status.damaged ? true : false;//spped up when pressed shift key
                                                                                        //or being damaged in 1 second
/*            shift_pressed = Input.GetKey("left shift") || status.damaged ? true : false;//spped up when pressed shift key
                                                                                        //or being damaged in 1 second*/
            run_speed = shift_pressed ? speed : 0;
            audio_player.pitch = shift_pressed ? 1.9f : 1.6f;

            //better use rigidbody to control movement.
            Vector2 iposition = rigidbody2d.position;
            iposition.x = iposition.x + (speed + run_speed) * horizontalInput * Time.deltaTime;
            iposition.y = iposition.y + (speed + run_speed) * verticalInput * Time.deltaTime;
            rigidbody2d.MovePosition(iposition);
        }
        else
        {
            if (audio_player.isPlaying)
            {
                audio_player.Stop();
            }

            animator.SetBool("isWalking", false);
            if (StoneInteractions.isPowering)
            {
                animator.SetBool("isPowering", true);
            }
            else if (BranchInteraction.isPowering)
            {
                animator.SetBool("isPowering", true);
                branch_animator.SetBool("IsPowering", true);
            }
            else
            {
                animator.SetBool("isPowering", false);
                if (branch_animator.gameObject.activeSelf)
                {
                    branch_animator.SetBool("IsPowering", false);
                }
            }

            // get mouse direction and rotate to this direction
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = (mousePosition - transform.position).normalized;
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            if (branch_animator.gameObject.activeSelf)
            {
                branch_animator.SetFloat("InputX", direction.x);
                branch_animator.SetFloat("InputY", direction.y);
            }
        }
    }

    private IEnumerator ExitMaze()
    {
        wholeCorStarted = true;
        if (!corStarted00)
        {
            speed = 1f;
            horizontalInput = 0;
            verticalInput = -1;
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", -1);
            corStarted00 = true;
        }
        yield return new WaitForSeconds(1);
        if (!corStarted0)
        {
            speed = 3;
            horizontalInput = 0;
            verticalInput = 0;
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", -1);
            gameObject.transform.GetChild(3).gameObject.SetActive(true);
            corStarted0 = true;
            //GameObject.Find("MainCamera").GetComponent<CameraController>().enabled = false;
        }
        yield return new WaitForSeconds(2);
        if (!corStarted1)
        {
            gameObject.transform.GetChild(3).gameObject.SetActive(false);
            horizontalInput = 0;
            verticalInput = -1;
            corStarted1 = true;
            //yield return null;
            if (MazeSuccess.col == 2)//start
            {
                MazeSuccess.col = 0;
                yield return null;
                GameObject.Find("MainCamera").GetComponent<CameraController>().enabled = true;
            }
            else if (MazeSuccess.col == 1)//win
            {
                GameObject.Find("MainCamera").GetComponent<CameraController>().enabled = false;
                fadeImage.gameObject.SetActive(true);
                StartCoroutine(FadeOut());
            }
        }
        //yield return new WaitForSeconds(1);
        corStarted00 = false;
        corStarted0 = false;
        corStarted1 = false;
    }
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color currentColor = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            currentColor.a = alpha;
            fadeImage.color = currentColor;
            yield return null;
        }
        fadeImage.transform.parent.GetChild(7).gameObject.SetActive(true);
        
        gameObject.SetActive(false);
        //SceneManager.LoadScene("HomeScene");
    }
    //public void SaveByJson(int level)
    //{
    //    SaveGame save = new SaveGame();
    //    save.SaveProcess(level);
    //    string jsonString = JsonUtility.ToJson(save);
    //    StreamWriter sw = new StreamWriter(Application.dataPath + "/Save/DataByJson.txt");
    //    sw.Write(jsonString);
    //    sw.Close();
    //}
}
