using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteractable : MonoBehaviour
{
	// 상호작용전 표시될 문자열입니다.
	public virtual new string name { get; }

	// 상호작용이 끝날 때 호출됩니다.
	protected System.Action onInteractionFinished { get; private set; }


	// 상호작용을 시작시킵니다.
	public void StartInteraction(System.Action interactionFinishEvent = null)
	{
		// 상호작용을 시작합니다.
		Interaction();

		// 상호작용 끝 이벤트 설정
		onInteractionFinished = interactionFinishEvent;
	}

	// 상호작용을 끝냅니다.
	public void FinishInteracting()
	{
		// 상호작용 끝 이벤트 실행.
		onInteractionFinished?.Invoke();

		// 실행 내용 초기화
		onInteractionFinished = null;
	}

	// 상호작용시 호출될 메서드입니다.
	protected abstract void Interaction();
}
