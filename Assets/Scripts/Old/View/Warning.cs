using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Warning :MonoBehaviour {


	/// <summary>
	/// 小提示.
	/// </summary>
	/// <param name="type">Type 0Black 1Green 2Red</param>
	/// <param name="content">Content.</param>
	public static void ShowShortWarning(int colorType,string content,Vector3 pos,bool shortTime=true){
		GameObject f;
		if (shortTime)
			f = Instantiate (Resources.Load ("shortwarning1"))as GameObject;
		else
			f = Instantiate (Resources.Load ("shortwarning2"))as GameObject;
		GameObject p = GameObject.Find ("Warnings");
		f.SetActive (true);
		f.transform.SetParent (p.transform);
		f.transform.localPosition = pos;
		Text t = f.GetComponentInChildren<Text> ();
		t.text = content;
		t.color = GetWarningColor (colorType);

		if (shortTime)
			GameObject.Find ("Canvas").GetComponent<MyTween> ().PopIn (f.transform);
		else
			GameObject.Find ("Canvas").GetComponent<MyTween> ().PopIn (f.transform, 5f);
	}

//	public static void ShowNewRank(string content){
//		GameObject f = Instantiate (Resources.Load ("rankWarning")) as GameObject;
//		GameObject p = GameObject.Find ("Warnings");
//		f.SetActive (true);
//		f.transform.SetParent (p.transform);
//		f.transform.localPosition = Vector3.zero;
//		Text t = f.GetComponentInChildren<Text> ();
//		t.text = content;
//		t.color = Color.green;
//
//		GameObject.Find("Canvas").GetComponent<MyTween> ().ZoomIn (f.transform);
//	}

    public static void ShowResetWarning(Action process){
		GameObject f = Instantiate (Resources.Load ("shortwarning2")) as GameObject;
		GameObject p = GameObject.Find ("Warnings");
		f.SetActive (true);
		f.transform.SetParent (p.transform);
		f.transform.localPosition = Vector3.zero;
		Text t = f.GetComponentInChildren<Text> ();
		t.text = "当前无法进阶！\n自动使用回天之力...";
		t.color = Color.green;
		GameObject.Find ("Canvas").GetComponent<MyTween> ().PopIn (f.transform, process);
	}

	static Color GetWarningColor(int colorType){
		Color c = Color.black;
		if (colorType == 0)
			c = Color.black;
		else if (colorType == 1)
			c = Color.green;
		else if (colorType == 2)
			c = Color.red;

		return c;
	}

		
}
