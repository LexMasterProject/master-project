using UnityEngine;
using System.Collections;

public class ControllableFollowCam : MonoBehaviour {
	public float distanceAway;
	public float distanceUp;
	public Transform follow;
	// Use this for initialization
	void Start () {
		//init camera variables
		distanceAway = 10.0f;
		distanceUp = 30.0f;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void LateUpdate()
	{
		transform.position = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
		transform.LookAt (follow);
	}
}
