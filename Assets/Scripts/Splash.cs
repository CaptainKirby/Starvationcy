using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour {


	void Start () 
	{
		StartCoroutine(Next());
	}
	

	IEnumerator Next()
	{
		yield return new WaitForSeconds(5);
		Application.LoadLevel("Test");
	}
}
