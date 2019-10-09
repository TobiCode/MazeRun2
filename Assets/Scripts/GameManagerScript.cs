using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    public static int widthOfMaze = 6;
    public static int heightOfMaze = 6;
    public GameObject player;
    private CharacterControllerTobi playerScript;
    private GameObject enemy;
    public GameObject mazeCam;
    public GameObject mainCam;
    public Vector3 cameraPosLookingBack;
    public Vector3 cameraRotLookingBack;
    public GameObject PathLights;
    public GameObject gameUIObject;
    public GameObject gameOverUIObject;
    public GameObject gameFinishUIObject;
    public Image darkenImage;
    public Image scratch1;
    public Image scratch2;
    public Text infoText;
    private bool play;
    private bool scratchesShown;

    public MazeGenerator_AfterBeatuifyAndPerformanceUpgrade mazeGenScript;
    public CameraMazeScript cameraMazeScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<CharacterControllerTobi>();
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
        if (playerScript.Live < 1)
        {
            playerScript.enabled = false;
            mainCam.transform.localPosition = cameraPosLookingBack;
            mainCam.transform.localRotation = Quaternion.Euler(cameraRotLookingBack);
            //show Scratches
            StartCoroutine(ShowScratches());
            if (scratchesShown)
            {
                //Stop Game and show Game Over Canvas
                gameOverUIObject.SetActive(true);
                gameUIObject.SetActive(false);
                StartCoroutine(FadeImage(false, darkenImage, 240f, 0.5f));
            }
        }
        else if(playerScript.Live >= 2)
        {
            playerScript.enabled = false;
            enemy.SetActive(false);
            gameFinishUIObject.SetActive(true);
            gameUIObject.SetActive(false);
        }
    }

    public void BackToMenuNextLevel()
    {
        LevelProgressManager.SaveLevelProgress(LevelProgressManager.levelProgress + 1);
        SceneManager.LoadScene(0);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator showInfoText()
    {
        infoText.enabled = true;
        yield return new WaitForSeconds(3);
        infoText.enabled = false;
    }

    IEnumerator FadeImage(bool fadeAway, Image img, float alpha, float overTime)
    {
        float startTime = Time.time;
        // fade from opaque to transparent
        if (fadeAway)
        {
            float beginningAlpha = img.color.a;
            // loop over 1 second backwards
            while (Time.time < startTime + overTime)
            {
                float fraction = (Time.time - startTime) / overTime * beginningAlpha;
                img.color = new Color(0, 0, 0, beginningAlpha - fraction);
                yield return null;
            }
            scratchesShown = true;
        }
        // fade from transparent to opaque
        else
        {
            while (Time.time < startTime + overTime)
            {
                float fraction = (Time.time - startTime) / overTime * alpha;
                img.color = new Color(0, 0, 0, fraction / 255f);
                yield return null;
            }
        }
    }

    IEnumerator ShowScratches()
    {
        //FadeIN
        yield return  StartCoroutine(FadeImage(false, scratch1, 255f, 0.1f));
        yield return  StartCoroutine(FadeImage(false, scratch2, 255f, 0.1f));
        //FadeOUT
        StartCoroutine(FadeImage(true, scratch1, 0f, 0.2f));
        StartCoroutine(FadeImage(true, scratch2, 0f, 0.2f));
    }
}
