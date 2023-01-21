using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isGrounded;
    [SerializeField] private LayerMask playerMask;

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("is checking");
        isGrounded = other != null && (other.gameObject.layer != playerMask);
    }

    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
    }

}
