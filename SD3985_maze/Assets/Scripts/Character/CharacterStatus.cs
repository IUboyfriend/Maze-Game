using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CharacterStatus : MonoBehaviour
{
    public static int maxHealth;
    public static int currentHealth;

    public static float maxColdness;
    public static float currentColdness;
    public GameObject FloatingDamage;
    //private CharacterController controller;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init()
    {
        maxHealth = 100;
        currentHealth = 100;
        maxColdness = 100f;
        currentColdness = 0f;
    }
    public static float coldnessIncrease = 1f;
    public float bonfireDetectionRadius;
    public bool damaged;

    //invincible time related
    public float invincibilityDuration = 1f;
    public float flashInterval = 0.1f;
    private bool isInvincible = false;
    private SpriteRenderer spriteRenderer;

    private Image redScreenOverlay;
    public float blinkDuration = 1f;
    public int blinkCount = 2;
    public Color redColor = new Color32(255, 0, 0, 65);


    private Transform GameOverSound;
    private Transform GetHurtSound;
    private Animator anim;
    private bool isdead;
    public Canvas canvas;
    private Image frost;
    private AudioSource frostME;
    private bool played;
    private float alphaValue;
    public Material default_material;
    private Light2D global_light;

    private float coldTime = 0f;
    private bool deathByCold;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Level0")
            currentHealth = 90; 
        else
            currentHealth = 100;


        maxHealth = 100;

        maxColdness = 100f;
        currentColdness = 0f;
        alphaValue = 0;
        GameOverSound = transform.Find("SE_GameOver");
        GetHurtSound = transform.Find("SE_GetHurt");
        //controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        isdead = false;
        deathByCold = false;
        redScreenOverlay = canvas.transform.GetChild(4).GetComponent<Image>();
        frost = GameObject.Find("BigMap").transform.GetChild(2).GetChild(0).GetComponent<Image>();
        frostME = frost.gameObject.GetComponent<AudioSource>();
        played = false;
        global_light = GameObject.Find("Light 2D_global").GetComponent<Light2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(controller.speed);
        if (currentHealth <= 0 && !isdead)
        {
            deathByCold = false;
            Die();
            isdead = true;
        }
        if (currentColdness < maxColdness)
        {
            if (frost.transform.GetChild(0).GetComponent<AudioSource>().isPlaying)
            {
                frost.transform.GetChild(0).GetComponent<AudioSource>().Stop();
            }
            coldTime = 0;
            currentColdness += coldnessIncrease * Time.deltaTime;
        }
        else if (currentColdness >= maxColdness)
        {
            //frost.transform.GetChild(0).GetComponent<AudioSource>().Play();
            if (!frost.transform.GetChild(0).GetComponent<AudioSource>().isPlaying)
            {
                frost.transform.GetChild(0).GetComponent<AudioSource>().Play();
            }
            coldTime += Time.deltaTime;
            if (coldTime >= 5f)
            {
                deathByCold = true;
                Die();
                isdead = true;
            }
        }


        int layerMask = LayerMask.GetMask("Bonfire");//default layer mask only contains default, if the bonfire is in this layer, we should make the mask for it.
        //Debug.Log(layerMask);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, bonfireDetectionRadius, layerMask);
        //Debug.Log("Number of colliders detected: " + hitColliders.Length);

        foreach (Collider2D hitCollider in hitColliders)
        {

            if (hitCollider.CompareTag("Bonfire"))
            {
                //Debug.Log("Bonfire in range!!");
                if (currentColdness > 0)
                    currentColdness -= 20f * Time.deltaTime;
                else
                    currentColdness = 0f;
            }
        }
        StartCoroutine(CheckColdness());

        // check if the player is poisoned
        Poisoning();
    }
    private IEnumerator CheckColdness()
    {
        if (currentColdness > 50)
        {

            //Debug.Log("freezed");
            if (!played)
            {
                frost.transform.parent.GetComponent<AudioSource>().Play();
                frostME.Play();
                played = true;
            }
            alphaValue = Mathf.Lerp(frost.color.a, 0.35f, 0.005f);
            frost.color = new Color(1, 1, 1, alphaValue);
            global_light.intensity = Mathf.Lerp(global_light.intensity, 0.5f, 0.005f);
        }
        else if (currentColdness <= 50)
        {
            played = false;
            //Debug.Log("not freezed!");
            alphaValue = Mathf.Lerp(frost.color.a, 0f, 0.1f);
            frost.color = new Color(1, 1, 1, alphaValue);
            global_light.intensity = Mathf.Lerp(global_light.intensity, 0.2f, 0.1f);
        }
        yield return new WaitForSeconds(.1f);
    }

    private void Die()
    {
        //Debug.Log("dead");
        if (frost.transform.GetChild(0).GetComponent<AudioSource>().isPlaying)
        {
            frost.transform.GetChild(0).GetComponent<AudioSource>().Stop();
        }
        GameOverSound.gameObject.GetComponent<AudioSource>().Play();
        //Debug.Log("You died!");
        anim.SetBool("isDead", true);//should be dead
        gameObject.GetComponent<SpriteRenderer>().material = default_material;
        if (transform.GetChild(4).gameObject.activeInHierarchy) { transform.GetChild(4).gameObject.SetActive(false); }//branch
        if (transform.GetChild(1).gameObject.activeInHierarchy) { transform.GetChild(1).gameObject.SetActive(false); }//light
        if (transform.GetChild(2).gameObject.activeInHierarchy) { transform.GetChild(2).gameObject.SetActive(false); }//shadow
        if (transform.GetChild(3).gameObject.activeInHierarchy) { transform.GetChild(3).gameObject.SetActive(false); }//bubble

    }
    public void CloseUI()
    {
        Animator anim = canvas.GetComponent<Animator>();
        anim.SetFloat("done", -1);
        anim.enabled = true;
        //anim.Play("closeUI", -1, 0f);
        StartCoroutine(showDeathPanel());
    }
    private IEnumerator showDeathPanel()
    {
        yield return new WaitForSeconds(1.1f);
        Transform deathPanel = canvas.transform.GetChild(5);
        if (deathByCold)
        {
            deathPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = @"<size=68><color=blue>Opps!</color></size>

The coldness made you crushed.";
        }
        else
        {
            deathPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = @"<size=68><color=red>Ouch!</color></size>

You are broken thanks to the wild enemies.";
        }
        deathPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void takeDamage(int damage)
    {
        if (GetHurtSound.gameObject.activeInHierarchy)
        {
            GetHurtSound.gameObject.GetComponent<AudioSource>().Play();
        }

        if (isInvincible) return; // If the character is already invincible, do not take damage

        currentHealth -= damage;

        FloatingDamage.GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString();
        FloatingDamage.GetComponent<TextMeshProUGUI>().color = new Color32(229, 49, 49, 225);
        Instantiate(FloatingDamage, transform.GetChild(6).GetChild(0));
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(RunFaster());
            StartCoroutine(BecomeInvincible()); // Start the invincibility period coroutine
            StartCoroutine(GettingHurtEffect());
        }

    }

    IEnumerator RunFaster()
    {
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.SetActive(true);//question mark

        damaged = true;
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.transform.GetChild(2).gameObject.SetActive(false);//question mark

        damaged = false;
    }

    IEnumerator BecomeInvincible()
    {
        isInvincible = true;
        spriteRenderer = GetComponent<SpriteRenderer>();

        float invincibilityEndTime = Time.time + invincibilityDuration;

        while (Time.time < invincibilityEndTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    IEnumerator GettingHurtEffect()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            redScreenOverlay.color = redColor;

            // Fade in the red screen overlay
            for (float t = 0; t < blinkDuration / 2; t += Time.deltaTime)
            {
                redScreenOverlay.color = new Color(redScreenOverlay.color.r, redScreenOverlay.color.g, redScreenOverlay.color.b, 65f / 255f * t / (blinkDuration / 2));
                yield return null;
            }

            for (float t = blinkDuration / 2; t > 0; t -= Time.deltaTime)
            {
                redScreenOverlay.color = new Color(redScreenOverlay.color.r, redScreenOverlay.color.g, redScreenOverlay.color.b, 65f / 255f * t / (blinkDuration / 2));
                yield return null;
            }
        }

        redScreenOverlay.color = new Color(redScreenOverlay.color.r, redScreenOverlay.color.g, redScreenOverlay.color.b, 0);
    }

    /*-------------------------*/

    public float poisoningTime = 0f;
    private bool isPoisoning = false;
    public void AddPoisoningTime(float time)
    {
        if (poisoningTime < 10f)
            poisoningTime += time;
    }

    private void Poisoning()
    {
        if (poisoningTime > 0)
        {
            if (!isPoisoning)
            {
                StartCoroutine(PoisoningEffect());
            }
            isPoisoning = true;
        }
    }

    private IEnumerator PoisoningEffect()
    {

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        Color targetlightColor = new Color32(255, 200, 200, 225);
        Color targetColor = new Color32(255, 100, 100, 225);
        IEnumerator coroutine = ChangePoisonedColor(0.25f, spriteRenderer, targetlightColor, targetColor);
        StartCoroutine(coroutine);
        transform.Find("SE_Poisoned").GetComponent<AudioSource>().Play();

        while (poisoningTime > 0)
        {
            int damage = 1;

            // Float damage text
            currentHealth -= damage;
            FloatingDamage.GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString();
            FloatingDamage.GetComponent<TextMeshProUGUI>().color = new Color32(229, 49, 49, 225);
            Instantiate(FloatingDamage, transform.GetChild(6).GetChild(0));

            poisoningTime -= 1f;
            yield return new WaitForSeconds(1f);
        }
        isPoisoning = false;
        StopCoroutine(coroutine);
        spriteRenderer.color = originalColor;
        // Debug.Log("Poisoning effect end");
        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.transform.GetChild(3).gameObject.SetActive(false);
    }

    private IEnumerator ChangePoisonedColor(float colorChangeDuration, SpriteRenderer spriteRenderer, Color originalColor, Color targetColor)
    {
        // poison dialog
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.transform.GetChild(3).gameObject.SetActive(true);
        while (true)
        {
            float t = 0.0f;
            while (t < colorChangeDuration)
            {
                t += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(t / colorChangeDuration);

                // Change the sprite color gradually from the original color to red
                spriteRenderer.color = Color.Lerp(originalColor, targetColor, normalizedTime);

                yield return null;
            }

            // Wait for a short time before changing the sprite color back to the original color
            yield return new WaitForSeconds(0.5f);

            t = 0.0f;
            while (t < colorChangeDuration)
            {
                t += Time.deltaTime;
                float normalizedTime = Mathf.Clamp01(t / colorChangeDuration);

                // Change the sprite color gradually back to the original color
                spriteRenderer.color = Color.Lerp(targetColor, originalColor, normalizedTime);

                yield return null;
            }
        }
    }

    public void FrogSpitDamage(int damage)
    {
        currentHealth -= damage;
/*        if (GetHurtSound.gameObject.activeInHierarchy)
        {
            GetHurtSound.gameObject.GetComponent<AudioSource>().Play();
        }*/
        FloatingDamage.GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString();
        FloatingDamage.GetComponent<TextMeshProUGUI>().color = new Color32(229, 49, 49, 225);
        Instantiate(FloatingDamage, transform.GetChild(6).GetChild(0));
    }


}
