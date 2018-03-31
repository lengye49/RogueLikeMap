using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

    //用单例写
	private Transform _player;
	private Grid _thisGrid;
	private CharacterAnimation _animation;

	private MapView _view;
	private MapDataCenter _data;
	private float _interval = 0.5f;

    private GameObject _mask;
    private bool IsCover = false;

//	private float t = 0f;
//	private Vector3 direction = Vector3.zero;

	List<Grid> _path;

	void Start () {
		_data = new MapDataCenter ();

		//需要把map做成prefab，这里new一个新的出来。这样以后有新地图就可以直接new map
		_view = GetComponentInChildren<MapView>();
		_view.Reset(_data.gridList,_data.Rows,_data.Columns);

		_thisGrid = _data.enterGrid;
		_player = GameObject.FindGameObjectWithTag ("Player").transform;
		_animation = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterAnimation> ();
        _mask = GameObject.FindGameObjectWithTag("Mask");
        Debug.Log(_mask.name);
        SetPosition(_thisGrid);
	}
		
	void SetPosition(Grid g){
        g.Print();
        OpenCell(g);
        List<Grid> nb = _data.GridNeighbour(g);
        for (int i = 0; i < nb.Count; i++)
            OpenCell(nb[i]);

        Debug.Log("*************" + _player.localPosition);
		Vector3 pos = _view.CalculatePos (_thisGrid);
        _player.DOLocalMove(pos, 0.01f, true);
//        _player.localPosition = pos;
        Debug.Log("*************" + _player.localPosition);
	}

    void OpenCell(Grid g){
        if (!g.isOpen)
        {
            g.isOpen = true;
            _view.ChangeCellView(g);
        }
    }

    void Update(){
        if (!IsCover && _mask.activeSelf)
        {
            _mask.SetActive(false);
        }
        else if (IsCover && !_mask.activeSelf)
        {
            _mask.SetActive(true);
        }

    }

	public void TargetingGrid(Grid target){
        //if isOpen=false, don't respond
        //if isWalkable=false，直接走，在movingByPath里面判断isWalkable,如果=false直接停止

        Debug.Log("Looking for Path...");
        _path = new List<Grid>();
		_path = _data.FindPath (target.x, target.y, _thisGrid.x, _thisGrid.y);
		if (_path.Count <= 0)
			return;
        Debug.Log("Moving...");
		MovingByPath ();
        IsCover = true;
	}
		

	void MovingByPath(){
		if (_path.Count == 1) {
			_animation.Rest ();//需要根据方向判断朝向,用grid.parent和grid来判断方向
            IsCover=false;
			return;
		}
        Debug.Log("Moving to ...");
		Grid nextGrid = _path [1];
        nextGrid.Print();
		_path.RemoveAt (1);

        Vector3 nextPos = _view.CalculatePos(nextGrid);
        Debug.Log(nextPos);
        _player.DOLocalMove(nextPos, _interval, false);
		_animation.Run ();//需要根据方向判断朝向,用grid.parent和grid来判断方向

		_thisGrid = nextGrid;
        StartCoroutine(MoveNext(_interval));
	}

	IEnumerator MoveNext(float duaration){
		yield return new WaitForSeconds (duaration);
		MovingByPath ();
	}
}
