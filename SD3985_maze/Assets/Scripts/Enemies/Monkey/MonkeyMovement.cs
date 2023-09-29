using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonkeyMovement : MonoBehaviour
{
    public float maxTravelDistance = 10f;
    public float firstAttackDistance = 8f;
    public GameObject branch;

    private GameObject player;
    private CharacterStatus characterStatus;
    private Animator monkey_anim;
    private NavMeshAgent agent;
    private Vector3 initialPosition;

    private bool attack = true;
    private bool armed = true;
    private Coroutine myCoroutine;
    private AudioSource monkeysound;

    private int attackState = 0;// 0: idle, 1: run, 2: mad, 3: back to initial position

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        characterStatus = player.GetComponent<CharacterStatus>();
        monkey_anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        monkeysound = transform.GetChild(1).GetComponent<AudioSource>();
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float playerD = Vector3.Distance(player.transform.position, initialPosition); // player to initial position
        float monkeyD = Vector3.Distance(transform.position, initialPosition);  // monkey to initial position
        float distance = Vector3.Distance(transform.position, player.transform.position); // monkey to player
        
        if(distance < firstAttackDistance && (attackState == 0 || attackState == 3) && playerD < maxTravelDistance){ // first attack: monkey is idle or run back, and player is close
            attackState = 1;
            monkey_anim.SetBool("isRunning", true);
            monkeysound.Play();
            agent.Resume();
            if(armed){
                myCoroutine = StartCoroutine(MadStateToggle());
            }
        }else if(playerD > maxTravelDistance){
            
            if(monkeyD < 3f){
                attackState = 0;
                monkey_anim.SetBool("isRunning", false);
            }
            else if(attackState == 1 || attackState == 2){
                attackState = 0;
                agent.Stop();
                monkeysound.Stop();
                monkey_anim.SetBool("isRunning", false);
                // stop toggle coroutine and reset parameters
                StopCoroutine(myCoroutine);
                monkey_anim.SetBool("isMad", false);
                agent.speed = 3.5f;
                Invoke("SetState", 2f); // set state to 3 after a while
            }
        }

        // Debug.Log("attackState: " + attackState);
        switch(attackState){
            case 1: // run
                agent.SetDestination(player.transform.position);
                if(distance < 3f && attack){
                    StartCoroutine(Attack());
                }
                break;
            case 2: // mad
                agent.SetDestination(player.transform.position);
                if(distance < 4f && attack){
                    attackState = 2;
                    StartCoroutine(Attack());
                }
                break;
            case 3: // back to initial position
                agent.Resume();
                agent.SetDestination(initialPosition);
                break;
            otherwise:
                break;
        }

        
        if(agent.velocity.x != 0)
            monkey_anim.SetFloat("Look X", agent.velocity.x);

        
    }

    private IEnumerator Attack()
    {
        int damage;
        if(armed) damage = 2;
        else damage = 1;
        attack = false;

        float interval;
        if(attackState == 2)interval = 0.3f;
        else interval = 0.5f;
        characterStatus.takeDamage(damage);
        yield return new WaitForSeconds(interval);
        
        attack = true;
    }

    private IEnumerator MadStateToggle(){
        while(true)
        {
            attackState = 1;
            monkeysound.pitch = -0.28f;

            monkey_anim.SetBool("isMad", false);
            agent.speed = 3.5f;
            yield return new WaitForSeconds(3f);

            attackState = 2;
            monkey_anim.SetBool("isMad", true);
            monkeysound.pitch = 0.5f;
            agent.speed = 8f;
            yield return new WaitForSeconds(5f);
        }
    }

    private void SetState()
    {
        attackState = 3;
        monkey_anim.SetBool("isRunning", true);
    }

    public void Expelliarmus(){
        if(attackState != 2){ // if monkey is not mad, then disarm it
            Debug.Log("Expelliarmus");
            armed = false;
            Instantiate(branch, transform.position, Quaternion.identity);
            monkey_anim.SetBool("isWithBranch", false);
            StopCoroutine(myCoroutine);
        }
    }
}
