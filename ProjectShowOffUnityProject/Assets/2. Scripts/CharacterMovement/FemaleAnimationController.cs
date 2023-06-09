using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleAnimationController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bool isJumping = Input.GetKey("up");
        bool isSliding = Input.GetKey("down");
        bool isRunningRight = Input.GetKey("right");
        bool isRunningLeft = Input.GetKey("left");
        bool isStumbelingBackwards = Input.GetKey("g");
        
        //JUMPING ANIMATION 
        if (isJumping)
        {
            animator.SetBool("isJumping", true);    
        }
        if (!isJumping)
        {
            animator.SetBool("isJumping", false);
        }
        //SLIDING ANIMATION 
        if (isSliding)
        {
            animator.SetBool("isRunningSlide", true);
        }
        if (!isSliding)
        {
            animator.SetBool("isRunningSlide", false);
        }
        //RunningRight
        if (isRunningRight)
        {
            animator.SetBool("isRunningRight", true);
        }
        if (!isRunningRight)
        {
            animator.SetBool("isRunningRight", false);
        }
        //Runningleft
        if (isRunningLeft)
        {
            animator.SetBool("isRunningLeft", true);
        }
        if (!isRunningLeft)
        {
            animator.SetBool("isRunningLeft", false);
        }
        //Stumbelingbackwards
        if (isStumbelingBackwards)
        {
            animator.SetBool("isStumbelingBackwards", true);
        }

        if (!isStumbelingBackwards)
        {
            animator.SetBool("isStumbelingBackwards", false);
        }

    }
}
