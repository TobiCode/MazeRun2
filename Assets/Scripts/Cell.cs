using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private bool visited=false;
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

    public Cell(int id, int widthMaze, int heightMaze)
    {
        this.Id = id;
        this.widthMaze = widthMaze;
        this.heightMaze = heightMaze;
        setNeighbors(id);
    }

    private void setNeighbors(int id)
    {
        //Calculate leftNeighbor
        if(id % widthMaze > 0)
        {
            this.LeftNeighbor = id + 1;
        }
        else
        {
            this.LeftNeighbor = 0;
        }

        //Calculate topNeighbor
        if (id <= widthMaze*heightMaze-widthMaze)
        {
            this.TopNeighbor = id + widthMaze;
        }
        else
        {
            this.TopNeighbor = 0;
        }

        //Calculate rightNeighbor
        if ((id % widthMaze) != (1))
        {
            this.RightNeighbor = id - 1;
        }
        else
        {
            this.RightNeighbor = 0;
        }

        //Calculate bottomNeighbor
        if (id > widthMaze)
        {
            this.BottomNeighbor = id - widthMaze;
        }
        else
        {
            this.BottomNeighbor = 0;
        }

    }

    public override string ToString() {
        string returnString = "Cell " + this.id + "\n" ;
        returnString += "LeftNeighbor: " + this.LeftNeighbor + "\n";
        returnString += "TopNeighbor: " + this.TopNeighbor + "\n";
        returnString += "RightNeighbor: " + this.RightNeighbor + "\n";
        returnString += "BottomNeighbor: " + this.BottomNeighbor + "\n";
        returnString += "LeftWall: " + this.LeftWall.name + "\n";
        returnString += "RightWall: " + this.RightWall.name + "\n";
        returnString += "BottomWall: " + this.BottomWall.name + "\n";
        returnString += "TopWall: " + this.TopWall.name + "\n";
        return returnString;
    }




}
