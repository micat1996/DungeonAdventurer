using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// 파티클 재생을 위한 객체를 나타냅니다.
public class ParticleInstance : MonoBehaviour
{
	[Header("재생시킬 파티클")]
	[SerializeField] protected ParticleSystem m_ParticleSystem;

	public bool canRecyclable { get; set; }
	public Action onRecycleStartEvent { get; set; }

	// 파티클 재생이 끝날 때까지 대기합니다.
	protected WaitUntil m_WaitParticleFin;

	// 파티클 재생이 끝날 경우 호출되는 대리자입니다.
	public Action onParticleFinished;


	protected virtual void Awake()
	{
		m_WaitParticleFin = new WaitUntil(() => !m_ParticleSystem.isPlaying);
	}

	public virtual void PlayParticle()
	{
#if UNITY_EDITOR
		if (m_ParticleSystem == null)
			Debug.LogError("_ParticleSystem is not valud");
#endif

		// 파티클 재생 끝 대기 시작
		StartCoroutine(WaitParticleFin());
	}

	// 파티클 재생이 끝날 때까지 대기합니다.
	protected virtual IEnumerator WaitParticleFin()
	{
		// 파티클을 재생시킵니다.
		m_ParticleSystem.Play();

		// 파티클 재생이 끝날 때까지 대기합니다.
		yield return m_WaitParticleFin;

		// 파티클 재생 끝 이벤트 호출
		onParticleFinished?.Invoke();
	}
}
