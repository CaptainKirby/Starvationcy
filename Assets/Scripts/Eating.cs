using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private bool caught;
	[HideInInspector]
	public bool seen;
	[HideInInspector]
	public bool starved;
	public Texture2D eyeOpen;
	public Texture2D eyeClosed;
	public float eyeSize = 25;
	private float foodMeter = 100;
	private float foodMeterStart;

	public float foodmeterDecayRate = 3;
	public float foodGain = 20;
	public GameObject uiRoot;
	private GameObject uiRootInst;
//	private GameObject foodbarObj;
	private UISprite foodbarSprite;
	public Color topColor = Color.green;
	public Color botColor = Color.red;
	public List<GameObject> guests;
	public List<GameObject> notSeeingGuests;
	private int guestCount;
	private Vector3 fwd;
	public float sightRange = 10f;
	private float greaseLevel = 10f;
	private float greaseCombo = 1;
	private float gold;
	public float greaseComboIncrease = 1;
	void Start () 
	{
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Guest")) 
		{
			guestCount += 1;
			guests.Add(go);
		}
		foodMeterStart = foodMeter;
		foodMeter = greaseLevel;
		foodEatInterval = foodMeter/ (foodMeterStart);
		uiRootInst = Instantiate(uiRoot, new Vector3(9999,9999,9999), uiRoot.transform.rotation) as GameObject;
		foodbarSprite = uiRootInst.GetComponentInChildren<UISprite>();
		foodbarSprite.color = topColor;
		camShake = GetComponent<CameraShake>();
		cam = Camera.main.transform;
		foodParticleInst = Instantiate(foodParticle, cam.transform.position + new Vector3(cam.transform.forward.x, cam.transform.forward.y - 0.5f, cam.transform.forward.z), foodParticle.transform.rotation) as GameObject;
		foodParticleSystem = foodParticleInst.GetComponent<ParticleSystem>();
		crosshair = cam.GetComponent<Crosshair>();
	}

	void Update () 
	{
		foodEatInterval = foodMeter/ (foodMeterStart);

		foodMeter = greaseLevel;



		foodbarSprite.color = Color.Lerp(botColor, topColor, greaseLevel/10);
		foodbarSprite.fillAmount = foodMeter/foodMeterStart;
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

		if(guests.Count > 0)
		{
			foreach(GameObject gObj in guests)
			{
				if(!gObj.GetComponent<Guest>().seeing)
				{
					if(!notSeeingGuests.Contains(gObj))
					{
						notSeeingGuests.Add(gObj);
					}
				}
				if(gObj.GetComponent<Guest>().seeing)
				{
					if(notSeeingGuests.Contains(gObj))
					{
						notSeeingGuests.Remove(gObj);
					}
				}
			}
		}

		if(notSeeingGuests.Count == guests.Count)
		{
			seen = false;
		}
		else seen = true;
//				Debug.DrawRay(transform.position + new Vector3(0, 1, 0),fwd * sightRange,Color.green);
//				fwd = (gObj.transform.position - transform.position).normalized;
//				RaycastHit hitRay;
//				if (Physics.Raycast (transform.position, fwd,out hitRay, sightRange)) 
//				{
//					Debug.Log (hitRay.transform.gameObject);
////					seen = true;
//					if(hitRay.transform.gameObject.CompareTag("Guest"))
//					{
//
//						seen = true;
//					}
////					else seen = false;
//				}
////				else seen = false;
//			}
//		}
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
		if((foodMeter + foodGain) < foodMeterStart)
		{
		foodMeter += foodGain;
		}
//		StartCoroutine(FoodShake ());
		food.transform.localScale = food.transform.localScale * 0.8f;
		camShake.Shake();
		foodParticleSystem.Stop();
		foodParticleInst.transform.position = cam.transform.position + new Vector3(cam.transform.forward.x, cam.transform.forward.y - 0.5f, cam.transform.forward.z);

		foodParticleSystem.Play();

		yield return new WaitForSeconds(foodEatInterval/2);
		if(seen)  caught = true;
		yield return new WaitForSeconds(foodEatInterval/2);
		if(caught)
		{
			yield return new WaitForSeconds(1);
			Application.LoadLevel(Application.loadedLevel);
		}
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
		if(food.GetComponent<Food>().greasy)
		{
			greaseLevel += 10;
			gold += Random.Range(850, 1200) * greaseCombo;
			greaseCombo += greaseComboIncrease;
		}

		if(food.GetComponent<Food>().healthy)
		{
			greaseLevel -= 20;
			greaseCombo = 0;
			gold += Random.Range(250, 400);
		}
		foodPicked = false;
		food.gameObject.SetActive(false);

		yield return null;
	}

	IEnumerator Starve()
	{
		yield return new WaitForSeconds(1);
		Application.LoadLevel(Application.loadedLevel);
	}

	void OnGUI()
	{
		GUI.Label(new Rect(Screen.width - 300, Screen.height - 100,200, 50),"Gold: " + gold);
		if(seen)
		{
			GUI.DrawTexture(new Rect((Screen.width - eyeSize) / 2, (Screen.height - eyeSize)- 20, eyeSize, eyeSize), eyeOpen);
		}
		if(!seen)
		{
			GUI.DrawTexture(new Rect((Screen.width - eyeSize) / 2, (Screen.height - eyeSize)- 20, eyeSize, eyeSize), eyeClosed);
		}
		if(caught)
		{
			GUI.Label (new Rect (Screen.width/2 - 175, Screen.height/2 - 10, 350, 20), "YOU'VE BEEN SEEN EATING, YOU FILTH");
		}
		if(starved)
		{
			GUI.Label (new Rect (Screen.width/2 - 175, Screen.height/2 - 10, 350, 20), "YOU STARVED! EAT FASTER!!");
		}
	}

}
