using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    private List<Cell> cellsOfMaze;

    public int mazeWidth;
    public int mazeHeight;
    public GameObject wallPrefab;
    public GameObject maze;




    // Start is called before the first frame update
    void Start()
    {
        cellsOfMaze = InitializeAllCells(mazeWidth, mazeHeight);
        InitializeWallsOfCells(wallPrefab, mazeWidth, mazeHeight);
        
        foreach (Cell cell in cellsOfMaze)
        {
            Debug.Log(cell.ToString());
        }
        
        //Delete Walls to get Maze
        Cell initCell = FindCellInCellList(Random.RandomRange(1, (mazeWidth * mazeHeight)));
        DeleteWallsDepthFirst(initCell);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private List<Cell> InitializeAllCells(int mazeWidth, int mazeHeight)
    {
        List<Cell> cellsOfMaze = new List<Cell>();
        int num_cells = mazeWidth * mazeHeight;

        for (int i = 1; i <= num_cells; i++)
        {
            Cell cell = new Cell(i, mazeWidth, mazeHeight);
            cellsOfMaze.Add(cell);
        }

        return cellsOfMaze;
    }

    private Cell FindCellInCellList(int id)
    {
        foreach (Cell cell in cellsOfMaze)
        {
            if (cell.Id == id)
            {
                return cell;
            }
        }

        return null;
    }

    private void InitializeWallsOfCells(GameObject wallPrefab, int mazeWidth, int mazeHeight)
    {
        float lengthWall = wallPrefab.transform.localScale.x;
        float widthWall = wallPrefab.transform.localScale.z;

        foreach (Cell cell in cellsOfMaze)
        {
            CreateBottomWall(cell, lengthWall, widthWall);
            CreateLeftWall(cell, lengthWall, widthWall);
            CreateRightWall(cell, lengthWall, widthWall);
            CreateTopWall(cell, lengthWall, widthWall);
            //break;
        }
    }


    private void CreateBottomWall(Cell cell,float lengthWall, float widthWall)
    {
        //Bottom Wall
        if (cell.BottomNeighbor > 0)
        {
            //Get BottomNeighborCell
            Cell bottomNeighborCell = FindCellInCellList(cell.BottomNeighbor);
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
                GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, 0, zOfWall), Quaternion.Euler(rot), maze.transform);
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
            GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, 0, zOfWall), Quaternion.Euler(rot), maze.transform);
            cell.BottomWall = newWall;
            newWall.name = indexOfCell + "_BottomWall";
        }
    }

    private void CreateRightWall(Cell cell, float lengthWall, float widthWall)
    {
        if (cell.RightNeighbor > 0)
        {
            //Get BottomNeighborCell
            Cell rightNeigborCell = FindCellInCellList(cell.RightNeighbor);
            if (rightNeigborCell.LeftWall != null)
            {
                //Use the existingWall
                cell.RightWall = rightNeigborCell.LeftWall;
            }
            else
            {
                //Set Wall
                int indexOfCell = cell.Id;
                float xOfWall = (indexOfCell % mazeWidth) * -lengthWall + lengthWall/2;
                if (indexOfCell % mazeWidth == 0)
                {
                    //last cell of row
                    xOfWall = mazeWidth * -lengthWall + lengthWall / 2;
                }
                float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall + widthWall;
                Vector3 rot = new Vector3(0, 90, 0);
                GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, 0, zOfWall), Quaternion.Euler(rot), maze.transform);
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
            float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall + widthWall;
            Vector3 rot = new Vector3(0, 90, 0);
            GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, 0, zOfWall), Quaternion.Euler(rot), maze.transform);
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
            Cell leftNeighborCell = FindCellInCellList(cell.LeftNeighbor);
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
                float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall + widthWall;
                Vector3 rot = new Vector3(0, 90, 0);
                GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, 0, zOfWall), Quaternion.Euler(rot), maze.transform);
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
            float zOfWall = ((indexOfCell - 1) / mazeWidth) * lengthWall + 2 * widthWall + widthWall;
            Vector3 rot = new Vector3(0, 90, 0);
            GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, 0, zOfWall), Quaternion.Euler(rot), maze.transform);
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
            Cell topNeighborCell = FindCellInCellList(cell.TopNeighbor);
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
                float zOfWall = ((indexOfCell - 1) / mazeWidth +1) * lengthWall + 2 * widthWall;
                Vector3 rot = new Vector3(0, 0, 0);
                GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, 0, zOfWall), Quaternion.Euler(rot), maze.transform);
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
            GameObject newWall = Instantiate(wallPrefab, new Vector3(xOfWall, 0, zOfWall), Quaternion.Euler(rot), maze.transform);
            cell.TopWall = newWall;
            newWall.name = indexOfCell + "_TopWall";
        }
    }

    private void DeleteWallsDepthFirst(Cell randomCell)
    {
        randomCell.Visited = true;

        Cell rightNeighbor = null;
        if (randomCell.RightNeighbor > 0)
        {
            rightNeighbor = FindCellInCellList(randomCell.RightNeighbor);
        }
        else
        {
            rightNeighbor = new Cell(-1, mazeWidth, mazeHeight);
            rightNeighbor.Visited = true;
        }
        Cell leftNeighbor = null;
        if (randomCell.LeftNeighbor > 0)
        {
            leftNeighbor = FindCellInCellList(randomCell.LeftNeighbor);
        }
        else
        {
            leftNeighbor = new Cell(-1, mazeWidth, mazeHeight);
            leftNeighbor.Visited = true;
        }
        Cell topNeighbor = null;
        if (randomCell.TopNeighbor > 0)
        {
            topNeighbor = FindCellInCellList(randomCell.TopNeighbor);
        }
        else
        {
            topNeighbor = new Cell(-1, mazeWidth, mazeHeight);
            topNeighbor.Visited = true;
        }
        Cell bottomNeighbor = null;
        if (randomCell.BottomNeighbor > 0)
        {
            bottomNeighbor = FindCellInCellList(randomCell.BottomNeighbor);
        }
        else
        {
            bottomNeighbor = new Cell(-1, mazeWidth, mazeHeight);
            bottomNeighbor.Visited = true;
        }

        while (rightNeighbor.Visited == false || leftNeighbor.Visited == false || topNeighbor.Visited == false || bottomNeighbor.Visited == false)
        {
            int randNumber = Random.Range(1, 5);
            if(randNumber == 1 && rightNeighbor.Visited == false)
            {
                //Connect with rightNeighbor
                Debug.Log("Delete Right Wall between: " + randomCell.Id + "-" + rightNeighbor.Id);
                Destroy(randomCell.RightWall);
                DeleteWallsDepthFirst(rightNeighbor);
            }
            else if (randNumber == 2 && leftNeighbor.Visited == false)
            {
                //Connect with leftNeighbor
                Debug.Log("Delete Left Wall between: " + randomCell.Id + "-" + leftNeighbor.Id);

                Destroy(randomCell.LeftWall);
                DeleteWallsDepthFirst(leftNeighbor);
            }
            else if (randNumber == 3 && topNeighbor.Visited == false)
            {
                //Connect with topNeighbor
                Debug.Log("Delete Top Wall between: " + randomCell.Id + "-" + topNeighbor.Id);

                Destroy(randomCell.TopWall);
                DeleteWallsDepthFirst(topNeighbor);
            }
            else if (randNumber == 4 && bottomNeighbor.Visited == false)
            {
                //Connect with bottomNeighbor
                Debug.Log("Delete Bottom Wall between: " + randomCell.Id + "-" + bottomNeighbor.Id);

                Destroy(randomCell.BottomWall);
                DeleteWallsDepthFirst(bottomNeighbor);
            }
        }
        


    }
}

