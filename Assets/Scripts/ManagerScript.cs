using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour
{

    public int widthOfMaze;
    public int heightOfMaze;
    public GameObject player;
    private GameObject enemy;
    public GameObject mazeCam;
    public GameObject PathLights;
    private bool play;


    public MazeGenerator_AfterBeatuifyAndPerformanceUpgrade mazeGenScript;
    public CameraMazeScript cameraMazeScript;
    // Start is called before the first frame update
    void Start()
    {
        //disable player and enemy
        player.SetActive(false);
        //Set width and height and generate Maze
        mazeGenScript.mazeHeight = widthOfMaze;
        mazeGenScript.mazeWidth = heightOfMaze;
        mazeGenScript.GenerateMazeAndSetPlayerEnemyToEntry();
        enemy = mazeGenScript.enemyClone;
        enemy.SetActive(false);
        //Start Maze discovery
        cameraMazeScript.startMazeDiscovery();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraMazeScript.isCameraAtPlayer && !play)
        {
            PathLights.SetActive(false);
            mazeCam.SetActive(false);
            play = true;
            player.SetActive(true);
            enemy.SetActive(true);
        }
        
    }
}
