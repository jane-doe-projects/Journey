using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator anim;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Player/raven_basic").GetComponent<Animator>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleAnimation();
    }

    void HandleAnimation()
    {
  
        if (playerController.isJumping)
        {
            SwitchIsInAir(true);
        }
        else
        {
            SwitchIsInAir(false);
        }
        SetVelocity(playerController.moveDir.magnitude);
    }

    public void TriggerJump()
    {
        anim.SetTrigger("doJump");
    }

    public void WalkingStateSwitch(bool isWalking)
    {
        anim.SetBool("walkingOn", isWalking);
    }

    public void SetVelocity(float velocity)
    {
        anim.SetFloat("velocity", velocity);
    }

    public void SwitchIsInAir(bool value)
    {
        anim.SetBool("isInAir", value);
    }
}
