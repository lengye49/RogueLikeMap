using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdManager:MonoBehaviour
{
    public Button ShowAdBtn;
	string appId = "5a9746eb1aadac9e71006a08";
	string[] placementStr;
    
	void Start(){
		placementStr = new string[]{ "REWARDED-4680244" };
		Vungle.init (appId, placementStr);
		Vungle.onAdFinishedEvent += (placementId, args) =>{
			AdFinished(placementId,args);
		} ;
		Vungle.loadAd (placementStr [0]);
    }

    void Update(){
		if (Vungle.isAdvertAvailable (placementStr [0])) {
			ShowAdBtn.interactable = true;
			ShowAdBtn.gameObject.GetComponentInChildren<Text> ().color = Color.green;
		} else {
			ShowAdBtn.interactable = false;
			ShowAdBtn.gameObject.GetComponentInChildren<Text> ().color = Color.grey;
		}
    }

	public void OnShowAd(){
        HideAdNotice ();

		Dictionary<string, object> options = new Dictionary<string, object> ();
		options ["userTag"] = "广告";
		options ["alertTitle"] = "道友请注意!";
		options ["alertText"] = "如果广告未播放完，则无法获得回天之力！";
		options ["closeText"] = "关闭";
		options ["continueText"] = "继续观看";

		Vungle.playAd (options, placementStr [0]);
    }

	void AdFinished(string placementId, AdFinishedEventArgs args){
		if (args.WasCallToActionClicked || args.IsCompletedView) {
			Initialize _ini = GetComponentInChildren<Initialize> ();
			_ini.AddResetPoint ();
			_ini.Reset ();
		} else {
			GetComponentInChildren<Initialize> ().ConfirmComplete ();
		}
    }

    public void OnCancelAd(){
        HideAdNotice ();
        GetComponentInChildren<Initialize> ().ConfirmComplete ();
    }

    void HideAdNotice(){
        ViewManager _view = GetComponent<ViewManager>();
        _view.HideAdNotice ();
    }
}


