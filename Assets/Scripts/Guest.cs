using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Guest : MonoBehaviour {
	public float walkRadius = 20;
	Vector3 randomDirection;
	private NavMeshAgent agent;
	Vector3 finalPosition;
	private float dist;
	// Use this for initialization
	private Transform player;
	public float sightRange = 10f;
	private Vector3 fwd;
	public bool seeing;
	private Eating eating;
	public float overlapSphereRadius = 5;
	private bool triggered;
	private AudioSource aSource;
	public List<AudioClip> clips;
	private float temp;
	void Start () {
		aSource = GetComponent<AudioSource>();
		player = GameObject.Find("Player").transform;
		eating = player.GetComponent<Eating>();
		agent = GetComponent<NavMeshAgent>();
		randomDirection = Random.insideUnitSphere * walkRadius;
		randomDirection += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
		finalPosition = hit.position;
		agent.SetDestination(finalPosition);
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if(seeing) eating.seen = true;
//		else eating.seen = false;
		Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, overlapSphereRadius);
		
		foreach(Collider col in hitColliders)
		{
			if(!triggered)
			{
				if(col.gameObject.name == "Player")
				{
	//				Debug.Log ("TEST");
					triggered = true;
					if(Random.value > 0.5f)
					{
						aSource.clip = clips[Random.Range(0, clips.Count)];
						aSource.pitch = Random.Range(1.0f, 1.2f);
						aSource.Play();
					}
				}
			}
		}

		if(triggered)
		{
			temp += Time.deltaTime;
		}

		if(temp > 10)
		{
			triggered = false;
			temp = 0;
		}

		dist = Vector3.Distance(this.transform.position, finalPosition);
		if(dist < 2f)
		{
			randomDirection = Random.insideUnitSphere * walkRadius;
			randomDirection += transform.position;
			NavMeshHit hit;
			NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
			finalPosition = hit.position;
			agent.SetDestination(finalPosition);
		}
		if(agent.velocity.magnitude < 0.1f)
		{
			randomDirection = Random.insideUnitSphere * walkRadius;
			randomDirection += transform.position;
			NavMeshHit hit;
			NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
			finalPosition = hit.position;
			agent.SetDestination(finalPosition);
		}

		fwd = (player.position - transform.position).normalized;
		RaycastHit hitRay;

		if (Physics.Raycast (transform.position, fwd,out hitRay, sightRange)) 
		{
			if(hitRay.transform.gameObject == player.gameObject)
			{
				Vector3 forward = transform.TransformDirection(Vector3.forward);
				Vector3 toOther = player.position - transform.position;
				if (Vector3.Dot(forward,toOther) > 0){

				Debug.DrawRay(transform.position + new Vector3(0, 1, 0),fwd * sightRange,Color.green);
				seeing = true;
				}
			}
			else seeing = false;
		}
		else seeing = false;





	}
}
