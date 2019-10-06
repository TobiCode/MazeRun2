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
    private float overTime;
    private bool isCameraMoved;


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
            Vector2 middlePointRefCell = refCell.GetMiddlepointOfCellXandZ();
            Cell refCell2 = cellsOfMaze[mazeWidth * mazeHeight - (mazeWidth / 2)];
            Vector2 middlePointRefCell2 = refCell2.GetMiddlepointOfCellXandZ();
            newPos = new Vector3(middlePointRefCell.x - 1.5f, (mazeWidth * mazeHeight) / 2, middlePointRefCell.y + (middlePointRefCell2.y - middlePointRefCell.y) / 2);
        }
        else
        {
            middleOfMaze = cellsOfMaze.Count / 2 + 1;
            Cell middleCell = cellsOfMaze[middleOfMaze];
            Debug.Log("Camera Test: " + middleCell.ToString());
            Vector2 middlePoint = middleCell.GetMiddlepointOfCellXandZ();
            Debug.Log("Camera Test: " + middlePoint.ToString());
            newPos = new Vector3(middlePoint.x, (mazeWidth * mazeHeight) / 2, middlePoint.y);
        }
        //height according to mazeWidth --> to keep whole maze in view
        float height = 2.7f * mazeWidth + 1.0f;
        newPos.y = height;
        //mazeCam should start at the middle of the maze
        float startingX = newPos.x;
        mazeCam.transform.position = new Vector3(startingX, 2, -14);
        //time accroding to size of maze
        overTime = Mathf.Log(mazeWidth, 1.35f);
        Debug.Log("Debug OverTime: " + overTime);

        StartCoroutine(MoveCamera(transform.position, newPos, overTime));
        Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
        StartCoroutine(RotateCamera(transform.rotation, targetRotation, overTime / 2));
        Debug.Log("DebugCoroutines, MoveObjectFinished");
        //
    }

    IEnumerator MoveCamera(Vector3 source, Vector3 target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
            yield return null;
        }
        transform.position = target;
        isCameraMoved = true;
    }

    IEnumerator RotateCamera(Quaternion source, Quaternion target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            if (isCameraMoved)
            {
                transform.rotation = Quaternion.Lerp(source, target, (Time.time - startTime) / overTime);
                yield return null;
            }
            else
            {
                startTime = Time.time;
                yield return null;
            }
        }
        transform.rotation = target;
    }
}
