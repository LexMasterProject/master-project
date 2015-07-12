﻿using UnityEngine;
using System.Collections;

public class ControllableFollowCam : MonoBehaviour {

	public Transform follow;
	public float dist;//distance between spider and camera
	public Vector3 directionSTC;//spider to camera
	public Vector3 targetPos;
	public float zoomSpeed;
	public float rotateSpeed;


	// Use this for initialization
	void Start () {
		directionSTC = new Vector3 (0, 1, -1);
		directionSTC.Normalize ();
		dist = 30;
		zoomSpeed = 10;
		rotateSpeed = 30;
	}
	
	// Update is called once per frame
	void Update () {
		if (enabled) {
			float distDiff = Input.GetAxis ("Vertical") * zoomSpeed * Time.deltaTime;
			dist-=distDiff;
			float leftRight= -Input.GetAxis("RotateLR")*rotateSpeed*Time.deltaTime;
			float upDown=Input.GetAxis("RotateUD")*rotateSpeed*Time.deltaTime;
			transform.RotateAround (follow.position, follow.up, leftRight);
			transform.RotateAround (follow.position, follow.right, upDown);


		}
	}

	void LateUpdate()
	{

		directionSTC = transform.position - follow.transform.position;
		directionSTC.Normalize ();
		targetPos= follow.position + directionSTC * dist;

		transform.position = targetPos;
		transform.LookAt (follow);
		Debug.DrawLine (follow.position, targetPos, Color.green);
	}
}
