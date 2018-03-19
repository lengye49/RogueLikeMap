using UnityEngine;
using System.Collections;

public class ButtonOnCell : MonoBehaviour {

	public void SendButtonMessage()
	{
		string[] s = this.gameObject.name.Split(',');
		int i = int.Parse (s [0]);
		int j = int.Parse (s [1]);
		Initialize ini = this.gameObject.GetComponentInParent<Initialize>();
		ini.ClickCell(i,j);
	}
}
