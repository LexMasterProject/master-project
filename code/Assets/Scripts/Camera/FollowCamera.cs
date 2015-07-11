using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public float distanceAway;
	public float distanceUp;
	public float smooth;
	public Transform follow;

	private Vector3 targetPosition;

	// Use this for initialization
	void Start () {

		//init camera variables
		distanceAway = 10.0f;
		distanceUp = 30.0f;
		smooth = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void LateUpdate()
	{
		targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;

		Debug.DrawRay (follow.position, Vector3.up * distanceUp, Color.red);
		Debug.DrawRay (follow.position,-1f*follow.forward*distanceAway,Color.blue);
		Debug.DrawLine (follow.position, targetPosition, Color.green);

		transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * smooth);
		transform.LookAt (follow);
	}
}
