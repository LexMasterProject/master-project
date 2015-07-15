using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {




	private Text[] textComponents;
	private Text cameraText;
	// Use this for initialization

	void Awake()
	{
		textComponents = GetComponentsInChildren<Text> ();

		foreach (Text text in textComponents) {

			if(text.name=="CameraText")
			{
				cameraText=text;
			}
		}

	
	}
	void Start () {


	//	text = GameObject.Find ("CameraText").;
//		Debug.Log (text);
	}
	
	// Update is called once per frame
	void Update () {
		switch (CameraSwitch.activeIndex) {
		case 0:
			cameraText.text="Camera:AutoFollowCamera";
			break;
		case 1:
			cameraText.text="Camera:ControllableFollowCamera";
			break;
		case 2:
			cameraText.text="Camera:WorldCamera";
			break;
		default:
			cameraText.text="Camera:AutoFollowCamera";
			break;
		}
	}
}
