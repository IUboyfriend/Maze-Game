/*//using System;
using UnityEngine;
using System.Collections;

public class RotateLightWithMouse : MonoBehaviour
{
    private Transform playerTransform;
    public int bias = 94;
    private float rotationSpeed = 5f;
    public float rotationSpeed1 = 5f;
    float horizontal;
    float vertical;
    float angle;
    private bool change = true;
    Animator animator;
    Animator branch_animator;
    float x, y = 0.1f;
    bool begin = false;
    private void Start()
    {
        //GameObject.Find("Player").transform.Find("Light_2D_torch").gameObject.SetActive(false);
        animator = GetComponentInParent<Animator>();
        branch_animator = transform.parent.GetChild(4).GetComponent<Animator>();
        playerTransform = transform.parent;
    }
    private void Update()
    {
        if (MazeSuccess.col == 0 && FirstLevelTutorial.completed)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        else
        {
            if (FirstLevelTutorial.faceLeft)
            {
                horizontal = -1;
                vertical = 0;
            }else if (FirstLevelTutorial.faceRight)
            {
                horizontal = 1;
                vertical = 0;
            }
            else
            {
                horizontal = 0;
                vertical = -1;
            }
        }
    }
    public int abs(int a) { return (a ^ (a >> 31)) - (a >> 31); }
    private void FixedUpdate()
    {
        StartCoroutine("FFixedUpdate");
    }
    private IEnumerator FFixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = playerTransform.position.z - Camera.main.transform.position.z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 direction = mouseWorldPos - playerTransform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - bias;
        if (change)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, angle), rotationSpeed * Time.deltaTime);
        }
        float zRotation = transform.eulerAngles.z;
        if (horizontal == 1 && vertical == 0)//go right
        {
            if (begin)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 270), rotationSpeed1 * Time.deltaTime);
            }
            if ((zRotation >= 315 || zRotation <= 225))
            {
                change = false;
                begin = true;
                yield return new WaitForSeconds(1);
            }
            else if (angle > -135 && angle < -45)
            {
                begin = false;
                change = true;
            }
        }

        else if (horizontal == 1 && vertical == 1)//go up right
        {
            if (begin)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 315), rotationSpeed1 * Time.deltaTime);
            }
            if (!(zRotation > 270))
            {
                change = false;
                begin = true;
                yield return new WaitForSeconds(1);
            }
            else if (angle > -90 && angle < 0)
            {
                begin = false;
                change = true;
            }
        }
        else if (horizontal == 0 && vertical == 1)//go up
        {
            if (begin)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), rotationSpeed1 * Time.deltaTime);
            }
            if (zRotation < 315 && zRotation > 45)
            {
                change = false;
                begin = true;
                yield return new WaitForSeconds(1);
            }
            else if (abs((int)angle) < 45)
            {
                begin = false;
                change = true;
            }
        }
        else if (horizontal == -1 && vertical == 1)//go up left
        {
            if (begin)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 45), rotationSpeed1 * Time.deltaTime);
            }
            if (!(zRotation > 270))
            {
                change = false;
                begin = true;
                yield return new WaitForSeconds(1);
            }
            else if (angle > 0 && angle < 90)
            {
                change = true;
                begin = false;
            }
        }
        else if (horizontal == 0 && vertical == -1)//go down
        {
            if (begin)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 180), rotationSpeed1 * Time.deltaTime);
            }
            if (zRotation < 135 || zRotation > 225)
            {
                change = false;
                begin = true;
                yield return new WaitForSeconds(1);
            }
            //else if (abs((int)angle) > 135)
            //else if (abs((int)angle) > 135 || abs((int)angle)<225)
            else if (abs((int)angle) > 135 && abs((int)angle) < 225)
            {
                begin = false;
                change = true;
            }
        }
        else if (horizontal == -1 && vertical == -1)//go down left
        {
            if (begin)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 135), rotationSpeed1 * Time.deltaTime);
            }
            //if (!(zRotation > 90 && zRotation < 180))
            if (zRotation < 90 || zRotation > 180)
            {
                change = false;
                begin = true;
                yield return new WaitForSeconds(1);
            }
            else if (angle > 80 || abs((int)angle) > 180)//mouse
            {
                begin = false;
                change = true;
            }
        }
        else if (horizontal == -1 && vertical == 0)//go left
        {
            if (begin)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 90), rotationSpeed1 * Time.deltaTime);
            }
            if (zRotation < 45 || zRotation > 135)
            {
                change = false;
                begin = true;
                yield return new WaitForSeconds(1);
            }
            else if (angle > 45 && angle < 135)
            {
                begin = false;
                change = true;
            }
        }
        else if (horizontal == 1 && vertical == -1)//go down right
        {
            if (begin)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 225), rotationSpeed1 * Time.deltaTime);
            }
            if (!(zRotation > 180 && zRotation < 270))
            {
                change = false;
                begin = true;
                yield return new WaitForSeconds(1);
            }
            else if (angle > -180 && angle < -90)
            {
                begin = false;
                change = true;
            }
        }
        else if (horizontal == 0 && vertical == 0)//idle
        {
            change = true;
            x = (angle / -90);
            float abs = x < 0 ? -x : x;
            if (abs > 1) { x = (x / abs) * (2 - abs); }
            abs = angle < 0 ? -angle : angle;
            int zhengfu = abs > 90 ? -1 : 1;
            y = (1 - x) * zhengfu;
            //Debug.Log(x + " " + y);
            animator.SetFloat("Vertical", y);
            animator.SetFloat("Horizontal", x);
            if (branch_animator.isActiveAndEnabled)
            {
                branch_animator.SetFloat("InputY", y);
                branch_animator.SetFloat("InputX", x);
            }
        }

    }

}
*/





using UnityEngine;

public class RotateLightWithMouse : MonoBehaviour
{
    private Transform playerTransform;
    public int bias = 94;
    private float rotationSpeed = 5f;

    private void Start()
    {
        playerTransform = transform.parent;
    }

    private void Update()
    {
        RotateTorchWithMouse();
    }

    private void RotateTorchWithMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = playerTransform.position.z - Camera.main.transform.position.z;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3 direction = mouseWorldPos - playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - bias;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
