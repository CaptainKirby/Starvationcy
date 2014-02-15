using UnityEngine;
using System.Collections;

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
	void Start () {
		player = GameObject.Find("Player").transform;
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

		fwd = (player.position - transform.position).normalized;

		RaycastHit hitRay;

		if (Physics.Raycast (transform.position, fwd,out hitRay, sightRange)) 
		{
			if(hitRay.transform.gameObject == player.gameObject)
			{
				Debug.DrawRay(transform.position + new Vector3(0, 1, 0),fwd * sightRange,Color.green);
				seeing = true;
			}
			else seeing = false;
		}
		else seeing = false;





	}
}
