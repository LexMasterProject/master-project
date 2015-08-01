using UnityEngine;
using System.Collections;

public class ControllableFollowCam : MonoBehaviour {

	public Transform follow;
	public Vector3 lastPos;
	public Vector3 offset;
	public float dist;//distance between spider and camera
	public float distDiff;
	public float leftRight;
	public float upDown;
	public Vector3 directionSTC;//spider to camera
	public Vector3 targetPos;
	public float zoomSpeed=10;
	public float rotateSpeed;
	public bool flag;

	private Camera camSelf;
	// Use this for initialization
	void Start () {
		directionSTC = new Vector3 (0, 1, -1);
		directionSTC.Normalize ();
		dist = 50;
		zoomSpeed = 30;
		rotateSpeed = 30;

		camSelf = GetComponent<Camera> ();
		offset = Vector3.zero;
		lastPos = follow.position;
		//Debug.Log ("lastPos:" + lastPos.ToString());

	}
	
	// Update is called once per frame
	void Update () {
		if (camSelf.enabled) {
			distDiff = Input.GetAxis ("Zoom") * zoomSpeed * Time.deltaTime;
			dist-=distDiff;
			leftRight= -Input.GetAxis("RotateLR")*rotateSpeed*Time.deltaTime;
			upDown=-Input.GetAxis("RotateUD")*rotateSpeed*Time.deltaTime;
		//	Debug.Log("pos:"+follow.position);
			offset= follow.position  -lastPos;
			lastPos=follow.position;

		
	


		}
	}

	void LateUpdate()
	{
		if (camSelf.enabled) {
			transform.RotateAround (follow.position, follow.up, leftRight);
			transform.RotateAround (follow.position, transform.right, upDown);
			transform.position = Vector3.MoveTowards (transform.position, follow.position, distDiff);
			transform.Translate(offset,Space.World);
//			directionSTC = transform.position - follow.transform.position;
//			directionSTC.Normalize ();
//			targetPos = follow.position + directionSTC * dist;
//
//			transform.position = targetPos;
			transform.LookAt (follow);
		}
	}
}
