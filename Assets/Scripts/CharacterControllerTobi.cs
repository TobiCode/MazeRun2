using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Strg+K+D for formatting Code

public class CharacterControllerTobi : MonoBehaviour
{
    private string MoveInputAxis = "Vertical";
    private string TurnInputAxis = "Horizontal";
    private Animator animator;

    // rotation that occurs in angles per second holding down input
    public float rotationRate = 180;

    // units moved per second holding down move input
    public float runSpeed = 0.05f;
    public float walkSpeed = 0.025f;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        float moveAxis = Input.GetAxis(MoveInputAxis);
        float turnAxis = Input.GetAxis(TurnInputAxis);

        ApplyInput(moveAxis, turnAxis);
    }

    private void ApplyInput(float moveInput,
                            float turnInput)
    {
        Move(moveInput);
        Turn(turnInput);
    }

    private void Move(float input)
    {
        if (input == 0)
        {
            animator.SetFloat("speed", 0);
        }
        else
        {
            //Right now only Running
            transform.Translate(Vector3.forward * input * runSpeed);
            animator.SetFloat("speed", 2);
        }
    }

    private void Turn(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    }
}
