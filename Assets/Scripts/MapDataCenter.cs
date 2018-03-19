using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapDataCenter{
	private int rowsCount;//thisMap.Rows
	private int columnsCount;//thisMap.Columns
	private Map thisMap;
	private Grid[,] gridList;
		
	//public MapDataCenter(Map m)
	public MapDataCenter(int row,int column){
		rowsCount = 10;
		columnsCount = 10;
		gridList = new Grid[row, column];
		InitMapData ();
	}

	void InitMapData(){
		int bossNum = CalculateMethod.GetRandomValue (thisMap.BossCount);
		int monsterNum = CalculateMethod.GetRandomValue (thisMap.MonsterCount);
		int eventNum = CalculateMethod.GetRandomValue (thisMap.EventCount);

	}
			
		//测试代码
//		for (int i = 0; i < rowsCount; i++) {
//			for (int j = 0; j < columnsCount; j++) {
//				GenerateNewGrid (i, j);
//			}
//		}
//	}
	//测试代码
//	void GenerateNewGrid(int x,int y){
//		gridList [x, y] = new Grid (x, y);
//		gridList [x, y].type = GridType.Normal;
//		if (x == 1 && y <4)
//			gridList [x, y].type = GridType.Block;
//		if(x==3 && y>0)
//			gridList [x, y].type = GridType.Block;
//		if(x==6 && y<4)
//			gridList [x, y].type = GridType.Block;
//		if(x==8 && y>4)
//			gridList [x, y].type = GridType.Block;
//	}


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

	void GenerateRoad(Grid g){
		road += "(" + g.x + "," + g.y + ")";
		if (g.parent != null)
			GenerateRoad (g.parent);
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

