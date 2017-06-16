using UnityEngine;
using System.Collections;

public class RoamingCamera : MonoBehaviour {



	public GameObject plane;
	public Vector3 initPos;


	public GameObject fly;
	public GameObject prefab;

	public GameObject []prefabHolders;
	public int numberOfObjects = 20;
	public float radius = 5f;
	public float zoomSpeed=15;
	public float rotateSpeed=10;
	public float moveSpeed=10;

	private RaycastHit hit;
	private float edgeXLength;
	private float edgeZLength;
	private Vector3[] resetPosArr; 
	private float distDiff;
	private float leftRighRotation;
	private float upDownRotation;
	private float forwardDiff;
	private float rightDiff;
	private Camera camSelf;
	public float distConstraint;
	public float angleMinConstraint;
	public float angleMaxConstraint;

	private EnvObject currEnvObject;

	enum EnvObject
	{
		OBSTACLE,
		DANGER,
		FOOD
	};






	
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
		camSelf = GetComponent<Camera> ();
		distConstraint = 20f;
		angleMinConstraint = 5f;
		angleMaxConstraint = 85f;

		currEnvObject = EnvObject.OBSTACLE;


	
	}



	// Update is called once per frame
	void Update () {
		if (camSelf.enabled) {
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				transform.position = resetPosArr [0];

				transform.LookAt (Vector3.zero);
			} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				transform.position = resetPosArr [1];
				transform.LookAt (Vector3.zero);
			} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				transform.position = resetPosArr [2];
				transform.LookAt (Vector3.zero);
			} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				transform.position = resetPosArr [3];
				transform.LookAt (Vector3.zero);
			}

			if(Input.GetKeyDown(KeyCode.F))
			{
				//Instantiate (fly,  hit.point, Quaternion.identity);
				createRandObject(hit.point);
			
			}

			if(Input.GetKeyDown(KeyCode.X))
			{
				//delete all env objects
			}

			if(Input.GetKeyDown(KeyCode.C))
			{
				switchEnvObject();
			}
			distDiff = Input.GetAxis ("Zoom") * zoomSpeed * Time.deltaTime;
			rightDiff = Input.GetAxis ("Horizontal") * moveSpeed * Time.deltaTime;
			forwardDiff = -Input.GetAxis ("Vertical") * moveSpeed * Time.deltaTime;
			leftRighRotation = -Input.GetAxis ("RotateLR") * rotateSpeed * Time.deltaTime;
			upDownRotation = -Input.GetAxis ("RotateUD") * rotateSpeed * Time.deltaTime;
		}
		 
	}

	void createRandObject(Vector3 pos)
	{

		if (currEnvObject == EnvObject.OBSTACLE) {
			//do cube
					GameObject cube= GameObject.CreatePrimitive(PrimitiveType.Cube);

					cube.transform.localScale=new Vector3(3,5,3);
					cube.transform.position=new Vector3(pos.x,5/2,pos.z);
					Color blue= new Color(255,0,0,1);
			cube.GetComponent<MeshRenderer>().material.color=blue;

		} else if (currEnvObject == EnvObject.FOOD) {
			//do food
		} else if (currEnvObject == EnvObject.DANGER) {
			//do danger
		}
	}


	void switchEnvObject()
	{
		if (currEnvObject == EnvObject.OBSTACLE) {
		
			currEnvObject = EnvObject.DANGER;
		} else if (currEnvObject == EnvObject.DANGER) {
			currEnvObject = EnvObject.FOOD;
		} else if (currEnvObject == EnvObject.FOOD) {
			currEnvObject=EnvObject.OBSTACLE;
		}
	}

	void LateUpdate()
	{
		if (camSelf.enabled) {

			transform.Translate (rightDiff, 0, 0);
			Vector3 vforwardDirection = new Vector3 (transform.forward.x, 0, transform.forward.z);
			vforwardDirection.Normalize ();
			transform.Translate (vforwardDirection * forwardDiff, Space.World);


			//transform.Translate (xDiff, 0, zDiff,Space.World);
			if (Physics.Raycast (transform.position, transform.forward, out hit, 1000)) {
				
				for (int i = 0; i < numberOfObjects; i++) {
					Destroy (prefabHolders [i]);
					float angle = i * Mathf.PI * 2 / numberOfObjects;
					Vector3 pos = new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle)) * radius;
					prefabHolders [i] = (GameObject)Instantiate (prefab, pos + hit.point, Quaternion.identity);
				}
			}




			float dist=Vector3.Distance(transform.position,hit.point);
			//Debug.Log(dist);
			if(dist<distConstraint&& distDiff>0)
			{
				//no camera forward to spider

			}
			else
			{
				transform.position = Vector3.MoveTowards (transform.position, hit.point, distDiff);
				
			}


			transform.RotateAround (hit.point, Vector3.up, leftRighRotation);

			Vector3 cameraForwardXZ= new Vector3(transform.forward.x,0,transform.forward.z);
			float angleToFloor=Vector3.Angle(transform.forward,cameraForwardXZ);
		//	Debug.Log(angleToFloor);
			if(angleToFloor<angleMinConstraint&&upDownRotation<0)
			{

			}
			else if(angleToFloor>angleMaxConstraint&&upDownRotation>0)
			{

			}
			else
			{
				transform.RotateAround (hit.point, transform.right, upDownRotation);

			}

		




		
		}
	}
}
