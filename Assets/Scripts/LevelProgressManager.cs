using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    public static int levelProgress;
    // Start is called before the first frame update
    void Start()
    {
        levelProgress = LoadProgress();
    }

    // Update is called once per frame
    void Update()
    {

    }

    static void SaveLevelProgress(int progress)
    {
        if (levelProgress < progress)
        {
            PlayerPrefs.SetInt("level_Progress", progress);
            PlayerPrefs.Save();
        }
    }

    static int LoadProgress()
    {
        int progress = PlayerPrefs.GetInt("level_Progress", 1);
        if (progress > levelProgress)
        {
            levelProgress = progress;
        }
        return progress;
    }
}
