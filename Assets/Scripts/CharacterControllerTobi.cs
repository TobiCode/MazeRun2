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

    // Init camera pos and rotation
    public Camera cameraFollowingPlayer;
    private Vector3 cameraInitPos;
    private Quaternion cameraInitRot;
    public Vector3 cameraPosLookingBack;
    public Vector3 cameraRotLookingBack;

    AudioSource myAudioSource;
    public AudioClip foot4; //Running
    public AudioClip foot3; //Start/Stop Running

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        cameraInitPos = cameraFollowingPlayer.transform.localPosition;
        cameraInitRot = cameraFollowingPlayer.transform.localRotation;
        myAudioSource = GetComponent<AudioSource>();

    }


    // Update is called once per frame
    private void Update()
    {
        float moveAxis = Input.GetAxis(MoveInputAxis);
        float turnAxis = Input.GetAxis(TurnInputAxis);

        ApplyInput(moveAxis, turnAxis);

        //Look back
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cameraFollowingPlayer.transform.localPosition = cameraPosLookingBack;
            cameraFollowingPlayer.transform.localRotation = Quaternion.Euler(cameraRotLookingBack);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            cameraFollowingPlayer.transform.localPosition = cameraInitPos;
            cameraFollowingPlayer.transform.localRotation = cameraInitRot;
        }


    }

    private void ApplyInput(float moveInput, float turnInput)
    {
        Move(moveInput);
        Turn(turnInput);
        AudioHandling(moveInput);
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

    private void AudioHandling(float input)
    {
        //Audio
        if (input > 0.8f)
        {
            myAudioSource.clip = foot4;
            Audio();
        }
        else if (input > 0f)
        {
            myAudioSource.clip = foot3;
            Audio();
        }
    }

    private void Turn(float input)
    {
        transform.Rotate(0, input * rotationRate * Time.deltaTime, 0);
    }

    private void Audio()
    {
        if (myAudioSource.isPlaying == false)
        {
            //Audio delay between each 
            myAudioSource.volume = Random.Range(0.8f, 1);
            myAudioSource.pitch = Random.Range(0.8f, 1);
            myAudioSource.Play();
        }
    }
}
