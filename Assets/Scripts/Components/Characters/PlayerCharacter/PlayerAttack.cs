using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerAttack : MonoBehaviour
{
	private PlayerCharacter _PlayerCharacter;
	private PlayerCharacterAnimator _AnimatorController;

	// 무기 장착 상태를 나타냅니다.
	public bool isSwordEquipped =>
		// 무기 슬롯에 무기가 장착되어 있을 경우
		(!_PlayerCharacter.inventory.equipItems[ItemType.Sword].isEmpty);

	// --- Regular Attack ---
	// 기본 공격이 끝난 시간
	private float _LastAttackFinishTime;

	// 기본 공격 딜레이
	private float _RegularAttackDelay = 0.5f;

	// 다음 연계 공격으로 이어짐을 나타냅니다.
	public bool nextComboRegularAttack { get; private set; }

	// 현재 진행중인 연계 공격 횟수를 나타냅니다.
	public RegularAttackCombo regularAttackCombo { get; private set; }

	// 기본 공격중임을 나타냅니다.
	public bool isRegularAttacking { get; private set; }

	public System.Action onRegularAttackStarted { get; private set; }
	public System.Action onRegularAttackFinished { get; private set; }


	private void Awake()
	{
		_PlayerCharacter = GetComponent<PlayerCharacter>();
		_AnimatorController = PlayerManager.Instance.playerCharacter.animatorController;

		// 연계 공격을 할 때마다 실행할 내용을 정의합니다.
		onRegularAttackStarted += () =>
		{
			++regularAttackCombo;
			nextComboRegularAttack = false;
			
		};

		// 모든 연계 공격이 끝나서 공격이 끝난 경우 실행할 내용을 정의합니다.
		onRegularAttackFinished += () =>
		{
			nextComboRegularAttack = false;
			isRegularAttacking = false;
			regularAttackCombo = RegularAttackCombo.None;
			_LastAttackFinishTime = Time.time;
		};


	}

	private void Update()
	{
		if (InputManager.Instance.gameInputRegularAttack)
		RegularAttackStart();
	}

	private void RegularAttackStart()
	{
		if (!isSwordEquipped) return;

		if (Time.time - _LastAttackFinishTime <= _RegularAttackDelay) return;

		// 이동 불가능한 상태이거나, 공중에 있을 경우 공격을 실행하지 않도록 합니다.
		if (!_PlayerCharacter.playerCharacterMovement.isMovable ||
			!_PlayerCharacter.playerCharacterMovement.isGrounded) return;

		if (nextComboRegularAttack) return;

		// 기본 공격중이라면
		if (isRegularAttacking)
		{
			nextComboRegularAttack = true;
		}

		// 기본 공격중이 아니라면
		else
		{
			// 공격중 상태로 설정하며
			isRegularAttacking = true;

			// 첫 번째 기본 공격 애니메이션을 재생합니다.
			_AnimatorController.PlayRegularAttackAnimation(RegularAttackCombo.Combo1);
		}
	}



}
