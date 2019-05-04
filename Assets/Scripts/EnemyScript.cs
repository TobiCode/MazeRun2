using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;
    public GameObject player;

    bool running;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        running = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!running)
            {
                running = true;
            }
            else
            {
                running = false;
            }
        }

        //Agent should run towards the player
        navMeshAgent.SetDestination(player.transform.position);

        animator.SetBool("Running", running);
    }
}
