using UnityEngine;
using System.Collections;

public class ControllableFollowCam : MonoBehaviour {

	public Transform follow;
	public Vector3 lastPos;
	public Vector3 offset;
	public float distDiff;
	public float leftRight;
	public float upDown;
	public Vector3 directionSTC;//spider to camera
	public Vector3 targetPos;
	public float zoomSpeed=10;
	public float rotateSpeed;
	public bool flag;
	public float distConstraint;

	private Camera camSelf;
	// Use this for initialization
	void Start () {

		zoomSpeed = 30;
		rotateSpeed = 30;
		distConstraint = 12.5f;
		camSelf = GetComponent<Camera> ();
		offset = Vector3.zero;
		lastPos = follow.position;
		//Debug.Log ("lastPos:" + lastPos.ToString());

	}
	
	// Update is called once per frame
	void Update () {
		if (camSelf.enabled) {
			distDiff = Input.GetAxis ("Zoom") * zoomSpeed * Time.deltaTime;
			leftRight= -Input.GetAxis("RotateLR")*rotateSpeed*Time.deltaTime;
			upDown=-Input.GetAxis("RotateUD")*rotateSpeed*Time.deltaTime;
			offset= follow.position  -lastPos;
			lastPos=follow.position;

		}
	}

	void LateUpdate()
	{
		if (camSelf.enabled) {
			transform.RotateAround (follow.position, follow.up, leftRight);
			transform.RotateAround (follow.position, transform.right, upDown);
			//Debug.Log(distDiff);
			float dist=Vector3.Distance(transform.position,follow.position);
			//Debug.Log(dist);
			if(dist<distConstraint&& distDiff>0)
			{
				//no camera forward to spider
			}
			else
			{
				transform.position = Vector3.MoveTowards (transform.position, follow.position, distDiff);

			}
			transform.Translate(offset,Space.World);
			transform.LookAt (follow);
		}
	}
}
