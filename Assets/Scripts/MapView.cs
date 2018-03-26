﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapView : MonoBehaviour {

    private float mapWidth;
    private float mapHeight;
    private int mapRow;
    private int mapColumn;
    private float cellLength;
    private Vector3 cellScale;
    private Vector3 refPoint;

    void Start(){
        mapWidth = 1920;//width;
        mapHeight = 1080;//height;
        mapRow = 9;//rows;
        mapColumn = 15;//columns;
        cellLength = GetCellLength();
        cellScale = GetCellScale();
        this.refPoint = Vector3.zero;//refPoint;
        DeployMapView();
    }

//    public MapView(){
//    //(int width,int height,int rows,int columns,Vector3 refPoint){
//        mapWidth = 1920;//width;
//        mapHeight = 1080;//height;
//        mapRow = 9;//rows;
//        mapColumn = 15;//columns;
//        cellLength = GetCellLength();
//        cellScale = GetCellScale();
//        this.refPoint = Vector3.zero;//refPoint;
//        DeployMapView();
//    }

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
                c.name = i + "," + j;
            }
        }
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

}
