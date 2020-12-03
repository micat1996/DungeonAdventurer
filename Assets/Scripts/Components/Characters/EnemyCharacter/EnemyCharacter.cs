using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyAttack))]
[RequireComponent(typeof(BehaviorController))]
public sealed class EnemyCharacter : HpableCharacter
{
	public NavMeshAgent navMeshAgent { get; private set; }
	public EnemyAttack enemyAttack { get; private set; }
	public BehaviorController behaviorController { get; private set; }

	protected override void Awake()
	{
		base.Awake();

		navMeshAgent = GetComponent<NavMeshAgent>();
		enemyAttack = GetComponent<EnemyAttack>();
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
