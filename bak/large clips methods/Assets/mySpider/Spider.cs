using UnityEngine;
using System.Collections;

public class Spider : MonoBehaviour {
	
	public float acceleration=20f;
	public float maxSpeed=40f;
	public float maxangularSpeed=2;
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

		if (Input.GetKey ("up")||Input.GetKey ("down")) {
		
			anim.SetBool("_isMoving",true);
		}
		else {
		
			anim.SetBool("_isMoving",false);
			halt();
		}


		constrainSpeed ();
		//blend trees

		if (anim.GetBool ("_isMoving")) {
			//will revise later
			//anim.SetFloat("_speed",Mathf.Sqrt( Mathf.Pow (rb.velocity.x,2)+Mathf.Pow(rb.velocity.z,2)) /maxSpeed);
			anim.SetFloat("_speed",(Mathf.Abs(rb.velocity.x)+Mathf.Abs(rb.velocity.z))/(2*maxSpeed));
			anim.SetFloat("_angularVelocity",(rb.angularVelocity.y)/maxangularSpeed);

		}

		Debug.Log (rb.angularVelocity);


	}

	void constrainSpeed()
	{
		Vector3 velocity = new Vector3 ();
		velocity.x  = rb.velocity.x > maxSpeed ? maxSpeed : rb.velocity.x;
		velocity.x  = rb.velocity.x < -maxSpeed ? -maxSpeed : rb.velocity.x;
		velocity.z= rb.velocity.z > maxSpeed ? maxSpeed : rb.velocity.z;
		velocity.z= rb.velocity.z < -maxSpeed ? -maxSpeed : rb.velocity.z;
		velocity.y = rb.velocity.y;

		rb.velocity = velocity;

	}

	void halt()
	{
		Vector3 velocity = new Vector3 ();
		velocity.x = 0;
		velocity.z = 0;
		velocity.y = rb.velocity.y;
		
		rb.velocity = velocity;
	}
	
	void FixedUpdate()
	{
	
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
