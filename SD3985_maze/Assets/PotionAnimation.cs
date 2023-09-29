using UnityEngine;

public class PotionAnimation : MonoBehaviour
{
    public void setAnimatorFalse()
    {
        GetComponent<Animator>().enabled = false;
    }
}
