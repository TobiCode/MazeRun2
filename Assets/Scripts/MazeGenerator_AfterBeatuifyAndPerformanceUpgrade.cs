using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MazeGenerator_AfterBeatuifyAndPerformanceUpgrade : MonoBehaviour
{

    public NavMeshSurface surface;

    private Dictionary<int, Cell> cellsOfMaze;

    public int yOffsetWall;
    public int mazeWidth;
    public int mazeHeight;
    public GameObject wallPrefab;
    public GameObject maze;
    private Material[] materials;

    public GameObject policeCar;
    public GameObject ambulanceCar;

    public float beautifyZShift;

    //Coordinates of entry and exit
    private float entryX;
    private float entryY;
    private float entryZ;
    private int entryId;

    private float exitX;
    private float exitY;
    private float exitZ;
    private int exitId;

    private List<int> pathStartEnd;

    // Start is called before the first frame update
    void Start()
    {
        //Every Startup we want a random material for the walls
        materials = Resources.LoadAll<Material>("WallMats");
        int randomNumber = Random.RandomRange(0, materials.Length - 1);
        wallPrefab.GetComponent<MeshRenderer>().material = materials[randomNumber];
        cellsOfMaze = InitializeAllCells(mazeWidth, mazeHeight);
        InitializeWallsOfCells(wallPrefab, mazeWidth, mazeHeight);
        ////Delete Walls to get Maze
        Cell initCell = cellsOfMaze[Random.RandomRange(1, (mazeWidth * mazeHeight))];
        DeleteWallsDepthFirst(initCell);
        CreateEntryAndExitOfMaze(mazeWidth, mazeHeight);

        //Instantiate Police and Ambulance
        CreatePoliceAndAmbulanceAtEnd(ambulanceCar, policeCar);

        ////Update NavMesh
        //surface.BuildNavMesh();

        //    List<int> path = FindPathStartToEndDepths(entryId, exitId);
        //    foreach (int step in path)
        //    {
        //        Debug.Log("Debug Pathfinding: " + step.ToString());
        //    }
        //    Debug.Log("Debug Pathfinding: " + path.ToString());

    }

    // Update is called once per frame
    void Update()
    {

    }

    private Dictionary<int, Cell> InitializeAllCells(int mazeWidth, int mazeHeight)
    {
        Dictionary<int, Cell> cellsOfMaze = new Dictionary<int, Cell>();
        int num_cells = mazeWidth * mazeHeight;
        for (int i = 1; i <= num_cells; i++)
        {
            Cell cell = new Cell(i, mazeWidth, mazeHeight);
            cellsOfMaze[i] = cell;
        }
        return cellsOfMaze;
    }

    private void InitializeWallsOfCells(GameObject wallPrefab, int mazeWidth, int mazeHeight)
    {
        float lengthWall = wallPrefab.transform.localScale.x;
        float widthWall = wallPrefab.transform.localScale.z;

        foreach (KeyValuePair<int, Cell> cell in cellsOfMaze)
        {
            CreateBottomWall(cell.Value, lengthWall, widthWall);
            CreateLeftWall(cell.Value, lengthWall, widthWall);
            CreateRightWall(cell.Value, lengthWall, widthWall);
            CreateTopWall(cell.Value, lengthWall, widthWall);
            //break;
        }
    }

    private void CreateBottomWall(Cell cell, float lengthWall, float widthWall)
    {
        //Bottom Wall
        if (cell.BottomNeighbor > 0)
        {
            //Get BottomNeighborCell
            Cell bottomNeighborCell = cellsOfMaze[cell.BottomNeighbor];
            if (bottomNeighborCell.TopWall != null)
            {
                //Use the existingWall
                cell.BottomWall = bottomNeighborCell.TopWall;
            }
            else
            {
                //Set Wall
                int indexOfCell = cell.Id;
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall;
                }
                float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall;
                Vector3 rot = new Vector3(0, 0, 0);
                GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, yOffsetWall, zOfWall), Quaternion.Euler(rot), maze.transform);
                cell.BottomWall = newWall;
                newWall.name = indexOfCell + "_BottomWall";
            }
        }
        else
        {
            //Set Wall
            int indexOfCell = cell.Id;
            float xOfWall = (indexOfCell % mazeWidth) * -lengthWall;
            if (indexOfCell % mazeWidth == 0)
            {
                //last cell of row
                xOfWall = mazeWidth * -lengthWall;
            }
            float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall;
            Vector3 rot = new Vector3(0, 0, 0);
            GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, yOffsetWall, zOfWall), Quaternion.Euler(rot), maze.transform);
            cell.BottomWall = newWall;
            newWall.name = indexOfCell + "_BottomWall";
        }
    }

    private void CreateRightWall(Cell cell, float lengthWall, float widthWall)
    {
        if (cell.RightNeighbor > 0)
        {
            //Get BottomNeighborCell
            Cell rightNeigborCell = cellsOfMaze[cell.RightNeighbor];
            if (rightNeigborCell.LeftWall != null)
            {
                //Use the existingWall
                cell.RightWall = rightNeigborCell.LeftWall;
            }
            else
            {
                //Set Wall
                int indexOfCell = cell.Id;
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + lengthWall / 2;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall + lengthWall / 2;
                }
                float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall + widthWall + beautifyZShift;
                Vector3 rot = new Vector3(0, 90, 0);
                GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, yOffsetWall, zOfWall), Quaternion.Euler(rot), maze.transform);
                cell.RightWall = newWall;
                newWall.name = indexOfCell + "_RightWall";
            }
        }
        else
        {
            //Set Wall
            int indexOfCell = cell.Id;
            float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + lengthWall / 2;
            if (indexOfCell % mazeWidth == 0)
            {
                //last cell of row
                xOfWall = mazeWidth * -lengthWall + lengthWall / 2;
            }
            float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall + widthWall + beautifyZShift;
            Vector3 rot = new Vector3(0, 90, 0);
            GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, yOffsetWall, zOfWall), Quaternion.Euler(rot), maze.transform);
            cell.RightWall = newWall;
            newWall.name = indexOfCell + "_RightWall";
        }
    }

    private void CreateLeftWall(Cell cell, float lengthWall, float widthWall)
    {
        //Bottom Wall
        if (cell.LeftNeighbor > 0)
        {
            //Get BottomNeighborCell
            Cell leftNeighborCell = cellsOfMaze[cell.LeftNeighbor];
            if (leftNeighborCell.RightWall != null)
            {
                //Use the existingWall
                cell.LeftWall = leftNeighborCell.RightWall;
            }
            else
            {
                //Set Wall
                int indexOfCell = cell.Id;
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall - lengthWall / 2;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall - lengthWall / 2;
                }
                float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall + widthWall + beautifyZShift;
                Vector3 rot = new Vector3(0, 90, 0);
                GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, yOffsetWall, zOfWall), Quaternion.Euler(rot), maze.transform);
                cell.LeftWall = newWall;
                newWall.name = indexOfCell + "_LeftWall";
            }
        }
        else
        {
            //Set Wall
            int indexOfCell = cell.Id;
            float xOfWall = (indexOfCell % mazeWidth) * -lengthWall - lengthWall / 2;
            if (indexOfCell % mazeWidth == 0)
            {
                //last cell of row
                xOfWall = mazeWidth * -lengthWall - lengthWall / 2;
            }
            float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall + widthWall + beautifyZShift;
            Vector3 rot = new Vector3(0, 90, 0);
            GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, yOffsetWall, zOfWall), Quaternion.Euler(rot), maze.transform);
            cell.LeftWall = newWall;
            newWall.name = indexOfCell + "_LeftWall";
        }
    }

    private void CreateTopWall(Cell cell, float lengthWall, float widthWall)
    {
        //Bottom Wall
        if (cell.TopNeighbor > 0)
        {
            //Get BottomNeighborCell
            Cell topNeighborCell = cellsOfMaze[cell.TopNeighbor];
            if (topNeighborCell.BottomWall != null)
            {
                //Use the existingWall
                cell.TopWall = topNeighborCell.BottomWall;
            }
            else
            {
                //Set Wall
                int indexOfCell = cell.Id;
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall;
                }
                float zOfWall = ((indexOfCell - 1) / mazeWidth + 1) * lengthWall + 2 * widthWall;
                Vector3 rot = new Vector3(0, 0, 0);
                GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, yOffsetWall, zOfWall), Quaternion.Euler(rot), maze.transform);
                cell.TopWall = newWall;
                newWall.name = indexOfCell + "_TopWall";
            }
        }
        else
        {
            //Set Wall
            int indexOfCell = cell.Id;
            float xOfWall = (indexOfCell % mazeWidth) * -lengthWall;
            if (indexOfCell % mazeWidth == 0)
            {
                //last cell of row
                xOfWall = mazeWidth * -lengthWall;
            }
            float zOfWall = ((indexOfCell - 1) / mazeWidth + 1) * lengthWall + 2 * widthWall;
            Vector3 rot = new Vector3(0, 0, 0);
            GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, yOffsetWall, zOfWall), Quaternion.Euler(rot), maze.transform);
            cell.TopWall = newWall;
            newWall.name = indexOfCell + "_TopWall";
        }
    }

    private void DeleteWallsDepthFirst(Cell randomCell)
    {
        Debug.Log("Starting with Cell: " + randomCell);
        randomCell.Visited = true;
        Cell rightNeighbor, leftNeighbor, topNeighbor, bottomNeighbor;
        GetNeighborCells(randomCell, out rightNeighbor, out leftNeighbor, out topNeighbor, out bottomNeighbor);
        Debug.Log("Right Neighbor: " + rightNeighbor.Id
        + "\n Left Neighbor: " + leftNeighbor.Id
        + "\n Top Neighbor: " + topNeighbor.Id
        + "\n Bottom Neighbor: " + bottomNeighbor.Id);

        while (rightNeighbor.Visited == false || leftNeighbor.Visited == false || topNeighbor.Visited == false || bottomNeighbor.Visited == false)
        {
            int randNumber = Random.Range(1, 5);
            if (randNumber == 1 && rightNeighbor.Visited == false)
            {
                Debug.Log("Delete Right Wall between: " + randomCell.Id + "-" + rightNeighbor.Id);
                Destroy(randomCell.RightWall);
                randomCell.RightWall = null;
                rightNeighbor.LeftWall = null;
                DeleteWallsDepthFirst(rightNeighbor);
            }
            else if (randNumber == 2 && leftNeighbor.Visited == false)
            {
                Debug.Log("Delete Left Wall between: " + randomCell.Id + "-" + leftNeighbor.Id);
                Destroy(randomCell.LeftWall);
                randomCell.LeftWall = null;
                leftNeighbor.RightWall = null;
                DeleteWallsDepthFirst(leftNeighbor);
            }
            else if (randNumber == 3 && topNeighbor.Visited == false)
            {
                Debug.Log("Delete Top Wall between: " + randomCell.Id + "-" + topNeighbor.Id);
                Destroy(randomCell.TopWall);
                randomCell.TopWall = null;
                topNeighbor.BottomWall = null;
                DeleteWallsDepthFirst(topNeighbor);
            }
            else if (randNumber == 4 && bottomNeighbor.Visited == false)
            {
                Debug.Log("Delete Bottom Wall between: " + randomCell.Id + "-" + bottomNeighbor.Id);
                Destroy(randomCell.BottomWall);
                randomCell.BottomWall = null;
                bottomNeighbor.TopWall = null;
                DeleteWallsDepthFirst(bottomNeighbor);
            }
        }
    }

    private void GetNeighborCells(Cell randomCell, out Cell rightNeighbor, out Cell leftNeighbor, out Cell topNeighbor, out Cell bottomNeighbor)
    {
        rightNeighbor = null;
        if (randomCell.RightNeighbor > 0)
        {
            rightNeighbor = cellsOfMaze[randomCell.RightNeighbor];
        }
        else
        {
            rightNeighbor = new Cell(-1, mazeWidth, mazeHeight);
            rightNeighbor.Visited = true;
        }
        leftNeighbor = null;
        if (randomCell.LeftNeighbor > 0)
        {
            leftNeighbor = cellsOfMaze[randomCell.LeftNeighbor];
        }
        else
        {
            leftNeighbor = new Cell(-1, mazeWidth, mazeHeight);
            leftNeighbor.Visited = true;
        }
        topNeighbor = null;
        if (randomCell.TopNeighbor > 0)
        {
            topNeighbor = cellsOfMaze[randomCell.TopNeighbor];
        }
        else
        {
            topNeighbor = new Cell(-1, mazeWidth, mazeHeight);
            topNeighbor.Visited = true;
        }
        bottomNeighbor = null;
        if (randomCell.BottomNeighbor > 0)
        {
            bottomNeighbor = cellsOfMaze[randomCell.BottomNeighbor];
        }
        else
        {
            bottomNeighbor = new Cell(-1, mazeWidth, mazeHeight);
            bottomNeighbor.Visited = true;
        }
    }

    private void CreateEntryAndExitOfMaze(int mazeWidth, int mazeHeight)
    {
        //Create Entry
        Cell entryCell = cellsOfMaze[Random.RandomRange(1, mazeWidth)];
        entryX = entryCell.BottomWall.transform.position.x;
        entryY = entryCell.BottomWall.transform.position.y;
        entryZ = entryCell.BottomWall.transform.position.z;
        entryId = entryCell.Id;
        Destroy((entryCell.BottomWall));

        //Create Exit
        int randomExitValue = Random.RandomRange(mazeHeight * mazeWidth - mazeWidth + 1, mazeHeight * mazeWidth);
        Cell exitCell = cellsOfMaze[randomExitValue];
        exitX = exitCell.TopWall.transform.position.x;
        exitY = exitCell.TopWall.transform.position.y;
        exitZ = exitCell.TopWall.transform.position.z;
        exitId = exitCell.Id;
        Destroy((exitCell.TopWall));
    }

    private void CreatePoliceAndAmbulanceAtEnd(GameObject ambulanceCar, GameObject policeCar)
    {
        Vector3 rot = new Vector3(0, 240, 0);
        Instantiate(policeCar, new Vector3(exitX + 4, exitY, exitZ + 10), Quaternion.Euler(rot), maze.transform);
        Vector3 rot2 = new Vector3(0, 120, 0);
        Instantiate(ambulanceCar, new Vector3(exitX - 4, exitY, exitZ + 10), Quaternion.Euler(rot2), maze.transform);
    }

    //private List<int> FindPathStartToEndDepths(int startId, int endId)
    //{
    //    foreach (Cell cell in cellsOfMaze)
    //    {
    //        cell.Visited = false;
    //    }

    //    Cell startCell = FindCellInCellList(startId);

    //    Queue<int> queue = new Queue<int>();
    //    queue.Enqueue(startId);

    //    while (queue.Count > 0)
    //    {
    //        int currentCellId = queue.Dequeue();
    //        Cell currentCell = FindCellInCellList(currentCellId);
    //        if (currentCellId == endId)
    //        {
    //            currentCell.VisitedCells.Add(currentCellId);
    //            return currentCell.VisitedCells;
    //        }

    //        else
    //        {
    //            if (currentCell.BottomNeighbor > 0 && (currentCell.BottomWall == null | currentCell.BottomNeighbor == exitId))
    //            {
    //                Cell bottomCell = FindCellInCellList(currentCell.BottomNeighbor);
    //                if (bottomCell.Visited == false)
    //                {

    //                    bottomCell.VisitedCells = currentCell.VisitedCells;
    //                    bottomCell.VisitedCells.Add(currentCellId);
    //                    queue.Enqueue(bottomCell.Id);
    //                }
    //            }

    //            if (currentCell.TopNeighbor > 0 && (currentCell.TopWall == null | currentCell.TopNeighbor == exitId))
    //            {
    //                Cell topCell = FindCellInCellList(currentCell.TopNeighbor);
    //                if (topCell.Visited == false)
    //                {

    //                    topCell.VisitedCells = currentCell.VisitedCells;
    //                    topCell.VisitedCells.Add(currentCellId);
    //                    queue.Enqueue(topCell.Id);
    //                }
    //            }

    //            if (currentCell.LeftNeighbor > 0 && (currentCell.LeftWall == null | currentCell.LeftNeighbor == exitId))
    //            {
    //                Cell leftCell = FindCellInCellList(currentCell.LeftNeighbor);
    //                if (leftCell.Visited == false)
    //                {

    //                    leftCell.VisitedCells = currentCell.VisitedCells;
    //                    leftCell.VisitedCells.Add(currentCellId);
    //                    queue.Enqueue(leftCell.Id);
    //                }
    //            }

    //            if (currentCell.RightNeighbor > 0 && (currentCell.RightWall == null | currentCell.RightNeighbor == exitId))
    //            {
    //                Cell rightCell = FindCellInCellList(currentCell.RightNeighbor);
    //                if (rightCell.Visited == false)
    //                {

    //                    rightCell.VisitedCells = currentCell.VisitedCells;
    //                    rightCell.VisitedCells.Add(currentCellId);
    //                    queue.Enqueue(rightCell.Id);
    //                }
    //            }
    //        }
    //    }
    //    return null;
    //}
}

