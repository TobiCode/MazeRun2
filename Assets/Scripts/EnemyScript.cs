using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Animator animator;
    bool running;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
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
        
        animator.SetBool("Running", running);
    }
}
