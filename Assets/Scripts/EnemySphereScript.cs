using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemySphereScript : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    public GameObject player;
    private Transform transformSphere;
    public float heightRangeHovering;
    public float heightChangePerFrame;

    private Vector3 initTransform;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        initTransform = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.SetDestination(player.transform.position);

        if(gameObject.transform.position.y < initTransform.y + heightRangeHovering)
        {
            navMeshAgent.Move(new Vector3(0, heightChangePerFrame, 0));
            //gameObject.transform.position += new Vector3(0, heightChangePerFrame, 0);
        }

        else
        {
            navMeshAgent.Move(new Vector3(0, -heightChangePerFrame, 0));
            //gameObject.transform.position -= new Vector3(0,  heightChangePerFrame, 0);
        }

    }
}
