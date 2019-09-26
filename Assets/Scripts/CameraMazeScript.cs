using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMazeScript : MonoBehaviour
{

    public MazeGenerator_AfterBeatuifyAndPerformanceUpgrade mazeGenScript;
    private Dictionary<int, Cell> cellsOfMaze;
    private int middleOfMaze;
    private List<int> pathStartEnd;
    public Camera mazeCam;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startMazeDiscovery()
    {
        cellsOfMaze = mazeGenScript.cellsOfMaze;
        pathStartEnd = mazeGenScript.pathStartEnd;
        int mazeWidth = mazeGenScript.mazeWidth;
        int mazeHeight = mazeGenScript.mazeHeight;

        Vector3 newPos;
        if (mazeWidth % 2 == 0)
        {
            Cell refCell = cellsOfMaze[mazeWidth / 2];
            Debug.Log("TestCam1: " + refCell.ToString());
            Vector2 middlePointRefCell = refCell.GetMiddlepointOfCellXandZ();
            Debug.Log("TestCam1: " + middlePointRefCell.ToString());
            Cell refCell2 = cellsOfMaze[mazeWidth * mazeHeight - (mazeWidth / 2)];
            Debug.Log("TestCam2: " + refCell2.ToString());
            Vector2 middlePointRefCell2 = refCell2.GetMiddlepointOfCellXandZ();
            Debug.Log("TestCam2: " + middlePointRefCell2.ToString());
            newPos = new Vector3(middlePointRefCell.x - 1.5f, 0, middlePointRefCell.y + (middlePointRefCell2.y - middlePointRefCell.y)/2);
            Debug.Log("Result: " + newPos.ToString());
        }
        else
        {
            middleOfMaze = cellsOfMaze.Count / 2 +1;
            Cell middleCell = cellsOfMaze[middleOfMaze];
            Debug.Log("Camera Test: " + middleCell.ToString());
            Vector2 middlePoint = middleCell.GetMiddlepointOfCellXandZ();
            Debug.Log("Camera Test: " + middlePoint.ToString());
            newPos = new Vector3(middlePoint.x , 0, middlePoint.y);
        }
        mazeCam.transform.position = newPos;
    }
}
