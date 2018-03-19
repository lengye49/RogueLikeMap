using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWarning : MonoBehaviour {

	public static bool StartShowWarning = false;
	public static string Msg = "";

	void Start () {
		StartShowWarning = false;
	}
	

	void Update () {
		if (StartShowWarning) {
			ShowWarning ();
			StartShowWarning = false;
		}
	}

	void ShowWarning(){
		Warning.ShowShortWarning (2, Msg, Vector3.zero, false);
	}
}
