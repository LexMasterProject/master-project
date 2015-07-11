using UnityEngine;
using System.Collections;

public class CameraSwitch : MonoBehaviour {

	public Camera followCam;
	public Camera roamingCam;
	// Use this for initialization
	void Start () {
		followCam.enabled = true;
		roamingCam.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.V) && followCam.enabled) {
			followCam.enabled=false;
			roamingCam.enabled=true;
		}
		else if(Input.GetKeyDown (KeyCode.V) && roamingCam.enabled)
		{
			followCam.enabled=true;
			roamingCam.enabled=false;
		}
	}
}
