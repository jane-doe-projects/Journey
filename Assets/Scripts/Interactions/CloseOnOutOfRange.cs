using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOnOutOfRange : MonoBehaviour
{
    [SerializeField] GameObject closingObj;
    [SerializeField] GameObject targetObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            closingObj.SetActive(false);
        }
    }
}
