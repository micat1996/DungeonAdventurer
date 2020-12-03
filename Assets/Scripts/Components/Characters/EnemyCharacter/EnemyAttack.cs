using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public sealed class EnemyAttack : MonoBehaviour
{
	[Header("플레이어 감지 범위")]
	[SerializeField] private float _PlayerDetectionAreaRadius = 3.0f;

	[Header("공격 범위")]
	[SerializeField] private float _AttackRangeRadius = 1.5f;

	[Header("플레이어 감지 범위를 표시합니다.")]
	[SerializeField] private bool _DrawPlayerDetectionArea = false;

	
	private void Update()
	{
		CheckAttackRange();
	}

	// 공격 범위를 확인합니다.
	private void CheckAttackRange()
	{
		foreach(var collider in Physics.OverlapSphere(
			transform.forward,
			_PlayerDetectionAreaRadius, 
			1 << LayerMask.NameToLayer("Player")))
		{
			
		}
	}

	private void OnDrawGizmos()
	{

		if (_DrawPlayerDetectionArea)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _PlayerDetectionAreaRadius);
		}
	}





}
