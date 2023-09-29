using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FirstLevelTutorial : MonoBehaviour
{
    private GameObject player;
    public GameObject arrow;
    public GameObject word;
    private Transform arrowposition;
    private Transform arrowposition2;
    private Transform wordposition;
    private Rigidbody2D rigidbody2d;
    private AudioSource audio_player;
    private Animator branch_animator;
    private Animator animator;
    private Image blackImage;
    private AudioSource tut_audio;
    private float run_speed;
    private float speed;
    private float horizontalInput;
    private float verticalInput;

    private bool L_pressed;
    private bool mouse_clicked;
    private bool E_pressed;
    private bool Q_pressed;
    private bool one_pressed;
    private bool shift_pressed;
    private bool validPress;
    private bool validClick;

    public static bool completed;
    public static bool allowBranchWave;//though name as branch wave, in fact used as a common tester for other scripts
    public static bool allowThrowStones = false;
    public static bool faceLeft;
    public static bool faceRight;
    public static bool bonfire_built;
    private bool pass;


    private GameObject subtitleObject;

    public static bool hasPickedUpBranch = false;
    public static bool hasPickedUpStone = false;
    public static bool hasPress1 = false;
    public static bool hasOffered = false;
    public static bool hasTakenPotion = false;
    public static bool hasTakenMeat = false;




    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        hasOffered=hasPress1 = hasPickedUpStone = hasPickedUpBranch = completed = allowBranchWave = bonfire_built = faceRight = faceLeft = false;
    }


    void Start()
    {
        CharacterStatus.currentColdness = 0;
        CharacterStatus.coldnessIncrease = 0f;
        CharacterStatus.currentHealth = 90;
        faceRight = faceLeft = completed = validClick = validPress = bonfire_built = false;
        allowBranchWave = false;
        player = GameObject.Find("Player");
        arrowposition = GameObject.Find("Canvas").transform.GetChild(1).GetChild(6);
        arrowposition2 = arrowposition.parent.GetChild(7);
        wordposition= arrowposition.parent.GetChild(8);
        blackImage = arrowposition.parent.parent.GetChild(3).GetComponent<Image>();
        animator = player.GetComponent<Animator>();
        audio_player = player.GetComponent<AudioSource>();
        rigidbody2d = player.GetComponent<Rigidbody2D>();
        branch_animator = player.transform.GetChild(4).gameObject.GetComponent<Animator>();
        speed = 1;
        run_speed = 0;
        one_pressed = mouse_clicked = Q_pressed = E_pressed = L_pressed = shift_pressed = false;
        pass = false;
        tut_audio = GameObject.Find("Environment").transform.GetChild(2).GetChild(2).gameObject.GetComponent<AudioSource>();

        subtitleObject = GameObject.Find("TUTSubtitle");
        StartCoroutine(GoTUT());
    }


    private void startDisplay()
    {
        TUTSubtitleController.shouldDisplayNext = true;
        TUTSubtitleController.isDisplaying = true;
        subtitleObject.transform.GetChild(0).gameObject.SetActive(true);
    }
    

    private IEnumerator GoTUT()
    {
        if (!pass)
        {
            setInput(0, -1, 1);
            yield return new WaitForSeconds(2);
            setInput(0, 0, 0);
            yield return new WaitForSeconds(.5f);
            player.transform.GetChild(3).gameObject.SetActive(true);
            player.transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            player.transform.GetChild(3).gameObject.SetActive(false);

            startDisplay();//0 background
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }


            startDisplay();//1 press E to turn on the flashlight
            
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }


            word.GetComponent<TextMeshProUGUI>().text = "\"L\"";
            GameObject g = Instantiate(word, wordposition);
            validPress = true;//while show this press-request subtitle, open the access of the bool variable.(prevent casual pressing).
            L_pressed = false;
            while (!L_pressed)// force process to wait for L key.
            {
                yield return null;
            }
            validPress = false;

            if (L_pressed)
            {
                Destroy(g);
                yield return new WaitForSeconds(0.5f);
            }
            setInput(0, -1, 1);
            yield return new WaitForSeconds(2);
            setInput(0, 0, 0);



            startDisplay();//2 pick up the branch
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            setInput(0, -1, 1);
            yield return new WaitForSeconds(.5f);
            setInput(0, 0, 0);
            validPress = true;
            allowBranchWave = true;
            E_pressed = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"E\"";
            GameObject g1 = Instantiate(word, wordposition);
            while (hasPickedUpBranch == false)
            {
                yield return null;
            }
            Destroy(g1);
            hasPickedUpBranch = false;
            allowBranchWave = validPress = E_pressed = false;


            startDisplay();//3 wave the branch
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            validClick = true;

            mouse_clicked = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"Hold down left mouse\"";
            GameObject g2 = Instantiate(word, wordposition);
            while (!mouse_clicked || subtitleObject.transform.GetChild(0).gameObject.activeSelf)
            {
                yield return null;
            }// This is the time when the subtitle disappear, but not the user waves the branch.

            mouse_clicked = false;
            allowBranchWave = true;

            while (!mouse_clicked || subtitleObject.transform.GetChild(0).gameObject.activeSelf)
            {
                yield return null;
            }

            allowBranchWave = false;
            validClick = false;




            /*            //subtitle 5: accumulate power        * It's also allowed if player just short click.
                        validClick = true;
                        allowBranchWave = true;
                        mouse_clicked = false;
                        while (!mouse_clicked)
                        {
                            yield return null;
                        }
                        allowBranchWave = false;
                        validClick = false;
                        yield return new WaitForSeconds(1f);
            */


            Destroy(g2);

            

            startDisplay();//4 Press E to pick up a stone
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            setInput(0, -1, 1);
            yield return new WaitForSeconds(1f);
            setInput(0, 0, 0);
          

            setInput(0, -1, 1);
            yield return new WaitForSeconds(.5f);
            setInput(0, 0, 0);

            validPress = true;
            allowBranchWave = true;
            E_pressed = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"E\"";
            GameObject g3 = Instantiate(word, wordposition);
            while (!hasPickedUpStone)
            {
                yield return null;
            }
            Destroy(g3);
            allowBranchWave = validPress = E_pressed = false;
            hasPickedUpStone = false;


            startDisplay();//5 Press the number key 1 to switch to the stone!
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            validPress = true;
/*            allowBranchWave = true;*/
            one_pressed = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"1\"";
            GameObject g4 = Instantiate(word, wordposition);

            while (! hasPress1)
            {
                yield return null;
            }
            Destroy(g4);
            allowBranchWave = validPress = one_pressed = false;
            hasPress1 = false;



            startDisplay();//6 throw a stone
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            //(now seems that the player can switch to branch again? but I dont want to care this kind of action)

            validClick = true;
            mouse_clicked = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"Hold down left mouse\"";
            GameObject g5 = Instantiate(word, wordposition);

            while (!mouse_clicked || subtitleObject.transform.GetChild(0).gameObject.activeSelf)
            {
                yield return null;
            }// This is the time when the subtitle disappear, but not the user throws the stone.
            Destroy(g5);
            mouse_clicked = false;
            allowBranchWave = true;
            allowThrowStones = true;

            while (!mouse_clicked || subtitleObject.transform.GetChild(0).gameObject.activeSelf)
            {
                yield return null;
            }
            allowThrowStones = false;
            allowBranchWave = false;
            validClick = false;




            startDisplay();//7 coldness value
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            CharacterStatus.coldnessIncrease = 1.5f;
            arrowposition.transform.rotation *= Quaternion.Euler(0, 0, 45);
            GameObject arw = Instantiate(arrow, arrowposition);
            Destroy(GameObject.Find("Stone_to_destroy"));
            yield return new WaitForSeconds(2f);
            Destroy(arw);


            setInput(0, -1, 1);
            yield return new WaitForSeconds(1.5f);
            setInput(0, 0, 0);
            faceLeft = true;
            setInput(-1, 0, 1);
            yield return new WaitForSeconds(.2f);
            setInput(0, 0, 0);
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 0);
            yield return new WaitForSeconds(2f);
            setInput(-1, 0, 1);
            yield return new WaitForSeconds(2f);
            setInput(0, 0, 0);
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 0);




            startDisplay();//8 press E to offer the shrine
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            validPress = true;
            E_pressed = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"E\"";
            GameObject g6 = Instantiate(word, wordposition);

            while (!hasOffered)
            {
                yield return null;
            }
            Destroy(g6);
            validPress = E_pressed = false;
            hasOffered = false;
            yield return new WaitForSeconds(1f);




            startDisplay();//9 shrine effect
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }

            // done pray, go back
            faceLeft = false;
            faceRight = true;
            setInput(1, 0, 1);
            yield return new WaitForSeconds(2f);
            setInput(0, 0, 0);
            faceRight = false;
            startDisplay();//10 Pick up two more stone and one more branch
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            validPress = true;
            allowBranchWave = true;
            E_pressed = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"E\"";
            GameObject g7 = Instantiate(word, wordposition);

            while ( !hasPickedUpStone)
            {
                yield return null;
            }
            Destroy(g7);
            hasPickedUpStone = false;
            validPress = allowBranchWave = E_pressed = false;
            yield return new WaitForSeconds(1f);


            setInput(0, -1, 1);// go down
            yield return new WaitForSeconds(2f);
            setInput(0, 0, 0);
            validPress = true;
            allowBranchWave = true;
            E_pressed = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"E\"";
            GameObject g8 = Instantiate(word, wordposition);

            while (!hasPickedUpBranch || !hasPickedUpStone)
            {
                yield return null;
            }
            Destroy(g8);
            hasPickedUpBranch = hasPickedUpStone = false;
            validPress = allowBranchWave = E_pressed = false;


            startDisplay();//11 Press Q to drop all the items to build a bonfire
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            validPress = true;
            allowBranchWave = true;
            bonfire_built = false;
            word.GetComponent<TextMeshProUGUI>().text = "\"Q! (you may need to use number key to switch weapon)\"";
            GameObject g9 = Instantiate(word, wordposition);

            while (!bonfire_built)
            {
                yield return null;
            }
            Destroy(g9);
            validPress = allowBranchWave = false;


            startDisplay();//12 The effect of the bonfire
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }




            faceLeft = false;
            faceRight = true;
            setInput(1, 0, 1);
            yield return new WaitForSeconds(2f);
            setInput(0, 0, 0);
            faceRight = false;




            yield return new WaitForSeconds(1f);
            setInput(0, -1, 1);// go down
            yield return new WaitForSeconds(2f);
            setInput(0, 0, 0);


            faceLeft = true;
            faceRight = false;
            setInput(-1, 0, 1);
            yield return new WaitForSeconds(2f);
            setInput(0, 0, 0);
            faceRight = true;


            startDisplay();//13 take the potion
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }

            word.GetComponent<TextMeshProUGUI>().text = "\"Press E and Long press left mouse key to take the potion\"";


            GameObject g10 = Instantiate(word, wordposition);
            while (!hasTakenPotion)
            {
                yield return null;
            }
            hasTakenPotion= false;
            Destroy(g10);


            yield return new WaitForSeconds(1f);
            setInput(0, -1, 1);// go down
            yield return new WaitForSeconds(2f);
            setInput(0, 0, 0);

            startDisplay();//14 take the Meat
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
            word.GetComponent<TextMeshProUGUI>().text = "\"Press E and Long press left mouse key to take the meat\"";

            GameObject g11 = Instantiate(word, wordposition);
            while (!hasTakenMeat)
            {
                yield return null;
            }
            hasTakenMeat = false;
            Destroy(g11);



            startDisplay();//15 End the TUT
            while (TUTSubtitleController.isDisplaying)
            {
                yield return null;
            }
/*            arrowposition2.transform.rotation *= Quaternion.Euler(0, 0, 180);*/
/*            GameObject arw2 = Instantiate(arrow, arrowposition2);
            yield return new WaitForSeconds(2f);

            Destroy(arw2);

*/
            //end tut.
            yield return new WaitForSeconds(1f);
            completed = true;
            StartCoroutine(TransitionToNextScene());

        }



    }
    private void setInput(int h, int v, int s)
    {
        speed = s;
        horizontalInput = h;
        verticalInput = v;
        animator.SetFloat("Horizontal", h);
        animator.SetFloat("Vertical", v);
    }

    IEnumerator TransitionToNextScene()
    {
        CharacterStatus.coldnessIncrease = 1.5f;
        completed = allowBranchWave = bonfire_built = faceRight = faceLeft = true;
        one_pressed = mouse_clicked = Q_pressed = E_pressed = L_pressed = shift_pressed = true;

        float t = 0.0f;
        while (t < 1)
        {
            float alpha = Mathf.Lerp(0.0f, 1.0f, t / 1);
            blackImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
            tut_audio.volume -= 0.01f;
            yield return null;
            t += Time.deltaTime;
        }
        SceneManager.LoadScene("Level1");
    }

    void Update()
    {
        if (validPress)
        {
            if (Input.GetKey(KeyCode.E) && !E_pressed)
            {
                Debug.Log("E pressed");
                E_pressed = true;
            }
            if (Input.GetKey(KeyCode.L) && !L_pressed)
            {
                Debug.Log("L pressed");
                L_pressed = true;
                player.transform.Find("Light_2D_torch").gameObject.SetActive(true);
            }
            if (Input.GetKey(KeyCode.Alpha1))
            {
                Debug.Log("Number 1 pressed");
                one_pressed = true;
            }
            if (Input.GetKey(KeyCode.Q) && !Q_pressed)
            {
                Debug.Log("Q pressed");
                Q_pressed = true;
            }
        }
        if (validClick)
        {
            if (Input.GetMouseButtonUp(0) && !mouse_clicked)
            {
                Debug.Log("Clicked once");
                mouse_clicked = true;
            }
        }
    }

    void FixedUpdate() //control character movement as usual
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

            shift_pressed = Input.GetKey("left shift") ? true : false;//spped up when pressed shift key
            run_speed = shift_pressed ? speed : 0;

            // Use rigidbody to control movement.
            Vector2 iposition = rigidbody2d.position;
            iposition.x += (speed + run_speed) * horizontalInput * Time.deltaTime;
            iposition.y += (speed + run_speed) * verticalInput * Time.deltaTime;
            rigidbody2d.MovePosition(iposition);
        }
        else
        {
            if (audio_player.isPlaying)
            {
                audio_player.Stop();
            }

            animator.SetBool("isWalking", false);
            if (StoneInteractions.isPowering || BranchInteraction.isPowering)
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
        }
    }
    public void GoLevelOne()
    {
        completed = true;
        StartCoroutine(TransitionToNextScene());
    }
}
