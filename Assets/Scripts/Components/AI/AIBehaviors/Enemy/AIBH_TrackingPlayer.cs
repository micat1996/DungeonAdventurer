using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBH_TrackingPlayer : AIBehaviorBase
{
	[SerializeField] private float _MaxTrackingTime = 3.0f;
	private EnemyCharacter _EnemyCharacter;
	private PlayerCharacter _PlayerCharacter;

	private WaitForSeconds _Wait05 = new WaitForSeconds(0.5f);

	protected override void Awake()
	{
		base.Awake();

		_EnemyCharacter = GetComponent<EnemyCharacter>();
		_PlayerCharacter = PlayerManager.Instance.playerCharacter;
	}

	public override void Run()
	{
		IEnumerator Run()
		{
			float trackingStartTime = Time.time;

			while (true)
			{
				if (Time.time - trackingStartTime > _MaxTrackingTime) break;

				yield return _Wait05;
				_EnemyCharacter.navMeshAgent.SetDestination(_PlayerCharacter.transform.position);
			}

			_EnemyCharacter.navMeshAgent.SetDestination(transform.position);
			behaviourFinished = true;
		}

		StartCoroutine(Run());
	}
}
