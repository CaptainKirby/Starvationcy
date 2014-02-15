using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	public Texture2D crosshairTexture;
	public Rect position;
	public float crosshairSize = 5;
	// Use this for initialization
	void Start () 
	{
		position = new Rect((Screen.width - crosshairSize) / 2, (Screen.height - crosshairSize) /2, crosshairSize, crosshairSize);
	}
	

	
	void OnGUI()
	{
//		if(OriginalOn == true)
//		{
			GUI.DrawTexture(position, crosshairTexture);
//		}
	}
	void Update () {
	
	}
}
