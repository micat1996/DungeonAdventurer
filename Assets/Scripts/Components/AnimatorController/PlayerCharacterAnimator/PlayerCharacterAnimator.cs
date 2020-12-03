using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 캐릭터 Animator 를 제어하는 컴포넌트입니다.
public sealed class PlayerCharacterAnimator : AnimatorController
{
	[SerializeField] private PlayerCharacter _PlayerCharacter;


	private void Start()
	{
		string weapon = _PlayerCharacter.playerAttack.isSwordEquipped ?
			"Sword_" : "NoWeapon_";


		// 점프 시작시 실행할 내용을 정의합니다.
		_PlayerCharacter.playerCharacterMovement.onJumpStarted +=
			// 남은 점프 카운트에 따라 점프 애니메이션을 재생합니다.
			(int remainJumpCount) => animator.Play(
				(_PlayerCharacter.playerAttack.isSwordEquipped ? "Sword_" : "NoWeapon_") + 
				((remainJumpCount > 0) ? "Jump_Single" : "Jump_Multi"), 0, 0.0f);

		// 착지시 실행할 내용을 정의합니다.
		_PlayerCharacter.playerCharacterMovement.onLanded +=
			(int remainJumpCount) =>
			{
				// 남은 점프 카운트가 0 이라면
				if (remainJumpCount == 0)
				{
					// 캐릭터 이동을 중단합니다.
					_PlayerCharacter.playerCharacterMovement.StopMove();

					// 착지 애니메이션을 재생합니다.
					animator.Play(
						(_PlayerCharacter.playerAttack.isSwordEquipped ? "Sword_" : "NoWeapon_") +
						"Jump_Crouch", 0, 0.0f);
				}
			};



	}

	private void Update()
	{
		// 속력을 설정합니다.
		SetParam("_VelocityLength", 
			_PlayerCharacter.playerCharacterMovement.velocity.magnitude);

		SetParam("_IsInAir",
			!_PlayerCharacter.playerCharacterMovement.isGrounded);

		SetParam("_Sword", _PlayerCharacter.playerAttack.isSwordEquipped);
	}

	// 기본 공격 애니메이션을 재생합니다.
	public void PlayRegularAttackAnimation(RegularAttackCombo combo)
	{
		//if (crossFade)
		//	animator.CrossFade($"RegularAttack_{combo.ToString()}", 0.1f);
		//else 
		animator.Play($"RegularAttack_{combo.ToString()}");
	}


	private void AnimEvent_CrouchFinished()
	{
		_PlayerCharacter.playerCharacterMovement.AllowMove();
	}

	private void AnimEvent_RegularAttackStarted()
	{
		_PlayerCharacter.playerAttack.onRegularAttackStarted?.Invoke();
	}

	private void AnimEvent_TryComboRegularAttack()
	{
		if (_PlayerCharacter.playerAttack.nextComboRegularAttack)
			PlayRegularAttackAnimation(_PlayerCharacter.playerAttack.regularAttackCombo + 1);

	}

	private void AnimEvent_RegularAttackFinished()
	{
		_PlayerCharacter.playerAttack.onRegularAttackFinished?.Invoke();
	}



}
