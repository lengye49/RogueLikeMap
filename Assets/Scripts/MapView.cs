﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//这个脚本不能直接挂载，需要动态生成，因为会有多个地图同时生成。
//离开地图时需要销毁地图

public class MapView : MonoBehaviour {

    private Sprite[] typeSprite;
    private float mapWidth;
    private float mapHeight;
    private int mapRow;
    private int mapColumn;
    private float cellLength;
    private Vector3 cellScale;
    private Vector3 refPoint;
    private Grid[,] glist;
    private List<GameObject> cellList;

    public void Reset(Grid[,] glist,int rows,int columns){
        mapWidth = 1920;//width;
        mapHeight = 1080;//height;
        mapRow = rows;//rows;
        mapColumn = columns;//columns;
        cellLength = GetCellLength();
        cellScale = GetCellScale();
        this.refPoint = Vector3.zero;//refPoint;
        this.glist=glist;
        cellList = new List<GameObject>();
		typeSprite = GetComponentInParent<Configs> ().itemSpriteList;
        DeployMapView();
    }

    public void DeployMapView(){
        Object mapCell = Resources.Load("mapCell");
        Transform _parent = this.gameObject.transform;
        for (int i = 0; i < mapRow; i++)
        {
            for (int j = 0; j < mapColumn; j++)
            {
                GameObject c = Instantiate(mapCell) as GameObject;
                c.transform.SetParent(_parent);
                c.transform.localScale = cellScale;
                c.transform.localPosition = CalculatePos(i, j);
                c.name = j + "," + i;
                cellList.Add(c);
                ShowMapImage(c, 0);
            }
        }
    }

    void ShowMapImage(GameObject g, int imageIndex){
        g.GetComponent<Image>().sprite = typeSprite[imageIndex];
    }

    public void ChangeCellView(Grid g){
        int imageIndex = 0;
        if (g.isOpen)
            imageIndex = (int)(g.type);
        GameObject o =cellList[ g.x + mapColumn * g.y];
        ShowMapImage(o, imageIndex);
    }

    float GetCellLength(){
        float sideWidth = mapWidth / mapColumn;
        float sideHeight = mapHeight / mapRow;
		return Mathf.Min(sideWidth, sideHeight);
    }

    Vector3 GetCellScale(){
        return new Vector3(cellLength / 128f, cellLength / 128f, 1f);
    }

    Vector3 CalculatePos(int thisRow,int thisColumn){
        float x = refPoint.x;
        x += (thisColumn + 0.5f - 0.5f * mapColumn) * cellLength;
        float y = refPoint.y;
        y += (thisRow + 0.5f - 0.5f * mapRow) * cellLength;

        return new Vector3(x, y, 1f);
    }

	public Vector3 CalculatePos(Grid g){
		float x = refPoint.x;
		x += (g.x + 0.5f - 0.5f * mapColumn) * cellLength;
		float y = refPoint.y;
		y += (g.y + 0.5f - 0.5f * mapRow) * cellLength;

		return new Vector3(x, y, 1f);
	}

}
