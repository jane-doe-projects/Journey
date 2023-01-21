using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Cinemachine;

public class FreeLookRecenterOnMouseRelease : MonoBehaviour
{
    CinemachineFreeLook thirdPersonCam;
    public float waitTime = 0f;
    public float recenterSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        thirdPersonCam = GameObject.Find("TPC").GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (EventSystem.current.IsPointerOverGameObject())
            return;*/

        if (Input.GetMouseButtonUp(0))
        {
            TriggerRecentering();
        }

        if (Input.GetMouseButtonDown(0))
        {
            StopRecentering();
        }
    }

    void StopRecentering()
    {
        thirdPersonCam.m_RecenterToTargetHeading.m_enabled = false;
    }

    void TriggerRecentering()
    {
        thirdPersonCam.m_RecenterToTargetHeading.m_WaitTime = waitTime;
        thirdPersonCam.m_RecenterToTargetHeading.m_RecenteringTime = recenterSpeed;
        thirdPersonCam.m_RecenterToTargetHeading.m_enabled = true;
    }
}
