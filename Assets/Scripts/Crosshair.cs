using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
	[HideInInspector]
	public Texture2D crosshairTexture;
	public Texture2D crosshairNeutralTex;
	public Texture2D crosshairGoTex;
	public Rect position;
	public float crosshairSize = 5;
	// Use this for initialization
	void Start () 
	{
		crosshairTexture = crosshairNeutralTex;

	}


	
	void OnGUI()
	{
//		if(OriginalOn == true)
//		{
			GUI.DrawTexture(position, crosshairTexture);
//		}
	}
	void Update () {
		position = new Rect((Screen.width - crosshairSize) / 2, (Screen.height - crosshairSize) /2, crosshairSize, crosshairSize);
	}
}
