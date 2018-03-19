using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ViewManager : MonoBehaviour {

    //界面
    public Transform startPanel;
	public Transform rankPanel;
	public Transform playPanel;

	public Transform registerPanel;
	private Vector3 startPos = new Vector3 (-5000, 0, 0);

    //游戏界面
    public Text scoreText;
    public Text gradeText;
    public Text resetPointText;
	public Sprite[] cells;
	public Sprite[] resetBg;

    //音乐
    private PlayMusic _playMusic;

    //结算
    public GameObject CompleteCover;
    public Text CompleteText;
	public GameObject NoticeCover;

	//本地排名积分
	public Text localScore;

	//动画
	private MyTween myTween;

	//网络加载
	public static bool isLoading = false;
	public GameObject loadingCover;
	public GameObject loadingImage;

    void Start(){
		registerPanel.localPosition = startPos;
		startPanel.localPosition = startPos;
		playPanel.localPosition = startPos;
		rankPanel.localPosition = startPos;
		_playMusic = GetComponentInParent<PlayMusic> ();
		UpdateLocalRank ();

		CompleteCover.gameObject.SetActive (false);
		myTween = GetComponent<MyTween> ();

		if (DataManager._player.AccountId > 0) {
			GoToStartPanel ();
		} else {
			GoToRegisterPanel ();
		}
    }
		
	void Update(){
		if ((!isLoading && loadingCover.activeSelf)) {
			loadingCover.SetActive (false);
		}
		if (isLoading ) {
			if (!loadingCover.activeSelf)
				loadingCover.SetActive (true);
			loadingImage.transform.Rotate (Vector3.back);
		}
	}

	public void GoToStartPanel(){
		myTween.SlideIn (startPanel);
		myTween.SlideOut (registerPanel);
		myTween.SlideOut (playPanel);

		_playMusic.PlayBg ("startMenuBg");
	}

	public void GoToRankPanel(){
		myTween.SlideIn (rankPanel);
		myTween.SlideOut (startPanel);
		DataManager.RequestTotalRank ();
	}

	public void OnRankReturnButton(){
		myTween.SlideIn (startPanel);
		myTween.SlideOut (rankPanel);
		rankPanel.GetComponent<RankManager>().CloseRank();
	}

	public void GoToRegisterPanel(){
		myTween.SlideIn (registerPanel);
		_playMusic.PlayBg ("startMenuBg");
	}

	public void OnCompleteReturnButton()
	{
		myTween.SlideIn (startPanel);
		myTween.SlideOut (playPanel);
		CompleteCover.gameObject.SetActive (false);
	}

	public void GoToGamePanel(){

		myTween.SlideIn (playPanel);
		myTween.SlideOut (startPanel);
		//设置游戏面板
		CompleteCover.gameObject.SetActive (false);
	}

	public void SetScore(int score){
		scoreText.text = score.ToString();
	}

	public void SetGrade(string grade){
		gradeText.text = grade;
	}

    public void CallInComplete(int maxLv,int score){
		CompleteCover.gameObject.SetActive (true);
        string msg="";
        if (maxLv >= Configs.LevelList.Length - 1)
            msg = "恭喜道友荣升真仙，从此\n超脱轮回，\n不死不灭，\n法力无边，\n万古长存！";
        else
            msg = "仙路渺茫，\n道友修为止于" + Configs.LevelList[maxLv-1] + ",\n最终法力达" + score + "。";
        CompleteText.text = msg;
	}

	public void ShowAdNotce(){
		NoticeCover.gameObject.SetActive (true);
	}

	public void HideAdNotice(){
		NoticeCover.gameObject.SetActive (false);
	}

	public void UpdateLocalRank(){
		int s = DataManager._player.HighScore;
		string g = Configs.LevelList [DataManager._player.HighLevel-1];

		if (s > 0)
			localScore.text = "个人最高道行：" + g + "(法力" + s + ")";
		else
			localScore.text = "";	
	}

    public void SetResetState(int num){
        resetPointText.text = num.ToString();
	}
        

	public void Upgrade(int newLevel){
		if (newLevel < Configs.ShowUpgradeLevel)
			return;
		string msg = "";
		if (newLevel >= Configs.GetResetPointLevel) {
			msg = "恭喜您突破到" + Configs.LevelList [newLevel-1] + ",获得1点回天之力！";
		} else {
			msg = "恭喜您突破到" + Configs.LevelList [newLevel-1] + "！";
		}
		Warning.ShowShortWarning (2, msg, new Vector3 (0, 180, 0),false);
	}

	
		
	#region 单元格属性
	public Sprite GetCellSprite(int num){
		switch (num) {
			case 1:
				return cells[0]; 
			case 3:
				return cells[1]; 
			case 9:
				return cells[2]; 	
			case 27:
				return cells[3];	
			case 81:
				return cells[4]; 
			case 243:
				return cells[5];
			case 729:
				return cells[6];
			case 2187:
				return cells[7];
			case 6561:
				return cells[8];
			case 19683:
				return cells [9];
			default:
				return cells[10];
		}
	}

	public Color GetCellColor(int num){
		switch (num) {
			case 1:
				return new Color (255f / 255f, 255f / 255f, 255f / 255f);
			case 3:
				return new Color (210f / 255f, 243f / 255f, 138f / 255f);
			case 9:
				return new Color (103f / 255f, 204f / 255f, 50f / 255f);
			case 27:
				return new Color (255f / 255f, 216f / 255f, 0f / 255f);
			case 81:
				return new Color (54f / 255f, 141f / 255f, 234f / 255f);
			case 243:
				return new Color (153f / 255f, 51f / 255f, 153f / 255f);
			case 729:
				return new Color (255f / 255f, 102f / 255f, 0f / 255f);
			case 2187:
				return new Color (210f / 255f, 31f / 255f, 217f / 255f);
			case 6561:
				return new Color (0f / 255f, 255f / 255f, 255f / 255f);
			case 19683:
				return new Color (255f / 255f, 56f / 255f, 104f / 255f);
			default:
				return Color.red;
		}
	}

	public Color GetTextColor(int num)
	{
		if (num > 27 && num <= 2187)
			return Color.white;
		return Color.black;
//		switch (num) {
//			case 1:
//				return new Color (0f / 255f, 160f / 255f, 233f / 255f);
//			case 3:
//				return new Color (250f / 255f, 205f / 255f, 137f / 255f);
//			case 9:
//				return new Color (128f / 255f, 194f / 255f, 105f / 255f);
//			case 27:
//				return new Color (255f / 255f, 247f / 255f, 153f / 255f);
//			case 81:
//				return new Color (236f / 255f, 105f / 255f, 65f / 255f);
//			case 243:
//				return new Color (0f / 255f, 255f / 255f, 255f / 255f);
//			case 729:
//				return new Color (0f / 255f, 255f / 255f, 0f / 255f);
//			case 2187:
//				return new Color (255f / 255f, 255f / 255f, 0f / 255f);
//			case 6561:
//				return new Color (255f / 255f, 0f / 255f, 0f / 255f);
//			case 19683:
//				return new Color (255f / 255f, 255f / 255f, 255f / 255f);
//			default:
//				return Color.white;
//		}
	}

	public string GetGradeByScore(int score){
		for (int i = 0; i <= 10; i++) {
			if (Mathf.Pow (3, i) == score)
				return Configs.LevelList [i];
		}
		return Configs.LevelList [0];
	}
	#endregion
}
