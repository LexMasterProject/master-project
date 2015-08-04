using UnityEngine;
using System.Collections;

public class IKHandling : MonoBehaviour {

	public Transform t1;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("here");
	}

	void LateUpdate()
	{
		Debug.Log("late");
		Debug.Log (t1.position);
		if (t1.position.y < 1) {
			t1.position = new Vector3 (t1.position.x, 1, t1.position.z);
		}
	}


}