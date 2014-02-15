using UnityEngine;
using System.Collections;

public class Eating : MonoBehaviour {

	private Transform cam;
	private Ray ray;
	RaycastHit hit;
	private Crosshair crosshair;
	public bool hitFood;
	private bool rayHit;

	private bool pickupButton;

	public Vector3 foodOffset;
	private bool foodPicked;
	private Transform food;
	void Start () 
	{
		cam = Camera.main.transform;
		crosshair = cam.GetComponent<Crosshair>();
	}

	void Update () 
	{
//		if(foodPicked)
//		{
//			food.position = food.position + foodOffset;
////			hit.transform.position = cam.transform.position + foodOffset;
//		}
		if(Input.GetKeyDown(KeyCode.JoystickButton0) && !foodPicked)
		{
			if(hitFood)
			{
				hit.transform.parent = cam.transform;
				food = hit.transform;
				foodPicked = true;
				food.position = cam.transform.position + new Vector3(cam.transform.forward.x, cam.transform.forward.y - 0.5f, cam.transform.forward.z);
				food.collider.enabled = false;

			}
		}
		rayHit = RayCast();
		ray = new Ray(cam.position, cam.forward);

		if(rayHit)
		{
			if(hit.transform.gameObject.tag == "Food")
			{
				hitFood = true;
				crosshair.crosshairTexture = crosshair.crosshairGoTex;
			}
			else
			{
				hitFood = false;
				crosshair.crosshairTexture = crosshair.crosshairNeutralTex;
			}
		}
		else
		{
			hitFood = false;
			crosshair.crosshairTexture = crosshair.crosshairNeutralTex;
		}
//		
	}

	bool RayCast()
	{
		return (Physics.Raycast (new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z), cam.forward, out  hit, 15.0f));

	}

}
