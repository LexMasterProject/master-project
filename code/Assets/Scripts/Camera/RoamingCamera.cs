using UnityEngine;
using System.Collections;

public class RoamingCamera : MonoBehaviour {



	public GameObject plane;
	public Vector3 initPos;



	public GameObject prefab;

	public GameObject []prefabHolders;
	public int numberOfObjects = 20;
	public float radius = 5f;
	public float zoomSpeed=15;

	private float edgeXLength;
	private float edgeZLength;
	private Vector3[] resetPosArr; 
	private float distDiff;



	// Use this for initialization
	void Start () {

		prefabHolders = new GameObject[numberOfObjects];
		edgeXLength = plane.GetComponent<Renderer> ().bounds.size.x;
		edgeZLength = plane.GetComponent<Renderer> ().bounds.size.z;

		resetPosArr = new Vector3[4];
		resetPosArr [0] = new Vector3 (-edgeXLength / 2, edgeXLength / 2, 0);
		resetPosArr [1] = new Vector3 (0, edgeZLength / 2, edgeZLength / 2);
		resetPosArr [2] = new Vector3 (edgeXLength / 2, edgeXLength / 2, 0); 
		resetPosArr [3] = new Vector3 (0, edgeZLength / 2, -edgeZLength / 2);

		transform.position = resetPosArr[0];
		transform.LookAt (Vector3.zero);
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			transform.position = resetPosArr[0];
			transform.LookAt (Vector3.zero);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			transform.position = resetPosArr[1];
			transform.LookAt (Vector3.zero);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			transform.position = resetPosArr[2];
			transform.LookAt (Vector3.zero);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha4))
		{
			transform.position = resetPosArr[3];
			transform.LookAt (Vector3.zero);
		}
		 distDiff = Input.GetAxis ("Zoom") * zoomSpeed * Time.deltaTime;



	}

	void LateUpdate()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward,out hit, 1000)) {

			for (int i = 0; i < numberOfObjects; i++) {
				Destroy(prefabHolders[i]);
				float angle = i * Mathf.PI * 2 / numberOfObjects;
				Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
				prefabHolders[i]=(GameObject)Instantiate(prefab, pos+hit.point, Quaternion.identity);
			}
		}
		transform.position = Vector3.MoveTowards (transform.position, hit.point, distDiff);
	}
}
