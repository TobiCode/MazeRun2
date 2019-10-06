using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour
{

    private int widthOfMaze;
    private int heightOfMaze;
    public GameObject player;
    private GameObject enemy;


    public MazeGenerator_AfterBeatuifyAndPerformanceUpgrade mazeGenScript;
    public CameraMazeScript cameraMazeScript;
    // Start is called before the first frame update
    void Start()
    {
        enemy = mazeGenScript.enemy;
        //disable player and enemy
        player.SetActive(false);
        enemy.SetActive(false);
        //Set width and height and generate Maze
        mazeGenScript.mazeHeight = 10;
        mazeGenScript.mazeWidth = 10;
        mazeGenScript.GenerateMazeAndSetPlayerEnemyToEntry();
        //Start Maze discovery
        cameraMazeScript.startMazeDiscovery();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
