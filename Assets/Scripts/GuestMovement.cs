using UnityEngine;
using System.Collections;

public class GuestMovement : MonoBehaviour {
	public float walkRadius = 20;
	Vector3 randomDirection;
	private NavMeshAgent agent;
	Vector3 finalPosition;
	private float dist;
	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
		randomDirection = Random.insideUnitSphere * walkRadius;
		randomDirection += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
		finalPosition = hit.position;
		agent.SetDestination(finalPosition);
	}
	
	// Update is called once per frame
	void Update () {
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
	}
}
