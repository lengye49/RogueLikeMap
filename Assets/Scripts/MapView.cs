using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//这个脚本不能直接挂载，需要动态生成，因为会有多个地图同时生成。

public class MapView : MonoBehaviour {

    public Sprite[] typeSprite;

    private float mapWidth;
    private float mapHeight;
    private int mapRow;
    private int mapColumn;
    private float cellLength;
    private Vector3 cellScale;
    private Vector3 refPoint;
    private Grid[,] glist;

//    void Start(){
//        mapWidth = 1920;//width;
//        mapHeight = 1080;//height;
//        mapRow = 9;//rows;
//        mapColumn = 15;//columns;
//        cellLength = GetCellLength();
//        cellScale = GetCellScale();
//        this.refPoint = Vector3.zero;//refPoint;
//        DeployMapView();
//    }

    public void Reset(Grid[,] glist,int rows,int columns){
    //(int width,int height,int rows,int columns,Vector3 refPoint){
        mapWidth = 1920;//width;
        mapHeight = 1080;//height;
        mapRow = rows;//rows;
        mapColumn = columns;//columns;
        cellLength = GetCellLength();
        cellScale = GetCellScale();
        this.refPoint = Vector3.zero;//refPoint;
        this.glist=glist;
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
                c.name = i + "," + j;
                c.GetComponent<Image>().sprite = typeSprite[(int)(glist[i, j].type)];
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

	public Vector3 CalculatePos(Grid g){
		float x = refPoint.x;
		x += (g.x + 0.5f - 0.5f * mapColumn) * cellLength;
		float y = refPoint.y;
		y += (g.y + 0.5f - 0.5f * mapRow) * cellLength;

		return new Vector3(x, y, 1f);
	}

}
