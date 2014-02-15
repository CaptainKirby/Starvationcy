using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {
	public int health = 3;
	public bool healthy;
	public bool greasy;
	public bool strangeOutline;
	void Start () {

		if(renderer.material.name == "FoodOutline (Instance)")
		{
			renderer.material.SetFloat("_EdgeWidth", 0);
		}

	}


	void Update () {
	
	}
}
