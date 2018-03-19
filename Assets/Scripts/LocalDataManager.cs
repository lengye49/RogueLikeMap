using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour  {
	public static PlayerInfo _player;
	void Awake(){
		_player = new PlayerInfo ();
		LoadData ();
	}
		
	public static void SaveData(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create(Application.persistentDataPath+"/info.dat");
		bf.Serialize (file, _player);
		file.Close ();
	}

	void LoadData(){
		//		File.Delete (Application.persistentDataPath + "/info.dat");
		if (File.Exists (Application.persistentDataPath + "/info.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/info.dat", FileMode.Open);
			_player = (PlayerInfo)bf.Deserialize (file);
			file.Close ();
			Debug.Log ("加载成功！");
		} else {
			InitPlayerInfo ();
		}
	}

	void InitPlayerInfo(){
		_player.AccountId = 0;
		_player.Name = "无名氏";
		_player.Country = 0;
		_player.HighLevel = 1;
		_player.HighScore = 0;
	}
}


[System.Serializable]
public class PlayerInfo{
	public int AccountId;
	public string Name;
	public int Country;
	public int HighLevel;
	public int HighScore;
}
