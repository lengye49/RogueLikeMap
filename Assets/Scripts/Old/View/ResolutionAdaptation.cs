using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResolutionAdaptation : MonoBehaviour {

	private float standard_width = 1080f;
	private float standard_height = 1920f;
	private float device_width = 0f;
	private float device_height = 0f;
	private float adjustor = 0f;

	void Start () {
		device_width = Screen.width;
		device_height = Screen.height;

		float standard_aspect = standard_width / standard_height;
		float device_aspect = device_width / device_height;

		if (device_width < standard_aspect)
			adjustor = standard_aspect / device_aspect;

		CanvasScaler canvasScalerTemp = transform.GetComponent<CanvasScaler>();  
		if (adjustor == 0)  
		{  
			canvasScalerTemp.matchWidthOrHeight = 1;  
		}  
		else  
		{  
			canvasScalerTemp.matchWidthOrHeight = 0;  
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
