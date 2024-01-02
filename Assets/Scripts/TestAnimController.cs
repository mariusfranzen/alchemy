using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimController : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.SetBool("IsHovering", true);
            animator.SetTrigger("HoverStart");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("DisappearTrigger");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("IsHovering", false);
            animator.SetTrigger("HoverEnd");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

        }
    }
}
