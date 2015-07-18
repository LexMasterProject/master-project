using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {


	private RaycastHit hit;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics.Raycast (transform.position, Vector3.down, out hit, 1000)) {
			

			GameObject hitGameObject=hit.collider.gameObject;
			Debug.Log(hit.collider.gameObject.name);
			Debug.Log(hit.distance);
			Destroy(hitGameObject);
		}
	}
}
