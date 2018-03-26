using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCell : MonoBehaviour {

    public void OnClick(){
        Debug.Log(this.gameObject.name);
    }
}
