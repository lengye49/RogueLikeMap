using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour {

	public Transform RankContainer;
	public static bool isRankReady = false;
	public static User[] TopUserList = new User[100];
    private ArrayList cellList;

    void Start(){
        cellList = new ArrayList();
    }

	void Update () {
		if (isRankReady) {
			isRankReady = false;
			ShowTotalRank ();
		}
	}

	void ShowTotalRank(){
		for (int i = 0; i < TopUserList.Length; i++) {
			User u = TopUserList [i];
			GameObject f = Instantiate (Resources.Load ("rankCell")) as GameObject;
            cellList.Add(f);
			f.SetActive (true);
			f.transform.SetParent (RankContainer);
			Text[] t = f.GetComponentsInChildren<Text> ();
			t [0].text = (i + 1).ToString ();
			t [1].text = u.name;
			t [2].text = Configs.PlaceList [u.place];
			t [3].text = Configs.LevelList [u.level-1];
			t [4].text = u.score.ToString ();
			Color c = CellColor (u.id, i + 1);
			for (int j = 0; j < 5; j++)
				t [j].color = c;
		}
		RankContainer.GetComponent<RectTransform> ().sizeDelta = new Vector2 (980, 8500);
	}

	Color CellColor(int uId,int rank){
		if (uId == DataManager._player.AccountId)
			return new Color (255f / 255f, 125f / 255f, 0f);
		if (rank == 1)
			return Color.red;
		else if (rank < 4)
			return Color.cyan;
		else if (rank < 10)
			return Color.yellow;
		else
			return Color.green;
	}

    public void CloseRank(){
        foreach (GameObject o in cellList)
        {
			DestroyImmediate(o);
        }
		cellList.Clear ();
    }



}
