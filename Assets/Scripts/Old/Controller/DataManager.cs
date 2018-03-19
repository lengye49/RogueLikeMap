using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour  {
	public static PlayerInfo _player;
	void Awake(){
		_player = new PlayerInfo ();
		_player.AccountId = 0;
		_player.Name = "无名氏";
		_player.Country = 0;
		_player.HighLevel = 1;
		_player.HighScore = 0;
		LoadData ();
	}

	#region 数据
	static int score=0;
	static int level=0;
    static string numList;

	public static int Score {
		get {
			return score;
		}
		set {
			score = value; 
		}
	}
	public static int Level {
		get {
			return level;
		}
		set {
			level = value; 
		}
	}

	public static string GetRankStr(int rank){
		string s = "";
		if (rank == 0) {
			if (level < 6)
				s = "籍籍无名！道友仍需努力！";
			else {
				//待处理
				s = "名满天下！超过了"+GetRankPercent()+"%的道友！";
			}
		} else {
			s="荣登仙榜！\n位列仙道第"+rank+"名！";
		}
		return s;
	}

	private static float GetRankPercent(){
		if (score < 1000)
			return 50.0f;
		else if (score > 9000)
			return 99.0f;
		else
			return 50.0f + (score - 1000f) * 0.005f;
	}

	public static int SetHighScore(){
		if ((level > _player.HighLevel) || (level == _player.HighLevel && score > _player.HighScore)) {
			_player.HighLevel = level;
			_player.HighScore = score;
			return 1;
		} else {
			return 0;
		}
	}
	#endregion

	#region 本地存储
	public static void SaveData(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create(Application.persistentDataPath+"/info.dat");

		bf.Serialize (file, _player);
		file.Close ();
//		Debug.Log ("存储成功！");
	}

	void LoadData(){
//		File.Delete (Application.persistentDataPath + "/info.dat");
		if (File.Exists (Application.persistentDataPath + "/info.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open(Application.persistentDataPath+"/info.dat",FileMode.Open);
			_player = (PlayerInfo)bf.Deserialize (file);
			file.Close ();
			Debug.Log ("加载成功！");
		}
	}
	#endregion

	#region 网络模块
	public static void Register(string playerName,int playerCountry){
		_player.Name = playerName;
		_player.Country = playerCountry;
		Client client = new Client ();
		client.GetRemoteService (RequestCode.Register,ActionCode.Register, "");
	}
		
	public static void SetOnlineRank(){
		Client client = new Client ();
		string msg = _player.AccountId + "," + _player.Name + "," + _player.Country + "," + _player.HighLevel + "," + _player.HighScore;
		client.GetRemoteService (RequestCode.Game,ActionCode.GetPersonalResult, msg);
	}
		

	public static void RequestTotalRank(){
		Client client = new Client ();
		client.GetRemoteService (RequestCode.Game, ActionCode.GetTotalRank, "");
	}
	
	public static void SetTotalRank(string str){
		RankManager.TopUserList = new User[100];
		string[] ss = str.Split(';');
		for (int i = 0; i < 100; i++) {
			string[] s = ss[i].Split(',');
			RankManager.TopUserList [i] = new User ();
			RankManager.TopUserList [ i].id = int.Parse(s[0]);
			RankManager.TopUserList [i].name = s[1];
			RankManager.TopUserList [ i].place = int.Parse(s[2]);
			RankManager.TopUserList [i].level = int.Parse(s[3]);
			RankManager.TopUserList [ i].score = int.Parse(s[4]);
		}

		RankManager.isRankReady = true;
	}
	#endregion
}


[System.Serializable]
public class PlayerInfo{
	public int AccountId;
	public string Name;
	public int Country;
	public int HighLevel;
	public int HighScore;
}
	