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

	public GameObject prevFood;
	public GameObject prevFood2;
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
//	private float minutes;
//	private float seconds;
	private float timer;
	public float timerStart = 70;
	private float gold;
	public float greaseComboIncrease = 1;
	public GameObject comboObj;
	public GameObject goldGetObj;
	public GameObject commentObj;
	public GameObject lastCommentObj;

	private UILabel comboCounterGui;
	private UILabel goldCounterGui;
	private UILabel timerGui;

	public List<string> greasyComments;
	public List<string> healthyComments;
	public List<string> lastComments;
	void Start () 
	{

		timer = timerStart;
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
		comboCounterGui = uiRootInst.transform.Find("ComboCount").gameObject.GetComponent<UILabel>();
		goldCounterGui = uiRootInst.transform.Find("GoldCount").gameObject.GetComponent<UILabel>();
		goldCounterGui = uiRootInst.transform.Find("GoldCount").gameObject.GetComponent<UILabel>();
		timerGui = uiRootInst.transform.Find("Time").gameObject.GetComponent<UILabel>();

		
	}

	void Update () 
	{

		int minutes = Mathf.FloorToInt(timer / 60F);
		int seconds = Mathf.FloorToInt(timer - minutes * 60);
		
		string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

		timerGui.text = niceTime;

		goldCounterGui.text = "$" + gold;
		comboCounterGui.text = "X" + greaseCombo;
		timer -= Time.deltaTime;
//		minutes = Mathf.Floor(timer / 60).ToString("00");
//		seconds = (timer % 60).ToString("00");

//		if(minutes < 10) {
//			minutes = "0" + minutes.ToString();
//		}
//		if(seconds < 10) {
//			seconds = "0" + Mathf.RoundToInt(seconds).ToString();
//		}

		foodEatInterval = foodMeter/ (foodMeterStart);

		foodMeter = greaseLevel;



		foodbarSprite.color = Color.Lerp(botColor, topColor, foodMeter/foodMeterStart);
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

		if(timer < 0 && !starved)
		{
			starved = true;
			StartCoroutine(Starve());
//			starved = false;
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
				if(hit.transform.gameObject != prevFood)
				{
					prevFood2 = prevFood;
					if(prevFood2)
					{
						prevFood2.renderer.material.SetFloat("_EdgeWidth",0);
					}
				}
				prevFood = hit.transform.gameObject;

				hitFood = true;
				crosshair.crosshairTexture = crosshair.crosshairGoTex;
				crosshair.crosshairSize = 25;
//				if(renderer.material.name "FoodOutline (Instance)")
//				{
				if(!prevFood.GetComponent<Food>().strangeOutline)
				{
					prevFood.renderer.material.SetFloat("_EdgeWidth", 1);
				}
				else
				{
					prevFood.renderer.material.SetFloat("_EdgeWidth", .03f);
				}
			}
			else
			{
				hitFood = false;
				crosshair.crosshairTexture = crosshair.crosshairNeutralTex;
				crosshair.crosshairSize = 5;
				if(prevFood)
				{
					prevFood.renderer.material.SetFloat("_EdgeWidth", 0);
				}

			}
		}
		else
		{
			hitFood = false;
			crosshair.crosshairTexture = crosshair.crosshairNeutralTex;
			crosshair.crosshairSize = 5;
			if(prevFood)
			{
				prevFood.renderer.material.SetFloat("_EdgeWidth", 0);
			}
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
		GameObject goldBj = Instantiate(goldGetObj,uiRootInst.transform.position, Quaternion.identity) as GameObject;
		goldBj.transform.parent = uiRootInst.transform;
		if(food.GetComponent<Food>().greasy)
		{
			float rngGold = Random.Range(850, 1200) * greaseCombo;
			goldBj.GetComponent<UILabel>().text = "$ " + rngGold;
			gold += rngGold;
		}
		if(food.GetComponent<Food>().healthy)
		{
			float rngGold = Random.Range(200, 400) * greaseCombo;
			goldBj.GetComponent<UILabel>().text = "$ " + rngGold;
			gold += rngGold;
		}
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

		yield return new WaitForSeconds(0.2f);
		goldBj.SetActive(false);
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
			GameObject combo = Instantiate(comboObj,uiRootInst.transform.position, Quaternion.identity) as GameObject;
			combo.transform.parent = uiRootInst.transform;

			GameObject comment = Instantiate(commentObj,uiRootInst.transform.position, Quaternion.identity) as GameObject;
			comment.transform.parent = uiRootInst.transform;
			int rng = Random.Range(0,greasyComments.Count);
			comment.GetComponent<UILabel>().text = greasyComments[rng] + "!";
			lastComments.Add(greasyComments[rng]);

			greaseLevel += 10;

			greaseCombo += greaseComboIncrease;
			combo.GetComponent<UILabel>().text = "X" + greaseCombo;
			food.gameObject.SetActive(false);
			yield return new WaitForSeconds(1.5f);
			combo.SetActive(false);
			comment.SetActive(false);
		}

		if(food.GetComponent<Food>().healthy)
		{
			GameObject comment = Instantiate(commentObj,uiRootInst.transform.position, Quaternion.identity) as GameObject;
			comment.transform.parent = uiRootInst.transform;
			int rng = Random.Range(0,healthyComments.Count);
			comment.GetComponent<UILabel>().text = healthyComments[rng] + "!";
			lastComments.Add(healthyComments[rng]);

			greaseLevel -= 20;
			greaseCombo = 0;
			yield return new WaitForSeconds(1.5f);
			comment.SetActive(false);
//			gold += Random.Range(250, 400);
		}
		foodPicked = false;
		if(food.gameObject.activeSelf)
		{
			food.gameObject.SetActive(false);
		}

		yield return null;
	}

	IEnumerator Starve()
	{
		GameObject lComment = Instantiate(lastCommentObj,uiRootInst.transform.position, Quaternion.identity) as GameObject;
		lComment.transform.parent = uiRootInst.transform;
//		for(int i = 0; i < lastComments.Count; i++)
//		{
//
//		}
		lComment.GetComponent<UILabel>().text = string.Join(" ", lastComments.ToArray());
		yield return new WaitForSeconds(5);

		Application.LoadLevel(Application.loadedLevel);
	}

	void OnGUI()
	{

		
//		GUI.Label(new Rect(10,10,250,100), niceTime);

//		GUI.Label(new Rect(Screen.width - 300, Screen.height - 100,200, 50),"Cost: " + gold);
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
//		if(starved)
//		{
//			GUI.Label (new Rect (Screen.width/2 - 175, Screen.height/2 - 10, 350, 20), "YYOU ARE DONE EATING!");
//		}
	}

}
