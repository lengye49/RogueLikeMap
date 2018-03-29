using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapDataCenter{
	private int rowsCount;//thisMap.Rows
	private int columnsCount;//thisMap.Columns
	private MapInfo mapInfo;
	public Grid[,] gridList;
	public Grid enterGrid;
		
    public int Rows{
        get{ return rowsCount;}
    }
    public int Columns{
        get{ return columnsCount;}
    }

	public MapDataCenter(){
        mapInfo = new MapInfo ("test.map");
		rowsCount = mapInfo.RowsCount;
		columnsCount = mapInfo.ColumnsCount;
        Debug.Log("Total = " + rowsCount + "," + columnsCount);
		
		InitMapData ();
	}
        

	void InitMapData(){
		int bossNum = CalculateMethod.GetRandomValue (mapInfo.BossRange);
		int monsterNum = CalculateMethod.GetRandomValue (mapInfo.MonsterRange);
		int eventNum = CalculateMethod.GetRandomValue (mapInfo.EventRange);
		int blockNum = CalculateMethod.GetRandomValue (mapInfo.BlockRange);

		List<Grid> gridPicked;
        Stack gridStack;
        Grid thisGrid;
        Grid nextGrid;
        List<Grid> neighbours;
        List<Grid> ends;

		
        UnityEngine.Random.InitState((int)Time.time);
		do {
            InitGridList();

            int r1 = CalculateMethod.GetRandomValue (0, rowsCount);
            int r2 = CalculateMethod.GetRandomValue (0, columnsCount);
            gridStack=new Stack();
            gridPicked=new List<Grid>();
            thisGrid = gridList[r1, r2];
            thisGrid.isPicked = true;
            gridPicked.Add (thisGrid);
            gridStack.Push(thisGrid);

			for (int i = 0; i < rowsCount * columnsCount - blockNum; i++) {
                neighbours = new List<Grid>();
				do {
                    thisGrid = gridStack.Pop() as Grid;
					neighbours = UnpickedGridNeighbour (thisGrid);
				} while(neighbours.Count == 0);
				nextGrid = RandomGrid (neighbours);
                gridStack.Push(thisGrid);
                gridStack.Push(nextGrid);

                nextGrid.isPicked=true;
                gridPicked.Add(nextGrid);
			}
			ends = EndPoints (gridPicked);
		} while(ends.Count < bossNum);



        //这一部分直接把数据存到gridList里面即可
        List<Grid> bossPoints = SetGridType (ref ends, bossNum,GridType.Boss);
		RemoveExist (ref gridPicked, ref bossPoints);
		RemoveExist (ref ends, ref bossPoints);
        List<Grid> enterPoints = SetGridType (ref gridPicked, 1,GridType.Enter);
		enterGrid = enterPoints [0];
		RemoveExist (ref gridPicked, ref enterPoints);
        List<Grid> monsterPoints = SetGridType (ref gridPicked, monsterNum,GridType.Monster);
		RemoveExist (ref gridPicked, ref monsterPoints);
        List<Grid> eventPoints = SetGridType (ref gridPicked, eventNum,GridType.Event);
		RemoveExist (ref gridPicked, ref eventPoints);
        List<Grid> emptyPoints = SetGridType(ref gridPicked, gridPicked.Count, GridType.Road);
	}

    void InitGridList(){
        gridList = new Grid[rowsCount, columnsCount];
        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < columnsCount; j++)
                gridList[i, j] = new Grid(i,j);
        }
    }

	void RemoveExist(ref List<Grid> org,ref List<Grid> exist){
		for (int i = 0; i < exist.Count; i++) {
			try{
				org.Remove (exist [i]);
			}catch(Exception e){
				Debug.Log (e);
			}
		}
	}

    List<Grid> SetGridType(ref List<Grid> gridPool,int requestNum,GridType t){
		List<Grid> gl = new List<Grid> ();
		for(int i=0;i<requestNum;i++){
			Grid g = RandomGrid (gridPool);
            gridList[g.x, g.y].type = t;
			gl.Add (g);
            gridPool.Remove (g);
		}
		return gl;
	}

	Grid RandomGrid(List<Grid> gridPool){
		int r = CalculateMethod.GetRandomValueForNewGrid ( gridPool.Count);
		return gridPool [r];
	}

	/// <summary>
	/// 查找端点：只联通一个点的点
	/// </summary>
	/// <returns>The points.</returns>
	/// <param name="pickedPoints">Picked points.</param>
	List<Grid> EndPoints(List<Grid> orgs){
		List<Grid> ends = new List<Grid> ();
		List<Grid> neighbours = new List<Grid> ();
		int num;
		for (int i = 0; i < orgs.Count; i++) {
			num = 0;
			neighbours = GridNeighbour (orgs [i]);
			for (int j = 0; j < neighbours.Count; j++) {
				if (orgs.Contains (neighbours [j]))
					num++;
			}
			if (num <=1)
				ends.Add (orgs [i]);
		}
		return ends;
	}

	private ArrayList openList;
	private ArrayList closeList;
	private List<Grid> path;
	private string road;
	/// <summary>
	/// 简单的A星寻路算法,不走对角线
	/// </summary>
	/// <returns>The road.</returns>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	public List<Grid> FindPath(int startX,int startY,int endX,int endY){
		Debug.Log ("A* Started! Looking" + endX + "," + endY + "-->" + startX + "," + startY);
		road = "";
		path = new List<Grid> ();
		openList = new ArrayList ();
		closeList = new ArrayList ();
        ResetGridState();

		openList.Add (gridList [startX, startY]);
		Grid current = openList [0] as Grid;

		while (openList.Count > 0 && (startX != endX || startY != endY)) {
			current = openList [0] as Grid;
			if (current.x == endX && current.y == endY) {
                Debug.Log("Path Found!");
				GenerateRoad(current);
                Debug.Log(road);
				return path;
			}
			foreach (Grid _grid in GridNeighbour(current)) {
				if (_grid.type != GridType.Block && !closeList.Contains (_grid)) {

					int g = current.g + 1;
					if (_grid.g == 0 || _grid.g > g) {
						_grid.g = g;
						_grid.parent = current;
					}

					_grid.h = Mathf.Abs (endX - _grid.x) + Mathf.Abs (endY - _grid.y);
					_grid.f = _grid.g + _grid.h;
					if (!openList.Contains (_grid))
						openList.Add (_grid);

					//根据f值进行升序排序
					openList.Sort ();
				}
			}
			closeList.Add (current);
			openList.Remove (current);

			if (openList.Count == 0)
				Debug.Log ("UnReachable Point!");
		}
		return path;
	}

	void GenerateRoad(Grid g){
		road += "(" + g.x + "," + g.y + ")";
		path.Add (g);
		if (g.parent != null)
			GenerateRoad (g.parent);
	}	

    void ResetGridState(){
        for (int i = 0; i < rowsCount; i++)
        {
			for (int j = 0; j < columnsCount; j++) {
				gridList [i, j].parent = null;
				gridList [i, j].g = 0;
				gridList [i, j].f = 0;
				gridList [i, j].h = 0;
			}
        }
    }

	List<Grid> GridNeighbour(Grid org){
		List<Grid> neighbour = new List<Grid> ();
		if (org.x != 0)
			neighbour.Add (gridList [org.x - 1, org.y]);
		if (org.y != 0)
			neighbour.Add (gridList [org.x, org.y - 1]);
		if (org.x != columnsCount - 1)
			neighbour.Add (gridList [org.x + 1, org.y]);
		if (org.y != rowsCount - 1)
			neighbour.Add (gridList [org.x, org.y + 1]);
		return neighbour;
	}

    List<Grid> UnpickedGridNeighbour(Grid org){
        List<Grid> neighbour = new List<Grid> ();
        if (org.x != 0 && !gridList[org.x - 1, org.y].isPicked)
        {
            neighbour.Add(gridList[org.x - 1, org.y]);
        }
        if (org.y != 0 && !gridList[org.x, org.y - 1].isPicked)
        {
            neighbour.Add(gridList[org.x, org.y - 1]);
        }
        if (org.x != columnsCount - 1 && !gridList[org.x + 1, org.y].isPicked)
        {
            neighbour.Add(gridList[org.x + 1, org.y]);
        }
        if (org.y != rowsCount - 1 && !gridList[org.x, org.y + 1].isPicked)
        {
            neighbour.Add(gridList[org.x, org.y + 1]);
        }
        return neighbour;
    }

}

public class Grid : IComparable{
	public int x;
	public int y;
	public GridType type;

	public int g;//距离起点
	public int h;//距离终点
	public int f;//总值

    public bool isOpen;
	public bool isPicked;
	public Grid parent;

	public Grid(int x,int y){
		this.x = x;
		this.y = y;
        isOpen = false;
		isPicked = false;
	}

	//升序，用于Sort方法
	public int CompareTo(object obj){
		Grid grid = (Grid)obj;
		if (this.f < grid.f)
			return -1;
		if (this.f > grid.f)
			return 1;
		return 0;
	}

    public void Print(){
        Debug.Log(" = [" + x + "," + y + "]");
    }

}
	

public enum GridType{
	Covered,
	Road,
    Boss,
    Monster,
    Event,
    Enter,
    Block,
}

