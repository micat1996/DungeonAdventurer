using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// AI 행동 객체를 만들기 위한 기반 타입입니다.
public abstract class AIBehaviorBase : MonoBehaviour
{
	[Header("행동 시작 / 끝 지연 시간")]
	[SerializeField] protected float m_BehaviorBeginDelay = 0.0f;
	[SerializeField] protected float m_BehaviorFinalDelay = 0.0f;

	// 행동 시작, 끝을 제어하기 위한 프로퍼티입니다.
	public bool allowBehaviourStart { get; set; }
	/// - 이 값이 TRUE 일 경우 행동이 시작됩니다.
	
	public bool behaviourFinished { get; set; }
	/// - 이 값이 TRUE 일 경우 행동이 끝납니다.

	// 행동이 시작되거나 끝날 때 실행할 내용들을 나타냅니다.
	public System.Action behaviorBeginEvent { get; set; }
	public System.Action behaviorFinalEvent { get; set; }

	// 행동 시작, 끝 지연 시간에 대한 읽기 전용 프로퍼티
	public float behaviorBeginDelay => m_BehaviorBeginDelay;
	public float behaviorFinalDelay => m_BehaviorFinalDelay;

	// 해당 행동을 제어하기 위한 BehaviorController Component 객체
	public BehaviorController behaivorController { get; private set; }


	protected virtual void Awake()
	{
		behaivorController = GetComponent<BehaviorController>();

		// 매번 행동 시작시 행동 시작을 허용하도록 합니다.
		behaviorBeginEvent = () => allowBehaviourStart = true;
	}

	public virtual void InitializeBehaviour()
	{
		allowBehaviourStart = behaviourFinished = false;
	}

	public abstract void Run();

}
