using UnityEngine;
using System.Collections;

public class AutoFollowCamera : MonoBehaviour {

	public float distanceAway;
	public float distanceUp;
	public Transform follow;
	public float smooth;

	private Vector3 targetPosition;
	private Camera camSelf;
	// Use this for initialization
	void Start () {

		//init camera variables
		distanceAway = 10.0f;
		distanceUp = 30.0f;
		smooth = 0.1f;
		camSelf = GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void LateUpdate()
	{
		if (camSelf.enabled) {
			targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
			transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * smooth);
			transform.LookAt (follow);
		}

	}
}
