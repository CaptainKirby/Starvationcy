using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioSourceManager : MonoBehaviour {

	private GameObject asHolder;
	public AudioSource[] aSources;
	void Start () 
	{
		asHolder = transform.FindChild("AudioSources").gameObject;
		aSources = asHolder.gameObject.GetComponentsInChildren<AudioSource>();
	}
	

	void Update () {
	
	}

	public void PlaySource(string name, bool pitch)
	{
		foreach(AudioSource aSource in aSources)
		{

			if(aSource.gameObject.name == name)
			{
				if(pitch)
				{
					aSource.pitch = Random.Range(0.8f, 1.2f);
				}
				aSource.Play();
			}
			else
			{

			}
		}
	}
}
