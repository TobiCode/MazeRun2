using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMazeScript : MonoBehaviour
{

    public MazeGenerator_AfterBeatuifyAndPerformanceUpgrade mazeGenScript;
    public GameObject spotLight;
    public GameObject spotLights;
    public Camera cameraOfPlayer;
    private Dictionary<int, Cell> cellsOfMaze;
    private int middleOfMaze;
    private List<int> pathStartEnd;
    public Camera mazeCam;
    public float timeBetweenPathLight;
    private float overTime;
    private bool isCameraMoved;
    private bool isCameraRotated;
    private bool isPathShown;
    public bool isCameraAtPlayer;

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
            Vector2 middlePoint = middleCell.GetMiddlepointOfCellXandZ();
            newPos = new Vector3(middlePoint.x, (mazeWidth * mazeHeight) / 2, middlePoint.y);
        }
        //height according to mazeWidth --> to keep whole maze in view
        float height = 2.7f * mazeWidth + 1.0f;
        newPos.y = height;
        //mazeCam should start at the middle of the maze
        float startingX = newPos.x;
        mazeCam.transform.position = new Vector3(startingX, 2, -17);
        //time accroding to size of maze 1.35f
        overTime = Mathf.Log(mazeWidth, 1.35f);
        StartCoroutine(MoveCamera(transform.position, newPos, overTime));
        Quaternion targetRotation = Quaternion.Euler(90, 0, 0);
        StartCoroutine(RotateCamera(transform.rotation, targetRotation, overTime / 3));
        StartCoroutine(ShowPath(pathStartEnd, timeBetweenPathLight));
        StartCoroutine(GoToPlayer(transform.position, cameraOfPlayer.transform.position, overTime / 3));
        StartCoroutine(RotateCameraAccrodingPlayer(transform.rotation, cameraOfPlayer.transform.rotation, overTime / 3));
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
        isCameraRotated = true;
    }

    IEnumerator ShowPath(List<int> pathStartEnd, float timeBetweenPathLight)
    {
        pathStartEnd.Reverse();
        while (!isPathShown)
        {
            if (isCameraRotated)
            {
                Debug.Log("ShowPath: " + "called with List: " + pathStartEnd.ToString() + "Listsize:" + pathStartEnd.Count);

                foreach (int cellId in pathStartEnd)
                {
                    Cell cell = cellsOfMaze[cellId];
                    Vector2 middlePointOfCell = cell.GetMiddlepointOfCellXandZ();
                    Instantiate(spotLight, new Vector3(middlePointOfCell.x, spotLight.transform.position.y, middlePointOfCell.y), spotLight.transform.rotation, spotLights.transform);
                    yield return new WaitForSeconds(timeBetweenPathLight);
                }
                isPathShown = true;
            }
            yield return null;
        }
    }

    IEnumerator GoToPlayer(Vector3 source, Vector3 target, float overTime)
    {
       
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            if (isPathShown)
            {
                transform.position = Vector3.Lerp(source, target, (Time.time - startTime) / overTime);
                yield return null;
            }
            else
            {
                startTime = Time.time;
                yield return null;
            }
        }
        transform.position = target;
        isCameraAtPlayer = true;
    }

    IEnumerator RotateCameraAccrodingPlayer(Quaternion source, Quaternion target, float overTime)
    {
        float startTime = Time.time;
        while (Time.time < startTime + overTime)
        {
            if (isPathShown)
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
        isCameraRotated = true;
    }
}
