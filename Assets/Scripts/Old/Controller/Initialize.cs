using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Initialize : MonoBehaviour {

	private ViewManager _view;
	private Object o;
	private GameObject[][] cells = new GameObject[5][];
	private int[][] nums = new int[5][];
	private bool[][] allCellsCheck = new bool[5][];
	private Vector3 offsetPos = new Vector3 (0, -186, 0);

	private ArrayList sameNumIndex;
	private ArrayList unCheckNeighbour;
	private ArrayList listCells;
	private int seed;
	private int midNum = 2;
	private int totalNum;
	private int score = 0;
	private PlayMusic _playerMusic;
	private int maxLv;

    public static bool hasResetEnergy = false;
	private bool isAdDone = false;

	void Start () {
		_view = GetComponentInParent<ViewManager> ();
		_playerMusic = this.gameObject.GetComponentInParent<PlayMusic> ();
		o = Resources.Load("Cell");
		listCells = new ArrayList ();
	}

	public void OnGameStart(){
		InitData ();
		StartGame ();
	}

	void StartGame(){
		for (int i = 0; i < listCells.Count; i++) {
			DestroyImmediate (listCells [i] as GameObject);
		}
		_playerMusic.PlayBg ("playPanelBg");
		_view.GoToGamePanel ();
		_view.SetScore (score);
		_view.SetGrade (Configs.LevelList [maxLv-1]);
        ClearResetPoint();
		isAdDone = false;
		InitCell ();
		DataManager.SaveData ();
	}
		
	void InitData(){

		score = 0;
		maxLv = 1;

		nums [0] = new int[3];
		nums [1] = new int[4];
		nums [2] = new int[5];
		nums [3] = new int[4];
		nums [4] = new int[3];

		allCellsCheck [0] = new bool[3];
		allCellsCheck [1] = new bool[4];
		allCellsCheck [2] = new bool[5];
		allCellsCheck [3] = new bool[4];
		allCellsCheck [4] = new bool[3];

		for (int i = 0; i < nums.Length; i++) {
			for (int j = 0; j < nums [i].Length; j++) {
				int n = Calculation.GetMyRandomForSeed (0, 3);
				int num = (int)Mathf.Pow (3, n);
				nums [i] [j] = num;
				maxLv = (maxLv > n + 1) ? maxLv : n + 1;
			}
		}
		StoreData ();
		_view.Upgrade (maxLv);
	}
		
    public void InitCell()
    {
        cells[0] = new GameObject[3];
        cells[1] = new GameObject[4];
        cells[2] = new GameObject[5];
        cells[3] = new GameObject[4];
        cells[4] = new GameObject[3];

        for (int i=0; i<cells.Length; i++) {
            for (int j=0; j<cells[i].Length; j++) {
                GameObject g = Instantiate (o) as GameObject;
                g.gameObject.transform.SetParent (this.gameObject.transform);
                g.transform.localPosition = offsetPos+ new Vector3 ((float)(j - (0.5 * cells [i].Length - 0.5)) * 172.1f, (midNum - i) * 138f, 0f);
                g.name = i.ToString () + "," + j.ToString ();
                cells [i] [j] = g;
				g.GetComponent<Image>().color = _view.GetCellColor(nums[i][j]);
                string s = _view.GetGradeByScore (nums[i][j]);
                g.GetComponentInChildren<Text> ().text = s;
				g.GetComponentInChildren<Text> ().color = _view.GetTextColor (nums [i] [j]);
				listCells.Add (g);
            }
        }
    }

    public void ClickCell(int row,int column)
    {
		CheckThisCell (row, column);
		if (totalNum >= 3) {
			_playerMusic.PlayerSound ("success");
			Calculate (row, column);
			CheckGameOver ();
		} else {
			_playerMusic.PlayerSound ("click");
		}
    }

    void CheckThisCell(int row,int column)
    {
        ResetArrays ();
        allCellsCheck [row] [column] = true;
        unCheckNeighbour.Add (new int[]{row,column});
        seed = nums [row] [column];
        totalNum = 1;

        while (unCheckNeighbour.Count>0) {
            int[] ins = unCheckNeighbour[0] as int[];
            CheckNeighbour(ins[0],ins[1]);
            unCheckNeighbour.RemoveAt(unCheckNeighbour.IndexOf(ins));
        }
    }
        
    void StoreData(){
        DataManager.Score = score;
        DataManager.Level = maxLv;
    }

	void CheckGameOver()
	{
		StoreData();
        if (maxLv > 10)
        {
            //游戏通关
            GameWin();
        }
		else if(CheckBlocked())
        {
            if (hasResetEnergy)
            {
                Reset();
            }
            else
            {
                //游戏失败
                GameFail();
            }
        }
	}

    bool CheckBlocked(){
        for (int i = 0; i < cells.Length; i++) {
            for (int j = 0; j < cells [i].Length; j++) {
                CheckThisCell (i, j);
                if (totalNum >= 3)
                {
                    return false;
                }
            }
        }
        return true;
    }



	void SetCell(int row,int column,int newSeed){
		nums [row] [column] = newSeed;
		cells [row] [column].gameObject.GetComponent<Image> ().color = _view.GetCellColor(newSeed);
		string s = _view.GetGradeByScore (newSeed);
		cells[row][column].gameObject.GetComponentInChildren<Text>().text = s;
		cells[row][column].gameObject.GetComponentInChildren<Text>().color = _view.GetTextColor (newSeed);
	}

	void GenerateNewCell(int row,int column){
		
		int max = Mathf.Max (maxLv - 4, 3);
		int min = Mathf.Max (maxLv - 8, 0);

        int n = Calculation.GetMyRandomForSeed(min, max);
		if (maxLv < n + 1) {
			maxLv = (n + 1);
			_view.Upgrade (maxLv);
			if (maxLv >= 5) {
                AddResetPoint();
			}
		}
		int newSeed = (int)Mathf.Pow (3,n);
		SetCell (row, column, newSeed);
	}

	void Calculate(int row, int column)
	{
		if (totalNum >= 3) {
			
			score += seed * totalNum;
			string msg = "法力 +" + seed * totalNum;

			Warning.ShowShortWarning (1, msg, new Vector3 (-300, 180, 0));

			int newN = (int)(Mathf.Log (totalNum, 3f));

			if ((maxLv < (newN + 1 + (int)(Mathf.Log (seed, 3f))))) {
				maxLv = (newN + 1+(int)(Mathf.Log(seed,3f)));
				_view.Upgrade (maxLv);
				if (maxLv >= Configs.GetResetPointLevel) {
                    AddResetPoint();
				}
			}

			int newSeed = seed * (int)Mathf.Pow (3, newN);
			SetCell (row, column, newSeed);

			for(int i=0;i<sameNumIndex.Count;i++)
			{
				int[] ins = sameNumIndex[i] as int[];
				GenerateNewCell (ins [0], ins [1]);
			}
			_view.SetScore (score);
			_view.SetGrade (Configs.LevelList [maxLv-1]);
		}
	}
		

	void ResetArrays()
	{
		sameNumIndex = new ArrayList ();

		for (int i=0; i<allCellsCheck.Length; i++) {
			for (int j=0; j<allCellsCheck[i].Length; j++) {
				allCellsCheck[i][j]=false;
			}
		}
		unCheckNeighbour = new ArrayList ();
	}

	void CheckNeighbour(int row, int column)
	{
		ArrayList ns = GetNeighbourhood (row, column);
		for (int i=0; i<ns.Count; i++) {
			int[] ins = ns[i] as int[];
			if(nums[ins[0]][ins[1]] == seed)
			{
				sameNumIndex.Add(ins);
				unCheckNeighbour.Add (ins);
				totalNum++;
			}
			allCellsCheck[ins[0]][ins[1]]=true;
		}
	}

	ArrayList GetNeighbourhood(int row, int column)
	{
		ArrayList ns = new ArrayList ();

		if (row < midNum) {
				ns.Add (new int[]{row - 1,column - 1});
				ns.Add (new int[]{row - 1,column });
				ns.Add (new int[]{row ,column - 1});
				ns.Add (new int[]{row ,column + 1});
				ns.Add (new int[]{row + 1,column });
				ns.Add (new int[]{row + 1,column + 1});
		} else if (row == midNum) {
				ns.Add (new int[]{row - 1,column - 1});
				ns.Add (new int[]{row - 1,column });
				ns.Add (new int[]{row ,column - 1});
				ns.Add (new int[]{row ,column + 1});
				ns.Add (new int[]{row + 1,column - 1 });
				ns.Add (new int[]{row + 1,column });
		} else {
				ns.Add (new int[]{row - 1,column });
				ns.Add (new int[]{row - 1,column + 1});
				ns.Add (new int[]{row ,column - 1});
				ns.Add (new int[]{row ,column + 1});
				ns.Add (new int[]{row + 1,column - 1 });
				ns.Add (new int[]{row + 1,column });
		}

		for (int i=ns.Count-1; i>=0; i--) {
			int[] a = ns [i] as int[];
			if (a [0] >= allCellsCheck.Length || a [0] < 0 || a [1] >= allCellsCheck [a [0]].Length || a [1] < 0 || allCellsCheck [a [0]] [a [1]])
				ns.Remove (ns [i]);
		}
		return ns;
	}
        
	public void Reset(){
        if (!hasResetEnergy)
            return;
		Warning.ShowResetWarning(ResetAllSmallNums);
        ClearResetPoint();
	}

    void ResetAllSmallNums(){
        while (CheckBlocked())
        {
            int min = Calculation.ArrayMin(nums);
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = 0; j < nums[i].Length; j++)
                {
                    if (nums[i][j] == min)
                        GenerateNewCell(i, j);
                }
            }
        }
    }

    public void AddResetPoint(){
        hasResetEnergy = true;
        _view.SetResetState(1);     
    }

    void ClearResetPoint(){
        hasResetEnergy = false;
        _view.SetResetState(0);
    }

    void GameWin(){
        _playerMusic.PlayerSound ("win");
        ConfirmComplete();
    }
    void GameFail(){
        _playerMusic.PlayerSound ("fail");

        if (isAdDone) {
            ConfirmComplete ();
        } else {
            _view.ShowAdNotce ();
            isAdDone = true;
        }
    }

	public void ConfirmComplete(){
        SettleRank();
		DataManager.SaveData ();
        _view.CallInComplete (maxLv,score);
	}

    void SettleRank(){
        int localRank = DataManager.SetHighScore ();

        if (localRank > 0)
        {
            DataManager.SetOnlineRank();
            _view.UpdateLocalRank();
        }
    }
}
