using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {
	
	public float acceleration=20f;
	public float maxSpeed=40f;
	public float torque=160f;
	
	private Rigidbody rb;
	private Animator anim;
	private GameObject head;

	// Use this for initialization
	void Start () {

		anim = gameObject.GetComponent<Animator> ();
	
		rb = GetComponent<Rigidbody>();
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 velocity = new Vector3 ();
		//set the limit
		velocity.x  = rb.velocity.x > maxSpeed ? maxSpeed : rb.velocity.x;
		velocity.x  = rb.velocity.x < -maxSpeed ? -maxSpeed : rb.velocity.x;
		velocity.z= rb.velocity.z > maxSpeed ? maxSpeed : rb.velocity.z;
		velocity.z= rb.velocity.z < -maxSpeed ? -maxSpeed : rb.velocity.z;
		velocity.y=rb.velocity.y;
		if (Input.GetKey ("up")) {
			anim.SetBool("_isWalking",true);	
			anim.SetBool("_isForward",true);

		}
		else if(Input.GetKey ("down"))
		{
			anim.SetBool("_isWalking",true);
			anim.SetBool("_isForward",false);

		}
		else {
			anim.SetBool("_isWalking",false);

		}

		rb.velocity = velocity;

	}
	
	void FixedUpdate()
	{
		Transform direction = transform.Find ("Armature/head");
		//Debug.Log (direction.position);
		if (Input.GetKey ("up")) {
			rb.AddForce(-transform.forward * acceleration);
		}
		else if(Input.GetKey ("down"))
		{
			rb.AddForce(transform.forward * acceleration);
		}

		float turn=Input.GetAxis("Horizontal");
		rb.AddTorque (transform.up * torque * turn);

		
	}
}
