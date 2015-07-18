using UnityEngine;
using System.Collections;

public class ControllableFollowCam : MonoBehaviour {

	public Transform follow;
	public float dist;//distance between spider and camera
	public Vector3 directionSTC;//spider to camera
	public Vector3 targetPos;
	public float zoomSpeed=10;
	public float rotateSpeed;

	private Camera camSelf;
	// Use this for initialization
	void Start () {
		directionSTC = new Vector3 (0, 1, -1);
		directionSTC.Normalize ();
		dist = 50;
		zoomSpeed = 10;
		rotateSpeed = 30;

		camSelf = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (camSelf.enabled) {
			float distDiff = Input.GetAxis ("Zoom") * zoomSpeed * Time.deltaTime;
			dist-=distDiff;
			float leftRight= -Input.GetAxis("RotateLR")*rotateSpeed*Time.deltaTime;
			float upDown=Input.GetAxis("RotateUD")*rotateSpeed*Time.deltaTime;
			transform.RotateAround (follow.position, follow.up, leftRight);
			transform.RotateAround (follow.position, transform.right, upDown);
		}
	}

	void LateUpdate()
	{
		if (camSelf.enabled) {
			directionSTC = transform.position - follow.transform.position;
			directionSTC.Normalize ();
			targetPos = follow.position + directionSTC * dist;

			transform.position = targetPos;
			transform.LookAt (follow);
			//Debug.DrawLine (follow.position, targetPos, Color.green);
		}
	}
}
