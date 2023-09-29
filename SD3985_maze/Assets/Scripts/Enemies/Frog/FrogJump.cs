using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrogJump : MonoBehaviour
{
    public float maxTravelDistance = 10f;

    private Animator frog_anim;
    private GameObject player;
    private bool jump = true;
    private bool nearPlayer = false;
    private bool spit = true;
    private Vector3 initialPosition;
    private int attackState = 0;// 0: idle, 1: jump, 2: spit, 3: back to initial position

    public GameObject bullet;

    UnityEngine.AI.NavMeshAgent agent;

    private void Start()
    {
        frog_anim = GetComponent<Animator>();
        // frog_anim.SetBool("Jump", true);

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        player = GameObject.Find("Player").gameObject;
        initialPosition = transform.position;
    }
    public void SetJumpParaToFalse()
    {
        // Debug.Log("HERE");
        frog_anim.SetBool("Jump", false);
    }

    private void Update()
    {
        float playerD = Vector3.Distance(player.transform.position, initialPosition);
        float frogD = Vector3.Distance(transform.position, initialPosition);
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if(playerD > maxTravelDistance){
            if(frogD < 3f)
                attackState = 0;
            else if(attackState == 1 || attackState == 2){
                attackState = 0;
                Invoke("SetState", 1f);
            }
                
        }else if(distance < 6f){
             attackState = 2;
        }else if(distance > 9f){
            attackState = 1;
        }
        
        switch(attackState){
            case 0: //idle
                break;
            case 1: //jump
                if(jump){
                    StartCoroutine(Jump(player.transform.position.x, player.transform.position.y));
                }
                break;
            case 2: //spit
                if(spit){
                    StartCoroutine(Spit());
                }
                break;
            case 3: //back to initial position
                if(jump){
                    StartCoroutine(Jump(initialPosition.x, initialPosition.y));
                }
                break;
        }
    }


    private void FixedUpdate()
    {
        //Debug.Log(frog_anim.GetBool("Jump"));
    }

    private IEnumerator Jump(float x, float y)
    {
        jump = false;
        frog_anim.SetBool("Jump", true);
        agent.Resume();
        agent.SetDestination(new Vector3(x, y, transform.position.z));
        yield return new WaitForSeconds(0.05f);
        // Debug.Log("look x: " + agent.velocity.x);
        if(agent.velocity.x != 0)
        {
            frog_anim.SetFloat("Look X", agent.velocity.x);
        }
        yield return new WaitForSeconds(0.2f);
        agent.Stop();
        // Debug.Log("reset path");
        yield return new WaitForSeconds(0.5f);
        jump = true;
    }

    private IEnumerator Spit()
    {
        spit = false;
        if(agent.velocity.x == 0)
        {
            frog_anim.SetFloat("Look X", player.transform.position.x - transform.position.x);
        }
        yield return new WaitForSeconds(0.5f);
        Instantiate(bullet, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        spit = true;
    }

    private void SetState()
    {
        attackState = 3;
    }
}
