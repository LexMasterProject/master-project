/************************************************************************************

Copyright   :   Copyright 2014 Oculus VR, LLC. All Rights reserved.

Licensed under the Oculus VR Rift SDK License Version 3.2 (the "License");
you may not use the Oculus VR Rift SDK except in compliance with the License,
which is provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

You may obtain a copy of the License at

http://www.oculusvr.com/licenses/LICENSE-3.2

Unless required by applicable law or agreed to in writing, the Oculus VR SDK
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VR = UnityEngine.VR;

/// <summary>
/// A head-tracked stereoscopic virtual reality camera rig.
/// </summary>
[ExecuteInEditMode]
public class OVRCameraRig : MonoBehaviour
{
	/// <summary>
	/// The left eye camera.
	/// </summary>
	public Camera leftEyeCamera { get; private set; }
	/// <summary>
	/// The right eye camera.
	/// </summary>
	public Camera rightEyeCamera { get; private set; }
	/// <summary>
	/// Provides a root transform for all anchors in tracking space.
	/// </summary>
	public Transform trackingSpace { get; private set; }
	/// <summary>
	/// Always coincides with the pose of the left eye.
	/// </summary>
	public Transform leftEyeAnchor { get; private set; }
	/// <summary>
	/// Always coincides with average of the left and right eye poses.
	/// </summary>
	public Transform centerEyeAnchor { get; private set; }
	/// <summary>
	/// Always coincides with the pose of the right eye.
	/// </summary>
	public Transform rightEyeAnchor { get; private set; }
	/// <summary>
	/// Always coincides with the pose of the tracker.
	/// </summary>
	public Transform trackerAnchor { get; private set; }
	/// <summary>
	/// Occurs when the eye pose anchors have been set.
	/// </summary>
	public event System.Action<OVRCameraRig> UpdatedAnchors;

	private readonly string trackingSpaceName = "TrackingSpace";
	private readonly string trackerAnchorName = "TrackerAnchor";
	private readonly string eyeAnchorName = "EyeAnchor";
	private readonly string legacyEyeAnchorName = "Camera";


	//common variables
	public Transform follow;
	
	//auto variables
	public float distanceAway;
	public float distanceUp;
	public float smooth;
	private Vector3 targetPosition;

	//auto control variables
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
	public float angleMinConstraint;
	public float angleMaxConstraint;

	//full control
	public GameObject gplane;
	public Vector3 ginitPos;
	public GameObject gprefab;
	
	public GameObject []gprefabHolders;
	public int gnumberOfObjects = 20;
	public float gradius = 5f;
	public float gzoomSpeed=15;
	public float grotateSpeed=10;
	public float gmoveSpeed=10;
	
	private RaycastHit ghit;
	private float gedgeXLength;
	private float gedgeZLength;
	private Vector3[] gresetPosArr; 
	private float gdistDiff;
	private float gleftRighRotation;
	private float gupDownRotation;
	private float gforwardDiff;
	private float grightDiff;
	public float gdistConstraint;
	public float gangleMinConstraint;
	public float gangleMaxConstraint;


	//env objects prefab
	public GameObject Obstacle;
	public GameObject Food;
	public GameObject Danger;
	public List<GameObject>envObjectsHolderList;
	
	private EnvObject gcurrEnvObject;
	
	enum EnvObject
	{
		OBSTACLE,
		DANGER,
		FOOD
	};






#if UNITY_ANDROID && !UNITY_EDITOR
    bool correctedTrackingSpace = false;
#endif

#region Unity Messages
	private void Awake()
	{
		EnsureGameObjectIntegrity();
	}

	private void Start()
	{
		EnsureGameObjectIntegrity();
		envObjectsHolderList = new List<GameObject> ();

		if (!Application.isPlaying)
			return;


		/*
			init variables
		*/
	
		transform.position = new Vector3 (0, 10, 0);

		//init auto
		distanceAway = 10.0f;
		distanceUp = 30.0f;
		smooth = 0.1f;

		//init auto control
		zoomSpeed = 30;
		rotateSpeed = 30;
		distConstraint = 12.5f;
		angleMinConstraint = 5f;
		angleMaxConstraint = 85f;
		offset = Vector3.zero;
		lastPos = follow.position;


		//init full control
		gprefabHolders = new GameObject[gnumberOfObjects];
		gedgeXLength = gplane.GetComponent<Renderer> ().bounds.size.x;
		gedgeZLength = gplane.GetComponent<Renderer> ().bounds.size.z;
		
		gresetPosArr = new Vector3[4];
		gresetPosArr [0] = new Vector3 (-gedgeXLength / 2, gedgeXLength / 2, 0);
		gresetPosArr [1] = new Vector3 (0, gedgeZLength / 2, gedgeZLength / 2);
		gresetPosArr [2] = new Vector3 (gedgeXLength / 2, gedgeXLength / 2, 0); 
		gresetPosArr [3] = new Vector3 (0, gedgeZLength / 2, -gedgeZLength / 2);
		
		transform.position = gresetPosArr[0];
		transform.LookAt (Vector3.zero);
		gdistConstraint = 20f;
		gangleMinConstraint = 5f;
		gangleMaxConstraint = 85f;
		gcurrEnvObject = EnvObject.OBSTACLE;

	
		UpdateAnchors();
	}

	private void Update()
	{
		EnsureGameObjectIntegrity();
		
		if (!Application.isPlaying)
			return;

		UpdateAnchors();

#if UNITY_ANDROID && !UNITY_EDITOR

        if (!correctedTrackingSpace)
        {
            //HACK: Unity 5.1.1p3 double-counts the head model on Android. Subtract it off in the reference frame.

            var headModel = new Vector3(0f, OVRManager.profile.eyeHeight - OVRManager.profile.neckHeight, OVRManager.profile.eyeDepth);
            var eyePos = -headModel + centerEyeAnchor.localRotation * headModel;

            if ((eyePos - centerEyeAnchor.localPosition).magnitude > 0.01f)
            {
                trackingSpace.localPosition = trackingSpace.localPosition - 2f * (trackingSpace.localRotation * headModel);
                correctedTrackingSpace = true;
            }
        }
#endif
	}

#endregion

	private void UpdateAnchors()
	{

		if ((CAMERA_MODE)CameraSwitch.currentMode == CAMERA_MODE.AUTO) {
			targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
			transform.position = Vector3.Lerp (transform.position, targetPosition, Time.deltaTime * smooth);
			transform.LookAt (follow);
		} else if ((CAMERA_MODE)CameraSwitch.currentMode == CAMERA_MODE.AUTO_CONTROL) {
			distDiff = Input.GetAxis ("Zoom") * zoomSpeed * Time.deltaTime;
			leftRight= -Input.GetAxis("RotateLR")*rotateSpeed*Time.deltaTime*0.8f;
			upDown=-Input.GetAxis("RotateUD")*rotateSpeed*Time.deltaTime*0.8f;
			offset= follow.position  -lastPos;
			lastPos=follow.position;

			transform.RotateAround (follow.position, follow.up, leftRight);
			
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
			
			Vector3 cameraForwardXZ= new Vector3(transform.forward.x,0,transform.forward.z);
			float angleToFloor=Vector3.Angle(transform.forward,cameraForwardXZ);
			//	Debug.Log(angleToFloor);
			if(angleToFloor<angleMinConstraint&&upDown<0)
			{
				
			}
			else if(angleToFloor>angleMaxConstraint&&upDown>0)
			{
				
			}
			else
			{
				transform.RotateAround (follow.position, transform.right, upDown);
				
			}
			
			
			
			transform.Translate(offset,Space.World);
			transform.LookAt (follow);
		} else if ((CAMERA_MODE)CameraSwitch.currentMode == CAMERA_MODE.FULL_CONTROL) {


			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				transform.position = gresetPosArr [0];
				
				transform.LookAt (Vector3.zero);
			} else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				transform.position = gresetPosArr [1];
				transform.LookAt (Vector3.zero);
			} else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				transform.position = gresetPosArr [2];
				transform.LookAt (Vector3.zero);
			} else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				transform.position = gresetPosArr [3];
				transform.LookAt (Vector3.zero);
			}

			if(follow.position.y>10||follow.position.y<-10)
			{
				destroyObjects();
			}
			
			if(Input.GetKeyDown(KeyCode.F)||Input.GetButtonDown("PlaceObject"))
			{
				GameObject target;
				if(gcurrEnvObject==EnvObject.OBSTACLE)
				{
					target=Obstacle;
				}
				else if(gcurrEnvObject==EnvObject.FOOD)
				{
					target=Food;
				}
				else
				{
					target=Danger;
				}
				Vector3 objectPos=new Vector3(ghit.point.x,target.transform.localScale.y/2,ghit.point.z);
				envObjectsHolderList.Add((GameObject)Instantiate (target,  objectPos, Quaternion.identity));
				//createRandObject(ghit.point);	
			}

			if(Input.GetKeyDown(KeyCode.X)||Input.GetButtonDown("DeleteObject"))
			{
				for (int i = 0; i < envObjectsHolderList.Count; i++) // Loop with for.
				{
					//Console.WriteLine(envObjectsHolderList[i]);
					Destroy(envObjectsHolderList[i]);
				}
			}
			
			if(Input.GetKeyDown(KeyCode.C)||Input.GetButtonDown("ChangeObject"))
			{
				switchEnvObject();
			}
			gdistDiff = Input.GetAxis ("Zoom") * gzoomSpeed * Time.deltaTime;
			grightDiff = Input.GetAxis ("Horizontal") * gmoveSpeed * Time.deltaTime;
			gforwardDiff = -Input.GetAxis ("Vertical") * gmoveSpeed * Time.deltaTime;
			gleftRighRotation = -Input.GetAxis ("RotateLR") * grotateSpeed * Time.deltaTime;
			gupDownRotation = -Input.GetAxis ("RotateUD") * grotateSpeed * Time.deltaTime;

			transform.Translate (grightDiff, 0, 0);
			Vector3 vforwardDirection = new Vector3 (transform.forward.x, 0, transform.forward.z);
			vforwardDirection.Normalize ();
			transform.Translate (vforwardDirection * gforwardDiff, Space.World);
			


			if((CAMERA_MODE)CameraSwitch.currentMode == CAMERA_MODE.FULL_CONTROL)
			{
			//transform.Translate (xDiff, 0, zDiff,Space.World);
			if (Physics.Raycast (transform.position, transform.forward, out ghit, 1000)) {
				
				for (int i = 0; i < gnumberOfObjects; i++) {
					Destroy (gprefabHolders [i]);
					float angle = i * Mathf.PI * 2 / gnumberOfObjects;
					Vector3 pos = new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle)) * gradius;
					gprefabHolders [i] = (GameObject)Instantiate (gprefab, pos + ghit.point, Quaternion.identity);
				}
			}
			}
			else
			{
				for (int i = 0; i < gnumberOfObjects; i++) {
					Destroy (gprefabHolders [i]);
				}
			}
			
			
			
			
			float dist=Vector3.Distance(transform.position,ghit.point);
			//Debug.Log(dist);
			if(dist<gdistConstraint&& gdistDiff>0)
			{
				//no camera forward to spider
				
			}
			else
			{
				transform.position = Vector3.MoveTowards (transform.position, ghit.point, gdistDiff);
				
			}
			
			
			transform.RotateAround (ghit.point, Vector3.up, gleftRighRotation);
			
			Vector3 cameraForwardXZ= new Vector3(transform.forward.x,0,transform.forward.z);
			float angleToFloor=Vector3.Angle(transform.forward,cameraForwardXZ);
			//	Debug.Log(angleToFloor);
			if(angleToFloor<gangleMinConstraint&&gupDownRotation<0)
			{
				
			}
			else if(angleToFloor>gangleMaxConstraint&&gupDownRotation>0)
			{
				
			}
			else
			{
				transform.RotateAround (ghit.point, transform.right, gupDownRotation);
				
			}
		}


		bool monoscopic = OVRManager.instance.monoscopic;
		OVRPose tracker = OVRManager.tracker.GetPose(0d);
		trackerAnchor.localRotation = tracker.orientation;
		centerEyeAnchor.localRotation = VR.InputTracking.GetLocalRotation(VR.VRNode.CenterEye);
        leftEyeAnchor.localRotation = monoscopic ? centerEyeAnchor.localRotation : VR.InputTracking.GetLocalRotation(VR.VRNode.LeftEye);
		rightEyeAnchor.localRotation = monoscopic ? centerEyeAnchor.localRotation : VR.InputTracking.GetLocalRotation(VR.VRNode.RightEye);

		trackerAnchor.localPosition = tracker.position;
		centerEyeAnchor.localPosition = VR.InputTracking.GetLocalPosition(VR.VRNode.CenterEye);
		leftEyeAnchor.localPosition = monoscopic ? centerEyeAnchor.localPosition : VR.InputTracking.GetLocalPosition(VR.VRNode.LeftEye);
		rightEyeAnchor.localPosition = monoscopic ? centerEyeAnchor.localPosition : VR.InputTracking.GetLocalPosition(VR.VRNode.RightEye);

	





		if (UpdatedAnchors != null)
		{
			UpdatedAnchors(this);
		}
	}


	 void  destroyObjects()
	{
		for (int i = 0; i < envObjectsHolderList.Count; i++) // Loop with for.
		{
			//Console.WriteLine(envObjectsHolderList[i]);
			Destroy(envObjectsHolderList[i]);
		}
	}
	void createRandObject(Vector3 pos)
	{
		
		if (gcurrEnvObject == EnvObject.OBSTACLE) {
			//do cube
			GameObject cube= GameObject.CreatePrimitive(PrimitiveType.Cube);
			
			cube.transform.localScale=new Vector3(3,5,3);
			cube.transform.position=new Vector3(pos.x,5/2,pos.z);
			Color blue= new Color(255,0,0,1);
			cube.GetComponent<MeshRenderer>().material.color=blue;
			
		} else if (gcurrEnvObject == EnvObject.FOOD) {
			//do food
		} else if (gcurrEnvObject == EnvObject.DANGER) {
			//do danger
		}
	}

	void switchEnvObject()
	{
		if (gcurrEnvObject == EnvObject.OBSTACLE) {
			
			gcurrEnvObject = EnvObject.DANGER;
		} else if (gcurrEnvObject == EnvObject.DANGER) {
			gcurrEnvObject = EnvObject.FOOD;
		} else if (gcurrEnvObject == EnvObject.FOOD) {
			gcurrEnvObject=EnvObject.OBSTACLE;
		}
	}

	public void EnsureGameObjectIntegrity()
	{
		if (trackingSpace == null)
			trackingSpace = ConfigureRootAnchor(trackingSpaceName);

		if (leftEyeAnchor == null)
            leftEyeAnchor = ConfigureEyeAnchor(trackingSpace, VR.VRNode.LeftEye);

		if (centerEyeAnchor == null)
            centerEyeAnchor = ConfigureEyeAnchor(trackingSpace, VR.VRNode.CenterEye);

		if (rightEyeAnchor == null)
            rightEyeAnchor = ConfigureEyeAnchor(trackingSpace, VR.VRNode.RightEye);

		if (trackerAnchor == null)
			trackerAnchor = ConfigureTrackerAnchor(trackingSpace);

        bool needsCamera = (leftEyeCamera == null || rightEyeCamera == null);

		if (needsCamera)
		{
            leftEyeCamera = centerEyeAnchor.GetComponent<Camera>();
			if (leftEyeCamera == null)
			{
				leftEyeCamera = centerEyeAnchor.gameObject.AddComponent<Camera>();
			}

            rightEyeCamera = leftEyeCamera;
		}
		
		// Only the center eye camera should now render.

        int cameraCount = 0;
        int mainCount = 0;
		
		foreach (var c in gameObject.GetComponentsInChildren<Camera>().Where(v => v != leftEyeCamera))
		{
			if (c && c.enabled)
			{
				Debug.LogWarning("Having a Camera on " + c.name + " is deprecated. Disabling the Camera. Please use the Camera on " + leftEyeCamera.name + " instead.");
				c.enabled = false;

				if (c.CompareTag("MainCamera"))
					mainCount++;
			}
        }

        // Use "MainCamera" unless there were previously cameras and they didn't use it.
        if (needsCamera && (cameraCount == 0 || mainCount != 0))
            leftEyeCamera.tag = "MainCamera";
	}

	private Transform ConfigureRootAnchor(string name)
	{
		Transform root = transform.Find(name);

		if (root == null)
		{
			root = new GameObject(name).transform;
		}

		root.parent = transform;
		root.localScale = Vector3.one;
		root.localPosition = Vector3.zero;
		root.localRotation = Quaternion.identity;

		return root;
	}

	private Transform ConfigureEyeAnchor(Transform root, VR.VRNode eye)
	{
		string eyeName = (eye == VR.VRNode.CenterEye) ? "Center" : (eye == VR.VRNode.LeftEye) ? "Left" : "Right";
		string name = eyeName + eyeAnchorName;
		Transform anchor = transform.Find(root.name + "/" + name);

		if (anchor == null)
		{
			anchor = transform.Find(name);
		}

		if (anchor == null)
		{
			string legacyName = legacyEyeAnchorName + eye.ToString();
			anchor = transform.Find(legacyName);
		}

		if (anchor == null)
		{
			anchor = new GameObject(name).transform;
		}

		anchor.name = name;
		anchor.parent = root;
		anchor.localScale = Vector3.one;
		anchor.localPosition = Vector3.zero;
		anchor.localRotation = Quaternion.identity;

		return anchor;
	}

	private Transform ConfigureTrackerAnchor(Transform root)
	{
		string name = trackerAnchorName;
		Transform anchor = transform.Find(root.name + "/" + name);

		if (anchor == null)
		{
			anchor = new GameObject(name).transform;
		}

		anchor.parent = root;
		anchor.localScale = Vector3.one;
		anchor.localPosition = Vector3.zero;
		anchor.localRotation = Quaternion.identity;

		return anchor;
	}
}
