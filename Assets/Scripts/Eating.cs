using UnityEngine;
using System.Collections;

public class Eating : MonoBehaviour {

	private Transform cam;
	private Ray ray;
	RaycastHit hit;
	private Crosshair crosshair;
	public bool hitFood;
	private bool rayHit;
	void Start () 
	{
		cam = Camera.main.transform;
		crosshair = cam.GetComponent<Crosshair>();
	}

	void Update () 
	{
		rayHit = RayCast();
		ray = new Ray(cam.position, cam.forward);

		if(rayHit)
		{
			if(hit.transform.gameObject.tag == "Food")
			{
				crosshair.crosshairTexture = crosshair.crosshairGoTex;
			}
			else
			{
				crosshair.crosshairTexture = crosshair.crosshairNeutralTex;
			}
		}
		else
		{
			crosshair.crosshairTexture = crosshair.crosshairNeutralTex;
		}
//		
	}

	bool RayCast()
	{
		return (Physics.Raycast (new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z), cam.forward, out  hit, 15.0f));

	}

}
