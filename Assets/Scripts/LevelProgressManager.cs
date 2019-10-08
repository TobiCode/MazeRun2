using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressManager
{
    public static int levelProgress;
    // Start is called before the first frame update

    public static void SaveLevelProgress(int progress)
    {
        if (levelProgress < progress)
        {
            PlayerPrefs.SetInt("level_Progress", progress);
            PlayerPrefs.Save();
        }
    }

    public static int LoadProgress()
    {
        int progress = PlayerPrefs.GetInt("level_Progress", 1);
        if (progress > levelProgress)
        {
            levelProgress = progress;
        }
        return progress;
    }
}
