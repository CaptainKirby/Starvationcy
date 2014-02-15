using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
	private Vector3 input;
	private CharacterMotor motor;
	private float speed;
	public float accel = 10;
	public float drag = 1;
	private Vector3 inputDir;
	void Start () 
	{
		motor = GetComponent<CharacterMotor>();
	}
	
	
	void Update () 
	{
		inputDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		speed = speed + accel * inputDir.magnitude * Time.deltaTime;
////		speed = Mathf.Clamp(speed, 0f, movementMax);
		speed = speed - speed * Mathf.Clamp01(drag * Time.deltaTime);

//		inputDir = new Vector3(
//		rigidbody.velocity = new Vector3(transform.forward.x * speed, gravity, transform.forward.z * speed);


		float Xon = Mathf.Abs (Input.GetAxis ("Joy X")); 
		
		if (Xon>.05){
			transform.Rotate(0, Input.GetAxis("Joy X") * 3, 0); 
		}
//		Vector3 moveDir = this.transform.TransformDirection(Vector3.forward);
//		moveDir = moveDir



//		motor.inputMoveDirection = new Vector3(transform.forward.x * inputDir.x, 0, transform.forward.z * inputDir.z);

		motor.inputMoveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
//		motor.inputMoveDirection = transform.right * Input.GetAxis("Horizontal");
//		Debug.Log (transform.forward.z);
		motor.inputJump = Input.GetKey(KeyCode.Space);
		
		
	}
	
	void FixedUpdate()
	{
//		Debug.Log (motor.grounded);
		
		
	}
}




