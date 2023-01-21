using UnityEngine;

public class CrosshairFocusRotationControl : MonoBehaviour
{
    void LateUpdate()
    {
        // rotate crosshair around player when camera rotates around player/crosshair position
        Quaternion yRotation = Camera.main.transform.rotation;
        yRotation.x = 0;
        yRotation.z = 0;
        float mouseRotationSpeed = 450;
        transform.rotation = Quaternion.Lerp(transform.rotation, yRotation, Time.deltaTime * mouseRotationSpeed);
    }
}
