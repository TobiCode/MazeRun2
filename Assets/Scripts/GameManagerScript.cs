using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    public static int widthOfMaze=6;
    public static int heightOfMaze=6;
    public GameObject player;
    private GameObject enemy;
    public GameObject mazeCam;
    public GameObject PathLights;
    public Text infoText;
    private bool play;

    public MazeGenerator_AfterBeatuifyAndPerformanceUpgrade mazeGenScript;
    public CameraMazeScript cameraMazeScript;
    // Start is called before the first frame update
    void Start()
    {
        infoText.enabled = false;
        //disable player and enemy
        player.SetActive(false);
        //Set width and height and generate Maze
        Debug.Log("TestSceneMgmt: " + widthOfMaze);
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
            StartCoroutine(showInfoText());
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator showInfoText()
    {
        infoText.enabled = true;
        yield return new WaitForSeconds(3);
        infoText.enabled = false;
    }
}
