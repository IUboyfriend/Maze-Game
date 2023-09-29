using UnityEngine;
using TMPro;
using System;


public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public EnemyHealthBar healthBar;
    public GameObject FloatingDamage;
    public GameObject meatPrefab;
    private Animator animator;
    private bool isdead;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
        isdead = false;
    }

    void Update()
    {
      
        if (currentHealth <= 0 && !isdead)
        {
            Instantiate(meatPrefab, transform.position, Quaternion.identity);
            GetComponent<AudioSource>().Play();
            animator.SetBool("isDead", true);
            isdead = true;
        }
    }
    public void DestroyBoar()
    {

        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(Math.Max(currentHealth, 0));
        //Float the damage
        FloatingDamage.GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString();
        FloatingDamage.GetComponent<TextMeshProUGUI>().color = new Color32(212, 82, 0,255);
        Instantiate(FloatingDamage, transform.Find("EnemyCanvas").Find("HealthBar"));
    }
}
