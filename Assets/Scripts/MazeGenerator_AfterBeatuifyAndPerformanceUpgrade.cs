using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class MazeGenerator_AfterBeatuifyAndPerformanceUpgrade : MonoBehaviour
{

    public NavMeshSurface surface;

    public Dictionary<int, Cell> cellsOfMaze;

    public int yOffsetWall;
    public int xMazeStart;
    public int mazeWidth;
    public int mazeHeight;
    public GameObject wallPrefab;
    public GameObject maze;
    private Material[] materials;

    public GameObject policeCar;
    public GameObject ambulanceCar;
    public GameObject player;
    public GameObject enemy;
    public GameObject enemyClone;

    public float beautifyZShift;

    //Coordinates of entry and exit
    public float entryX;
    public float entryY;
    public float entryZ;
    private int entryId;

    private float exitX;
    private float exitY;
    private float exitZ;
    private int exitId;

    public List<int> pathStartEnd;
    public float distanceEnemy;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void GenerateMazeAndSetPlayerEnemyToEntry()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //Every Startup we want a random material for the walls
        materials = Resources.LoadAll<Material>("WallMats");
        int randomNumber = Random.RandomRange(0, materials.Length - 1);
        wallPrefab.GetComponent<MeshRenderer>().material = materials[randomNumber];
        cellsOfMaze = InitializeAllCells(mazeWidth, mazeHeight);
        InitializeWallsOfCells(wallPrefab, mazeWidth, mazeHeight);
        //Delete Walls to get Maze
        Cell initCell = cellsOfMaze[Random.RandomRange(1, (mazeWidth * mazeHeight))];
        DeleteWallsDepthFirst(initCell);
        CreateEntryAndExitOfMaze(mazeWidth, mazeHeight);
        //Instantiate Police and Ambulance
        CreatePoliceAndAmbulanceAtEnd(ambulanceCar, policeCar);
        //Moove Player and enemy
        PutPlayerAndEnemyNearEntrance(player, enemy);
        //Update NavMesh
        surface.BuildNavMesh();
        //List<int> path = FindPathStartToEndDepths(entryId, exitId);
        List<int> shortestPath = FindStartToEnd(entryId, exitId);
        pathStartEnd = shortestPath;
        //string shortestPathString = "";
        //foreach (int step in shortestPath)
        //{
        //    shortestPathString = shortestPathString + step + ", ";
        //}
        //Debug.Log("Debug Pathfinding: " + shortestPathString);
        sw.Stop();
        Debug.Log("Elapsed= " + sw.Elapsed);
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
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + xMazeStart;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall + xMazeStart;
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
            float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + xMazeStart;
            if (indexOfCell % mazeWidth == 0)
            {
                //last cell of row
                xOfWall = mazeWidth * -lengthWall + xMazeStart;
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
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + lengthWall / 2 + xMazeStart;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall + lengthWall / 2 + xMazeStart;
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
            float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + lengthWall / 2 + xMazeStart;
            if (indexOfCell % mazeWidth == 0)
            {
                //last cell of row
                xOfWall = mazeWidth * -lengthWall + lengthWall / 2 + xMazeStart;
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
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall - lengthWall / 2 + xMazeStart;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall - lengthWall / 2 + xMazeStart;
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
            float xOfWall = (indexOfCell % mazeWidth) * -lengthWall - lengthWall / 2 + xMazeStart;
            if (indexOfCell % mazeWidth == 0)
            {
                //last cell of row
                xOfWall = mazeWidth * -lengthWall - lengthWall / 2 + xMazeStart;
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
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + xMazeStart;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall + xMazeStart;
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
            float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + xMazeStart;
            if (indexOfCell % mazeWidth == 0)
            {
                //last cell of row
                xOfWall = mazeWidth * -lengthWall + xMazeStart;
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
        randomCell.Visited = true;
        Cell rightNeighbor, leftNeighbor, topNeighbor, bottomNeighbor;
        GetNeighborCells(randomCell, out rightNeighbor, out leftNeighbor, out topNeighbor, out bottomNeighbor);

        while (rightNeighbor.Visited == false || leftNeighbor.Visited == false || topNeighbor.Visited == false || bottomNeighbor.Visited == false)
        {
            int randNumber = Random.Range(1, 5);
            if (randNumber == 1 && rightNeighbor.Visited == false)
            {
                //Debug.Log("Delete  Wall between: " + randomCell.Id + "-" + rightNeighbor.Id);
                Destroy(randomCell.RightWall);
                randomCell.RightWall = null;
                rightNeighbor.LeftWall = null;
                DeleteWallsDepthFirst(rightNeighbor);
            }
            else if (randNumber == 2 && leftNeighbor.Visited == false)
            {
                //Debug.Log("Delete Left Wall between: " + randomCell.Id + "-" + leftNeighbor.Id);
                Destroy(randomCell.LeftWall);
                randomCell.LeftWall = null;
                leftNeighbor.RightWall = null;
                DeleteWallsDepthFirst(leftNeighbor);
            }
            else if (randNumber == 3 && topNeighbor.Visited == false)
            {
                //Debug.Log("Delete Top Wall between: " + randomCell.Id + "-" + topNeighbor.Id);
                Destroy(randomCell.TopWall);
                randomCell.TopWall = null;
                topNeighbor.BottomWall = null;
                DeleteWallsDepthFirst(topNeighbor);
            }
            else if (randNumber == 4 && bottomNeighbor.Visited == false)
            {
                //Debug.Log("Delete Bottom Wall between: " + randomCell.Id + "-" + bottomNeighbor.Id);
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
        const float opticalCorrectnes = 0.2f;
        Instantiate(policeCar, new Vector3(exitX + 4, exitY - opticalCorrectnes, exitZ + 10), Quaternion.Euler(rot), maze.transform);
        Vector3 rot2 = new Vector3(0, 120, 0);
        Instantiate(ambulanceCar, new Vector3(exitX - 4, exitY - opticalCorrectnes, exitZ + 10), Quaternion.Euler(rot2), maze.transform);
    }

    private void PutPlayerAndEnemyNearEntrance(GameObject player, GameObject enemy)
    {
        player.transform.position = new Vector3(entryX, entryY, entryZ - 2);
        Vector3 rot2 = new Vector3(0, 0, 0);
        Vector3 position2 = new Vector3(entryX, entryY, entryZ - distanceEnemy);
        enemyClone = Instantiate(enemy, position2, Quaternion.Euler(rot2));
    }

    //Deprecated
    private List<int> FindPathStartToEndDepths(int startId, int endId)
    {
        foreach (KeyValuePair<int, Cell> cell in cellsOfMaze)
        {
            cell.Value.Visited = false;
        }

        Cell startCell = cellsOfMaze[startId];

        Queue<int> queue = new Queue<int>();
        queue.Enqueue(startId);

        while (queue.Count > 0)
        {
            int currentCellId = queue.Dequeue();
            Cell currentCell = cellsOfMaze[currentCellId];
            if (currentCellId == endId)
            {
                currentCell.VisitedCells.Add(currentCellId);
                return currentCell.VisitedCells;
            }

            else
            {
                if (currentCell.BottomNeighbor > 0 && (currentCell.BottomWall == null | currentCell.BottomNeighbor == exitId))
                {
                    Cell bottomCell = cellsOfMaze[currentCell.BottomNeighbor];
                    if (bottomCell.Visited == false)
                    {

                        bottomCell.VisitedCells = currentCell.VisitedCells;
                        bottomCell.VisitedCells.Add(currentCellId);
                        queue.Enqueue(bottomCell.Id);
                    }
                }

                if (currentCell.TopNeighbor > 0 && (currentCell.TopWall == null | currentCell.TopNeighbor == exitId))
                {
                    Cell topCell = cellsOfMaze[currentCell.TopNeighbor];
                    if (topCell.Visited == false)
                    {

                        topCell.VisitedCells = currentCell.VisitedCells;
                        topCell.VisitedCells.Add(currentCellId);
                        queue.Enqueue(topCell.Id);
                    }
                }

                if (currentCell.LeftNeighbor > 0 && (currentCell.LeftWall == null | currentCell.LeftNeighbor == exitId))
                {
                    Cell leftCell = cellsOfMaze[currentCell.LeftNeighbor];
                    if (leftCell.Visited == false)
                    {

                        leftCell.VisitedCells = currentCell.VisitedCells;
                        leftCell.VisitedCells.Add(currentCellId);
                        queue.Enqueue(leftCell.Id);
                    }
                }

                if (currentCell.RightNeighbor > 0 && (currentCell.RightWall == null | currentCell.RightNeighbor == exitId))
                {
                    Cell rightCell = cellsOfMaze[currentCell.RightNeighbor];
                    if (rightCell.Visited == false)
                    {

                        rightCell.VisitedCells = currentCell.VisitedCells;
                        rightCell.VisitedCells.Add(currentCellId);
                        queue.Enqueue(rightCell.Id);
                    }
                }
            }
        }
        return null;
    }

    //Breadth First Search
    //TODO: BreadthSearchCounterNotSetCorrectly, will set previous Cells in newNextCells.
    private List<int> FindStartToEnd(int startId, int endId)
    {
        int counter = 0;
        //cellsOfMaze[startId].BreathSearchCounter = counter;
        List<int> nextCells = new List<int>();
        nextCells.Add(startId);
        do
        {
            List<int> newNextCells = new List<int>();
            //Update counter of the NextCells and add their neighbors to the NewNextCellsList
            foreach (int cellId in nextCells)
            {
                cellsOfMaze[cellId].BreathSearchCounter = counter;
                cellsOfMaze[cellId].BreadthSearchCounterIsSet = true;
                foreach (int newNextCellId in cellsOfMaze[cellId].GetVisitableCells())
                {
                    if(cellsOfMaze[newNextCellId].BreadthSearchCounterIsSet == false)
                    {
                        newNextCells.Add(newNextCellId);
                    }
                }
            }
            counter += 1;

            //string debugText = "Test NewNextCells";
            //foreach (int testId in newNextCells)
            //{
            //    debugText += testId + ", ";
            //}
            //Debug.Log(debugText);

            nextCells = newNextCells;
        }
        while (!nextCells.Contains(endId));

        //All counters set and EndCell reached. Now go back from the endcell to the startcell the path with the lowest 
        Cell endCell = cellsOfMaze[endId];
        endCell.BreathSearchCounter = counter;
        List<int> shortestPath = new List<int>();
        shortestPath.Add(endCell.Id);
        do
        {
            //Debug.Log("Test: GoBack From" + endCell.ToString());
            Cell nextCell = GetNextCellBackwardStepBreadthSearch(endCell);
            shortestPath.Add(nextCell.Id);
            endCell = nextCell;
        }
        while (endCell.Id != startId);
        return shortestPath;
    }

    private Cell GetNextCellBackwardStepBreadthSearch(Cell currentCell)
    {
        List<int> neighborCells = currentCell.GetVisitableCells();
        foreach (int neighborCellId in neighborCells)
        {
            //Debug.Log("Test: GoBack Neighbors" + cellsOfMaze[neighborCellId].ToString());

            if (cellsOfMaze[neighborCellId].BreathSearchCounter == currentCell.BreathSearchCounter - 1)
            {
                return cellsOfMaze[neighborCellId];
            }
        }
        return null;
    }
}

