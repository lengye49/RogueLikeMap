using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapDataCenter{
	private int rowsCount;//thisMap.Rows
	private int columnsCount;//thisMap.Columns
	private MapInfo mapInfo;
	private Grid[,] gridList;
		


	//public MapDataCenter(Map m)
	public MapDataCenter(int row,int column){
		rowsCount = 10;
		columnsCount = 10;
		gridList = new Grid[row, column];
		mapInfo = new MapInfo ("test.map");
		InitMapData ();
	}

	void InitMapData(){
		int bossNum = CalculateMethod.GetRandomValue (mapInfo.BossRange);
		int monsterNum = CalculateMethod.GetRandomValue (mapInfo.MonsterRange);
		int eventNum = CalculateMethod.GetRandomValue (mapInfo.EventRange);
		int blockNum = CalculateMethod.GetRandomValue (mapInfo.BlockRange);
		List<Grid> gridPicked = new List<Grid> ();

		int r1 = CalculateMethod.GetRandomValue (0, rowsCount);
		int r2 = CalculateMethod.GetRandomValue (0, columnsCount);
		Grid startPoint = gridList [r1, r2];
		gridPicked.Add (startPoint);

		List<Grid> neighbours;
		Grid thisGrid;
		Grid nextGrid;
		List<Grid> ends;
		do {
			for (int i = 0; i < rowsCount * columnsCount - blockNum; i++) {
				//随机选取当前点
				thisGrid = RandomGrid (gridPicked);
				//查找临点
				do {
					neighbours = GridNeighbour (thisGrid);
					for (int j = 0; j < gridPicked.Count; j++) {
						if (neighbours.Contains (gridPicked [j]))
							neighbours.Remove (gridPicked [j]);
					}
				} while(neighbours.Count == 0);
				//从临点中选取下一点
				nextGrid = RandomGrid (neighbours);
				gridPicked.Add (nextGrid);

			}
			ends = EndPoints (gridPicked);
		} while(ends.Count < bossNum + 1);

		List<Grid> bossPoints = RandomGridList (ends, bossNum);
		RemoveExist (ref gridPicked, ref bossPoints);
		RemoveExist (ref ends, ref bossPoints);
		List<Grid> enterPoints = RandomGridList (ends, 1);
		RemoveExist (ref gridPicked, ref enterPoints);
		List<Grid> monsterPoints = RandomGridList (gridPicked, monsterNum);
		RemoveExist (ref gridPicked, ref monsterPoints);
		List<Grid> eventPoints = RandomGridList (gridPicked, eventNum);
		RemoveExist (ref gridPicked, ref eventPoints);
		List<Grid> emptyPoints = gridPicked;

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

	List<Grid> RandomGridList(List<Grid> gridPool,int requestNum){
		List<Grid> gl = new List<Grid> ();
		List<Grid> gp = gridPool;
		for(int i=0;i<requestNum;i++){
			Grid g = RandomGrid (gp);
			gl.Add (g);
			gp.Remove (g);
		}
		return gl;
	}

	Grid RandomGrid(List<Grid> gridPool){
		int r = CalculateMethod.GetRandomValue (0, gridPool.Count);
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
			if (num == 1)
				ends.Add (orgs [i]);
			if (num == 0)
				Debug.Log ("Impossible!");
		}
		return ends;
	}

	private ArrayList openList;
	private ArrayList closeList;
	private string road;


	/// <summary>
	/// 简单的A星寻路算法,不走对角线
	/// </summary>
	/// <returns>The road.</returns>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	public void FindPath(int startX,int startY,int endX,int endY){

		openList = new ArrayList ();
		closeList = new ArrayList ();
		road = "";

		openList.Add (gridList [startX, startY]);
		Grid current = openList [0] as Grid;

		while (openList.Count > 0 && (startX != endX || startY != endY)) {
			current = openList [0] as Grid;
			if (current.x == endX && current.y == endY) {
				Debug.Log ("PathFound!");
				GenerateRoad(current);
				Debug.Log (road);
				return;
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
	}

	void GenerateRoad(Grid g){
		road += "(" + g.x + "," + g.y + ")";
		if (g.parent != null)
			GenerateRoad (g.parent);
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

}

class Grid : IComparable{
	public int x;
	public int y;
	public GridType type;

	public int g;//距离起点
	public int h;//距离终点
	public int f;//总值

	public Grid parent;

	public Grid(int x,int y){
		this.x = x;
		this.y = y;
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

}
	

enum GridType{
	Block = 99,
	Covered = 0,
	Road = 1,
	Normal = 2
}

