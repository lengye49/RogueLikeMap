using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour {

	private MapDataCenter _data;
    private MapView _view;
	// Use this for initialization
	void Start () {
        _data = new MapDataCenter ();
        _view = GetComponentInChildren<MapView>();
        _view.Reset(_data.gridList,_data.Rows,_data.Columns);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
