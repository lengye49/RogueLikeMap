using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Register : MonoBehaviour {

	public Text textName;
	public Dropdown dropDownCountry;

	private string playerName;
	private int playerCountry;
	private Vector3 pos = new Vector3(0,-650,0);

	public void Confirm(){
		playerName = textName.text;

		char[] c = playerName.ToCharArray();
		for(int i=0;i<c.Length;i++){
			if(c[i]<0x4e00 || c[i]>0x9fbb){
				Warning.ShowShortWarning (2, "道号必须是中文！",pos);
				return;
			}	
		}

		if (playerName == "") {
			Warning.ShowShortWarning (2, "请问道友高姓大名？",pos);
			return;
		}

		if (playerName.Length > 5) {
			Warning.ShowShortWarning (2, "道号最多五个字！", pos);
			return;
		}

		playerCountry = dropDownCountry.value;
		if (playerCountry == 0) {
			Warning.ShowShortWarning (2, "请问道友仙乡何处？",pos);
			return;
		}
			
		DataManager.Register (playerName, playerCountry);
		this.GetComponentInParent<ViewManager>().GoToStartPanel();
		DataManager.SaveData ();
	}
}
