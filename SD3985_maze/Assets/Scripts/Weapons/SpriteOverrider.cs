using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOverrider : MonoBehaviour
{
    //private bool fliped = false;
    //private bool right = true;
    //SpriteRenderer sr;
    Animator anim;
    float horizontalInput;
    float verticalInput;
    Color c1;
    Color c2;

    // Start is called before the first frame update
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        c1 = new Color(1, 1, 1, 0);
        c2 = new Color(1, 1, 1, 1);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<Animator>().GetBool("isWalking")&& transform.parent.GetComponent<Animator>().GetBool("isPowering"))
        {
            gameObject.GetComponent<SpriteRenderer>().color = c1;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = c2;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if ((horizontalInput != 0f || verticalInput != 0f) && !BranchInteraction.isPowering)
        {

            //Control animation flipping
            //right = horizontalInput > 0;
            //if (!fliped && !right)
            //{
            //    sr.flipX = true;
            //    fliped = true;
            //}
            //if (fliped)
            //{
            //    if (right)
            //    {
            //        sr.flipX = false;
            //        fliped = false;
            //    }
            //}
            anim.SetFloat("InputX", horizontalInput);
            anim.SetFloat("InputY", verticalInput);
        }
        else if (BranchInteraction.isPowering)
        {
            //anim.SetFloat("InputX", horizontalInput);
            //anim.SetFloat("InputY", verticalInput);
        }
        else//idle
        {
            //if (transform.parent.GetChild(1).gameObject.activeInHierarchy)//light on
            //{
            //    sr.flipX = false;
            //    fliped = false;
            //}
            //sr.flipX = false;
            //fliped = false;
            anim.PlayInFixedTime("Branch", 0, 0.2f);
        }
    }
  
}
