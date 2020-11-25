using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 캐릭터 Animator 를 제어하는 컴포넌트입니다.
public sealed class PlayerCharacterAnimator : AnimatorController
{
	[SerializeField] private PlayerCharacter _PlayerCharacter;

	private void Start()
	{
		// 점프 시작시 실행할 내용을 정의합니다.
		_PlayerCharacter.playerCharacterMovement.onJumpStarted +=
			// 남은 점프 카운트에 따라 점프 애니메이션을 재생합니다.
			(int remainJumpCount) => animator.Play(
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
					animator.Play("Jump_Crouch", 0, 0.0f);
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
	}


	private void AnimEvent_CrouchFinished()
	{
		_PlayerCharacter.playerCharacterMovement.AllowMove();
	}



}
