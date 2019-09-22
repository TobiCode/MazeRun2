using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;
    private GameObject player;

    bool running;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player_Game");
        animator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = transform.rotation * Quaternion.Euler(0, 90,0);
        if (navMeshAgent.speed > 0.75)
        {
            running = true;
        }

        //Agent should run towards the player
        navMeshAgent.SetDestination(player.transform.position);

        animator.SetBool("running", running);
    }
}
