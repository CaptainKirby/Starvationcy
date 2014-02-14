using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	private CharacterMotor motor;
	void Start () 
	{
		motor = GetComponent<CharacterMotor>();
	}
	
	
	void Update () 
	{

		motor.inputMoveDirection = new Vector3(Vector3.right.x * Input.GetAxis("Horizontal"), 0, Vector3.forward.z * Input.GetAxis("Vertical"));
		motor.inputJump = Input.GetKey(KeyCode.Space);
		
		
	}
	
	void FixedUpdate()
	{
		Debug.Log (motor.grounded);
		
		
	}
}




