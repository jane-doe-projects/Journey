using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform camTransform;

    private void Start()
    {
        camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        //transform.LookAt(transform.position + camTransform.forward);
        transform.LookAt(transform.position + camTransform.forward);
        
    }
}
