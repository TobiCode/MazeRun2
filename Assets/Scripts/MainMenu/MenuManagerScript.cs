using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManagerScript : MonoBehaviour
{

    public Text levelNumber;
    public int currentSelectedLevelNumber;
    public float logTime;
    public Text logBox;
    public int maxLevel;
    public int minLevel;

    public void OnPlayClick()
    {
        //Set level
        GameManagerScript.widthOfMaze = 6 + (currentSelectedLevelNumber-1) *2;
        GameManagerScript.heightOfMaze = 6 + (currentSelectedLevelNumber - 1) * 2;
        //Load Scene
        SceneManager.LoadScene(1);
    }

    public void onPlusClick()
    {
        if (currentSelectedLevelNumber < LevelProgressManager.levelProgress && currentSelectedLevelNumber < maxLevel)
        {
            currentSelectedLevelNumber += 1;
            levelNumber.text = "" + currentSelectedLevelNumber;
        }
        else if (currentSelectedLevelNumber == maxLevel)
        {
            StartCoroutine(showLogMessage("Max Level selected", logTime));
        }
        else
        {
            StartCoroutine(showLogMessage("Level not unlocked yet", logTime));
        }

    }

    public void onMinusClick()
    {
        if (currentSelectedLevelNumber > minLevel)
        {
            currentSelectedLevelNumber -= 1;
            levelNumber.text = "" + currentSelectedLevelNumber;
        }
        else
        {
            StartCoroutine(showLogMessage("Min Level selected", logTime));
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        LevelProgressManager.LoadProgress();
        levelNumber.text = "" + LevelProgressManager.levelProgress;
        currentSelectedLevelNumber = LevelProgressManager.levelProgress;
        logBox.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator showLogMessage(string text, float time)
    {
        logBox.text = text;
        logBox.enabled = true;
        yield return new WaitForSeconds(time);
        logBox.enabled = false;
    }
}
