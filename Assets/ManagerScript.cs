using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour
{

    public MazeGenerator_AfterBeatuifyAndPerformanceUpgrade mazeGenScript;
    // Start is called before the first frame update
    void Start()
    {
        //Set width and height
        mazeGenScript.mazeHeight = 5;
        mazeGenScript.mazeWidth = 5;
        mazeGenScript.GenerateMazeAndSetPlayerEnemyToEntry();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
