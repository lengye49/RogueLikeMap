using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {

    public void OnClick(){
		string[] s = this.gameObject.name.Split (',');
		Debug.Log ("Targeting..." + this.gameObject.name);
		Grid g = new Grid (int.Parse (s [0]), int.Parse (s [1]));
		GetComponentInParent<PlayerController> ().TargetingGrid (g);
    }
}
