using UnityEngine;
using System.Collections;


enum CAMERA_MODE
{
	AUTO,
	AUTO_CONTROL,
	FULL_CONTROL
};

public class CameraSwitch : MonoBehaviour {

//	public Camera autoFollowCam;
//	public Camera roamingCam;
//	public Camera controllableFollowCam;
//	public static int activeIndex;
	// Use this for initialization

	public static int currentMode;


	void Start () {
//		autoFollowCam.enabled = true;
//		roamingCam.enabled = false;
//		controllableFollowCam.enabled = false;
		currentMode =(int) CAMERA_MODE.AUTO;
//		activeIndex = 0;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("ChangeCam") && (CAMERA_MODE)currentMode==CAMERA_MODE.AUTO) {
			currentMode=(int)CAMERA_MODE.AUTO_CONTROL;
		} else if (Input.GetButtonDown("ChangeCam") && (CAMERA_MODE)currentMode==CAMERA_MODE.AUTO_CONTROL) {
			currentMode=(int)CAMERA_MODE.FULL_CONTROL;
		} else if (Input.GetButtonDown("ChangeCam") && (CAMERA_MODE)currentMode==CAMERA_MODE.FULL_CONTROL) {
			currentMode=(int)CAMERA_MODE.AUTO;
		}
	}
}
