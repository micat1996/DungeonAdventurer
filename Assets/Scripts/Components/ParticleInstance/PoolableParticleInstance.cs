using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 재사용 기능이 추가된 파티클 객체를 나타냅니다.
public sealed class PoolableParticleInstance : ParticleInstance,
	IObjectPoolable
{

	// 파티클 재생이 끝날 때까지 대기합니다.
	protected override IEnumerator WaitParticleFin()
	{

		// 파티클을 재생시킵니다.
		m_ParticleSystem.Play();

		// 재사용 불가능한 상태로 설정합니다.
		canRecyclable = false;

		// 파티클 재생이 끝날 때까지 대기합니다.
		yield return m_WaitParticleFin;

		// 재사용 가능한 상태로 설정합니다.
		canRecyclable = true;

		onParticleFinished?.Invoke();
	}


}
