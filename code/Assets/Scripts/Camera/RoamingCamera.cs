using UnityEngine;
using System.Collections;

public class RoamingCamera : MonoBehaviour {

	public float horizontalSpeed=40;
	public float verticalSpeed=40;
	public float camRotateSpeed=80;
	public float camDist=30;

	//internal check
	private float curDist;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		float horizontal = Input.GetAxis ("Horizontal") * horizontalSpeed * Time.deltaTime;
//		float vertical = Input.GetAxis ("Vertical") * verticalSpeed * Time.deltaTime;
//		float rotation= Input.GetAxis("Rotation");
//
//		transform.Translate (Vector3.forward*vertical,Space.World);
//		transform.Translate (Vector3.right * horizontal,Space.World);
//
//		if (rotation != 0) {
//			transform.Rotate(Vector3.up,rotation*camRotateSpeed*Time.deltaTime,Space.World);
//		}
//
//		RaycastHit hit;
//		if (Physics.Raycast (transform.position, -transform.up, out hit, 100)) {
//			curDist=Vector3.Distance(transform.position,hit.point);
//		}
//		if (curDist != camDist) {
//			float diff=camDist-curDist;
//			transform.position=Vector3.Lerp(transform.position,transform.position+Vector3.up*diff,Time.deltaTime);
//		}
	}
}
