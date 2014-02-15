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
	private Food foodComponent;
	private bool takingBite;
	public float foodEatInterval = 0.5f;
	[HideInInspector]
	public bool eating;
	public GameObject foodParticle;
	private GameObject foodParticleInst;
	private ParticleSystem foodParticleSystem;

	private CameraShake camShake;
	void Start () 
	{
		camShake = GetComponent<CameraShake>();
		cam = Camera.main.transform;
		foodParticleInst = Instantiate(foodParticle, cam.transform.position + new Vector3(cam.transform.forward.x, cam.transform.forward.y - 0.5f, cam.transform.forward.z), foodParticle.transform.rotation) as GameObject;
		foodParticleSystem = foodParticleInst.GetComponent<ParticleSystem>();
		crosshair = cam.GetComponent<Crosshair>();
	}

	void Update () 
	{
		if(foodPicked)
		{

			if(Input.GetKeyDown(KeyCode.JoystickButton1))
			{
				if(!takingBite)
				{
					StartCoroutine(TakeBite());
				}
			}
		}


		if(Input.GetKeyDown(KeyCode.JoystickButton0) && !foodPicked)
		{
			if(hitFood)
			{
				hit.transform.parent = cam.transform;
				food = hit.transform;
				foodPicked = true;
				food.position = cam.transform.position + new Vector3(cam.transform.forward.x, cam.transform.forward.y - 0.5f, cam.transform.forward.z);
				food.collider.enabled = false;
				foodComponent = food.GetComponent<Food>();
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

	IEnumerator TakeBite()
	{
//		Debug.Log ("BITE!");
		eating = true;
		takingBite = true;
		foodComponent.health -= 1;
//		StartCoroutine(FoodShake ());
		camShake.Shake();
		foodParticleInst.transform.position = cam.transform.position + new Vector3(cam.transform.forward.x, cam.transform.forward.y - 0.5f, cam.transform.forward.z);

		foodParticleSystem.Play();
//		float y =  Mathf.Sin(Time. * 1.0f);
//		food.transform.position = new Vector3(food.transform.position.x, food.transform.position.y + y, food.transform.position.z);
		yield return new WaitForSeconds(foodEatInterval);

		takingBite = false;
		eating = false;
		if(foodComponent.health == 0)
		{
			StartCoroutine(FoodEaten());
		}
	}

	IEnumerator FoodShake()
	{
		bool onOff = true;
		float mTime = 0.1f;
		bool up = false;;
		while(onOff)
		{
//			food.transform.position = new Vector3 (food.transform.position.x, food.transform.position.y + (mTime/1000), food.transform.position.z);
//			if(mTime < 1 && !up)
//			{
//				mTime += Time.deltaTime * 10;
//			}
//			if(mTime > 1 && !up)
//			{
//				up = true;
//			}
//
//			if(up)
//			{
//				mTime-= Time.deltaTime ;
//				if(mTime < 0)
//				{
//					onOff = false;
//					Debug.Log ("TEST");
//				}
//			}

			yield return null;
		}

	}
	IEnumerator FoodEaten()
	{
		foodPicked = false;
		food.gameObject.SetActive(false);
		yield return null;
	}

}
