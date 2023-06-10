using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleAnimationController : MonoBehaviour
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
        bool isJumping = Input.GetKey("w");
        bool isSliding = Input.GetKey("s");
        bool isRunningRight = Input.GetKey("d");
        bool isRunningLeft = Input.GetKey("a");
        bool isStumbelingBackwards;


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
            Debug.Log(isSliding);
        }
        if (!isSliding)
        {
            animator.SetBool("isRunningSlide", false);
            Debug.Log(isSliding);
        }
        //RunningRight
        if (isRunningRight)
        {
            animator.SetBool("isRunningRight", true);
            Debug.Log(isRunningRight);
        }
        if (!isRunningRight)
        {
            animator.SetBool("isRunningRight", false);
            Debug.Log(isRunningRight);
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
         void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("YourColliderTag"))
            {
                isStumbelingBackwards = true;
            }
        }

         void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("YourColliderTag"))
            {
                isStumbelingBackwards = false;
            }
        }

    }
}
