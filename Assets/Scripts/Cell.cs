using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private List<int> visitedCells = new List<int>();
    private bool visited = false;
    private int widthMaze;
    private int heightMaze;
    private bool isStart;
    private bool isFinish;
    private GameObject bottomWall;
    private GameObject leftWall;
    private GameObject rightWall;
    private GameObject topWall;
    private int leftNeighbor;
    private int rightNeighbor;
    private int topNeighbor;
    private int bottomNeighbor;
    private int id;
    private int breadthSearchCounter;
    private bool breadthSearchCounterIsSet = false;

    public int LeftNeighbor { get => leftNeighbor; set => leftNeighbor = value; }
    public int RightNeighbor { get => rightNeighbor; set => rightNeighbor = value; }
    public int BottomNeighbor { get => bottomNeighbor; set => bottomNeighbor = value; }
    public int TopNeighbor { get => topNeighbor; set => topNeighbor = value; }
    public GameObject BottomWall { get => bottomWall; set => bottomWall = value; }
    public GameObject LeftWall { get => leftWall; set => leftWall = value; }
    public GameObject RightWall { get => rightWall; set => rightWall = value; }
    public GameObject TopWall { get => topWall; set => topWall = value; }
    public int Id { get => id; set => id = value; }
    public bool Visited { get => visited; set => visited = value; }
    public List<int> VisitedCells { get => visitedCells; set => visitedCells = value; }
    public int BreathSearchCounter { get => breadthSearchCounter; set => breadthSearchCounter = value; }
    public bool BreadthSearchCounterIsSet { get => breadthSearchCounterIsSet; set => breadthSearchCounterIsSet = value; }

    public Cell(int id, int widthMaze, int heightMaze)
    {
        this.id = id;
        this.widthMaze = widthMaze;
        this.heightMaze = heightMaze;
        setNeighbors(id);
    }

    private void setNeighbors(int id)
    {
        //Calculate leftNeighbor
        this.leftNeighbor = (id % widthMaze) > 0 ? id + 1 : 0;
        //Calculate topNeighbor
        this.topNeighbor = id <= widthMaze * heightMaze - widthMaze ? id + widthMaze : 0;
        //Calculate rightNeighbor
        this.rightNeighbor = ((id % widthMaze) != 1) ? id - 1 : 0;
        //Calculate bottomNeighbor
        this.bottomNeighbor = id > widthMaze ? id - widthMaze : 0;
    }
    public override string ToString()
    {
        string returnString = "Cell " + this.id + "\n";
        returnString += "LeftNeighbor: " + this.leftNeighbor + "\n";
        returnString += "TopNeighbor: " + this.topNeighbor + "\n";
        returnString += "RightNeighbor: " + this.rightNeighbor + "\n";
        returnString += "BottomNeighbor: " + this.bottomNeighbor + "\n";
        returnString += "LeftWall: " + this.leftWall + "\n";
        returnString += "RightWall: " + this.rightWall + "\n";
        returnString += "BottomWall: " + this.bottomWall + "\n";
        returnString += "TopWall: " + this.topWall + "\n";
        returnString += "BreadthSearchCounter: " + this.breadthSearchCounter + "\n";

        return returnString;
    }

    public List<int> GetVisitableCells()
    {
        List<int> vistiableCells =  new List<int>();

        if(this.leftNeighbor > 0 && this.leftWall == null)
        {
            vistiableCells.Add(this.LeftNeighbor);
        }
        if (this.rightNeighbor > 0 && this.RightWall == null)
        {
            vistiableCells.Add(this.rightNeighbor);
        }
        if (this.topNeighbor > 0 && this.topWall == null)
        {
            vistiableCells.Add(this.topNeighbor);
        }
        if (this.bottomNeighbor > 0 && this.bottomWall == null)
        {
            vistiableCells.Add(this.bottomNeighbor);
        }
        return vistiableCells;
    }

}
