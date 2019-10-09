using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;
    private GameObject player;
    public float AttackDistance = 10;
    AudioSource myAudioSource;
    public AudioClip stabbing;
    public AudioClip crawling;
    private bool stabbed;


    bool running;
    bool attacking;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player_Game");
        animator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        running = false;
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = transform.rotation * Quaternion.Euler(0, 90,0);
        if (navMeshAgent.speed > 0.75)
        {
            running = true;
            if (!stabbed)
            {
                myAudioSource.clip = crawling;
                Audio();
            }
        }

        else
        {
            running = false;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < AttackDistance)
        {
            attacking = true;
            //Play stabbing sound
            if (!stabbed)
            {
                myAudioSource.clip = stabbing;
                Audio();
                stabbed = true;
            }
            player.GetComponent<CharacterControllerTobi>().Live -= 1; 
        }
        else
        {
            attacking = false;
        }

        animator.SetBool("running", running);
        animator.SetBool("attacking", attacking);

        //Agent should run towards the player
        navMeshAgent.SetDestination(player.transform.position);
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
