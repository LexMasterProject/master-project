using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {

	public Camera autoFollowCam;
	public Camera roamingCam;
	public Camera controllableFollowCam;
	public static int activeIndex;
	// Use this for initialization
	void Start () {
		autoFollowCam.enabled = true;
		roamingCam.enabled = false;
		controllableFollowCam.enabled = false;
		activeIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.V) && autoFollowCam.enabled) {
			activeIndex=1;
			controllableFollowCam.enabled = true;
			autoFollowCam.enabled = false;
			roamingCam.enabled = false;
		} else if (Input.GetKeyDown (KeyCode.V) && controllableFollowCam.enabled) {
			activeIndex=2;
			roamingCam.enabled = true;
			controllableFollowCam.enabled = false;
			autoFollowCam.enabled = false;
		} else if (Input.GetKeyDown (KeyCode.V) && roamingCam.enabled) {
			activeIndex=0;
			autoFollowCam.enabled=true;
			roamingCam.enabled=false;
			controllableFollowCam.enabled=false;
		}
	}
}
