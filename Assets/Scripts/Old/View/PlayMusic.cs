using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayMusic : MonoBehaviour {

	private float bgVolumn = 1.0f;
	private float soundVolumn = 1.0f;
	private AudioSource bgNow;

	public GameObject startMenuBgContainer;
	public GameObject playPanelBgContainer;
	public GameObject failSoundContainer;
	public GameObject clickSoundContainer;
	public GameObject successSoundContainer;
	public GameObject winSoundContainer;


	private AudioSource startMenuBg;
	private AudioSource playPanelBg;
	private AudioSource failSound;
	private AudioSource clickSound;
	private AudioSource successSound;
	private AudioSource winSound;

	void Awake () {
		startMenuBg = startMenuBgContainer.GetComponent<AudioSource> ();
		playPanelBg = playPanelBgContainer.GetComponent<AudioSource> ();
		failSound = failSoundContainer.GetComponent<AudioSource> ();
		clickSound = clickSoundContainer.GetComponent<AudioSource> ();
		successSound = successSoundContainer.GetComponent<AudioSource> ();
		winSound = winSoundContainer.GetComponent<AudioSource> ();
	}
		

	public void PlayBg(string bgName)
	{
		if (bgName == "startMenuBg" && bgNow!=startMenuBg) {
			startMenuBg.volume = bgVolumn;
			startMenuBg.Play ();
			bgNow = startMenuBg;
			playPanelBg.Stop ();
		} else if (bgName == "playPanelBg" && bgNow!=playPanelBg) {
			playPanelBg.volume = bgVolumn;
			playPanelBg.Play ();
			bgNow = playPanelBg;
			startMenuBg.Stop ();
		} 
	}

	public void PlayerSound(string soundName){
		if (soundName == "fail") {
			failSound.volume = 0.2f;
			failSound.Play ();
			playPanelBg.Stop ();
		} else if (soundName == "success") {
			successSound.volume = 0.1f;
			successSound.Play ();
		} else if (soundName == "click") {
			clickSound.volume = 0.2f;
			clickSound.Play ();
		} else if(soundName == "win"){
			clickSound.volume = soundVolumn;
			winSound.Play ();
		}
	}

}
