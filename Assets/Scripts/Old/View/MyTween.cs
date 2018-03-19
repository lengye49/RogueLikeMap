using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class MyTween:MonoBehaviour  {

	#region 滑进滑出效果
	private float slideTime = 0.5f;
	private Vector3 startPos = new Vector3 (-5000, 0, 0);
	private Vector3 endPos = new Vector3 (5000, 0, 0);

	public void SlideIn(Transform t){
		t.gameObject.SetActive (true);
		t.DOLocalMoveX (0, slideTime);
	}

	public void SlideOut(Transform t){
		if (isNear (startPos.x, t.localPosition.x))
			return;
		t.DOLocalMoveX (endPos.x, slideTime);
		StartCoroutine (SetInitPos (t));
	}
	IEnumerator SetInitPos(Transform t){
		yield return new WaitForSeconds (slideTime);
		t.localPosition = startPos;
		t.gameObject.SetActive (false);
	}
	bool isNear(float basicPos,float point){
		if (basicPos - point > -100 && basicPos - point < 100)
			return true;
		return false;
	}

	#endregion


	#region 缩放效果
	private float zoomTime = 0.5f;
	private Vector3 startScale = new Vector3(0.01f, 0.01f, 0.01f);
	private Vector3 scaleRate = Vector3.one;

	public void ZoomIn(Transform t){
		t.gameObject.SetActive (true);
		t.localPosition = Vector3.zero;
		t.localScale = startScale;
		t.DOBlendableScaleBy (scaleRate, zoomTime);
	}

	public void ZoomOut(Transform t){
		t.DOBlendableScaleBy (-scaleRate, zoomTime);
		StartCoroutine (SetInitScale (t));
	}
	IEnumerator SetInitScale(Transform t){
		yield return new WaitForSeconds (slideTime);
		t.localScale = startScale;
		t.gameObject.SetActive (false);
	}
	#endregion

	#region 冒泡效果
	private float popTime = 0.2f;
	private float disappearTime = 0.5f;
	private float yShiftPop = 100f;
	private float yShiftDisappear = 150f;
	public void PopIn(Transform t,float showTime = 1.3f){
		t.localScale = startScale;
		t.DOLocalMoveY (t.localPosition.y + yShiftPop, popTime);
		t.DOBlendableScaleBy (scaleRate, popTime);
		StartCoroutine(ShowTime(t,null,showTime));
	}

    public void PopIn(Transform t,Action process, float showTime=2.5f){
        t.localScale = startScale;
        t.DOLocalMoveY (t.localPosition.y + yShiftPop, popTime);
        t.DOBlendableScaleBy (scaleRate, popTime);
        StartCoroutine(ShowTime(t, process, showTime));
    }

    IEnumerator ShowTime(Transform t,Action process,float showTime){
		yield return new WaitForSeconds (showTime);
        if (process != null)
            process();
		Disappear (t);
	}
	void Disappear(Transform t){
		t.DOLocalMoveY (t.localPosition.y + yShiftDisappear, disappearTime);
		t.GetComponentInChildren<Text>().DOFade (0, disappearTime);
		StartCoroutine (DestroyTransform (t));
	}
	IEnumerator DestroyTransform(Transform t){
		yield return new WaitForSeconds (disappearTime);
		DestroyImmediate (t.gameObject);
	}
	#endregion

}
