using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BehaviorController))]
public sealed class EnemyCharacter : MonoBehaviour
{
	public NavMeshAgent navMeshAgent { get; private set; }
	public BehaviorController behaviorController { get; private set; }

	private void Awake()
	{
		navMeshAgent = GetComponent<NavMeshAgent>();
		behaviorController = GetComponent<BehaviorController>();
	}

	private void Update()
	{
		if (navMeshAgent.velocity.magnitude > 0.1f)
		{
			Vector3 xzDirection = navMeshAgent.velocity;
			xzDirection.y = 0.0f;
			xzDirection.Normalize();

			LookAt(xzDirection);
		}
	}


	public void LookAt(Vector3 lookDirecion)
	{
		transform.eulerAngles = lookDirecion.ToAngle() * Vector3.up;
	}

}
