using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {
	private Transform _player;
	private Grid _thisGrid;
	private CharacterAnimation _animation;

	private MapView _view;
	private MapDataCenter _data;
	private float _interval = 0.5f;

//	private float t = 0f;
//	private Vector3 direction = Vector3.zero;

	List<Grid> _path;

	void Start () {
		_data = new MapDataCenter ();
		_view = GetComponentInChildren<MapView>();
		_view.Reset(_data.gridList,_data.Rows,_data.Columns);

		_thisGrid = _data.enterGrid;
		_player = GameObject.FindGameObjectWithTag ("Player").transform;
		_animation = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterAnimation> ();
	}
		
	void SetPosition(Grid g){
		g.Print ();
		Vector3 pos = _view.CalculatePos (_thisGrid);
		_player.localPosition = pos;
	}

	public void MoveTo(Grid target){
		_path = _data.FindPath (target.x, target.y, _thisGrid.x, _thisGrid.y);
		if (_path.Count <= 0)
			return;
		MovingByPath ();
	}
		

	void MovingByPath(){
		if (_path.Count == 0) {
			_animation.Rest ();//需要根据方向判断朝向,用grid.parent和grid来判断方向
			return;
		}
		Grid nextGrid = _path [0];
		_path.RemoveAt (0);

		_player.DOLocalMove (_view.CalculatePos (nextGrid), _interval, false);
		_animation.Run ();//需要根据方向判断朝向,用grid.parent和grid来判断方向

		_thisGrid = nextGrid;
		MoveNext (_interval);
	}

	IEnumerator MoveNext(float duaration){
		yield return new WaitForSeconds (duaration);
		MovingByPath ();
	}
}
