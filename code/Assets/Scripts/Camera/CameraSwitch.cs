using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {

	public Camera autoFollowCam;
	public Camera roamingCam;
	public Camera controllableFollowCam;
	// Use this for initialization
	void Start () {
		autoFollowCam.enabled = true;
		roamingCam.enabled = false;
		controllableFollowCam.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.V) && autoFollowCam.enabled) {
			autoFollowCam.enabled=false;
			roamingCam.enabled=true;
		}
		else if(Input.GetKeyDown (KeyCode.V) && roamingCam.enabled)
		{
			autoFollowCam.enabled=true;
			roamingCam.enabled=false;
		}
	}
}
