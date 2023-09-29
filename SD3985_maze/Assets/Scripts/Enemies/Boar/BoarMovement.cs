using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.AI;

public class BoarMovement : MonoBehaviour
{
    private GameObject target; // The name of the game object we want to move towards
    public float speed = 5f; // The speed at which we want to move
    public float maxDistance = 10f; // The max distance we want to be from the target
    public bool isMoving = false;
    public bool isRushing = false;
    public bool isPunching = false;
    private Vector2 rushPoint;
    private float waitTime = 0f;
    private Animator animator;
    private GameObject particle;
    private AudioSource boarsound;
    private Vector2 enemyToPlayer;

    private CharacterStatus characterHealth;

    UnityEngine.AI.NavMeshAgent agent;

    private Vector3 initialPosition;

    void Start()
    {
        target = GameObject.Find("Player").gameObject;
        animator = GetComponent<Animator>();
        characterHealth = target.GetComponent<CharacterStatus>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        initialPosition = transform.position;
        particle = transform.Find("boar_particle").gameObject;
        boarsound = transform.GetChild(1).GetComponent<AudioSource>();
        Debug.Log("particle: " + particle);
    }
    void Update()
    {
        
        if(waitTime > 0){
            waitTime -= Time.deltaTime;
            return;
        }

        enemyToPlayer = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y+0.1f);
        if(isRushing){
            boarsound.pitch = 1f;
            animator.SetBool("isRushing", true);
            particle.SetActive(true);
            rushToPlayer();
            Debug.Log("rush to player");
        }
        else if(isMoving){
            boarsound.pitch = 0.6f;
            animator.SetBool("isMoving", true);
            moveFollowsPlayer();
            if(enemyToPlayer.magnitude > 8f)
            {
                Debug.Log("stop moving");
                animator.SetBool("isMoving", false);
                isMoving = false;
                isRushing = false;
                speed = 5f;
                rushPoint = new Vector2(0,0);
                StartCoroutine(backToInitialPosition());
            }
        }
        // Interrupt return process
        if(isMoving || isRushing || isPunching){
            agent.ResetPath();
        }else{
            if(agent.velocity.magnitude > 0.1f)
            {
                animator.SetFloat("Look X", agent.velocity.x);
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        
    }
    // player in sight
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == target.name && Vector3.Distance(initialPosition, target.transform.position) < maxDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyToPlayer); // Cast a ray from our game object to the right
            if(hit.collider != null && hit.collider.gameObject.name == target.name){
                animator.SetFloat("Look X", enemyToPlayer.x);
                if(rushPoint == new Vector2(0,0) && isMoving == false && isRushing == false)
                {
                    isRushing = true;
                    isMoving = false;
                    speed = 10f;
                    rushPoint = new Vector2(target.transform.position.x, target.transform.position.y);
                    waitTime = 0.5f;
                }
            }
        }
    }

    void rushToPlayer(){
        animator.SetBool("isMoving", false);
        if ((new Vector2(rushPoint.x - transform.position.x, rushPoint.y - transform.position.y)).magnitude > 1f && enemyToPlayer.magnitude > 1f){
            transform.position = Vector3.MoveTowards(transform.position, rushPoint, speed * Time.deltaTime); // Move towards it
        }else{
            isRushing = false;
            isMoving = true;
            speed = 3f;
            waitTime = 1.5f;
            animator.SetBool("isRushing", false);
            animator.SetBool("isMoving", true);
            particle.SetActive(false);
            if (enemyToPlayer.magnitude < 1f)
                characterHealth.takeDamage(10);
        }
    }

    void moveFollowsPlayer(){
        // if (enemyToPlayer.magnitude > 1f)
        // {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyToPlayer); // Cast a ray from our game object to the right
            if (hit.collider != null && hit.collider.gameObject.name == target.name && Vector3.Distance(initialPosition, target.transform.position) < maxDistance)
            { 
                // Renderer targetRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                if(enemyToPlayer.magnitude > 1f)
                    transform.position = Vector3.MoveTowards(transform.position, hit.collider.gameObject.transform.position, speed * Time.deltaTime); // Move towards it
                Debug.Log("enemyToPlayer.magnitude: " + enemyToPlayer.magnitude);
                if(enemyToPlayer.magnitude < 3f && !isPunching){
                    isPunching = true;
                    animator.SetBool("isPunching", true);
                    StartCoroutine(punchingAttack(0.75f));
                }
                if(enemyToPlayer.magnitude > 3f){
                    isPunching = false;
                    animator.SetBool("isPunching", false);
                }
            }else{
                animator.SetBool("isMoving", false);
                animator.SetBool("isPunching", false);
                isMoving = false;
                isPunching = false;
                rushPoint = new Vector2(0,0);
                StartCoroutine(backToInitialPosition());
            }
        // }
    }
    private IEnumerator punchingAttack(float time){
        while(isPunching){
            characterHealth.takeDamage(3);
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator backToInitialPosition(float time = 3f){
        yield return new WaitForSeconds(time);
        agent.SetDestination(initialPosition);
    }
    
}
