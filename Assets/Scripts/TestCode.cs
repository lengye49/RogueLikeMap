using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour {

	private MapDataCenter _map;
	// Use this for initialization
	void Start () {
		_map = new MapDataCenter (10, 10);
		_map.FindPath (0, 1, 9, 8);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
