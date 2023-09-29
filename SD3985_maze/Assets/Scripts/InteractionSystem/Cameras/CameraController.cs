using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 2f;
    public Vector3 offset;
    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = desiredPosition;
        //transform.LookAt(target);
    }

    public static void ChangeVol(float newValue)
    {
        AudioListener.volume = newValue;
    }
}





