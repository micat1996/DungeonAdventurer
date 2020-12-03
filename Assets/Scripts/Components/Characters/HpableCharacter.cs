using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class HpableCharacter : MonoBehaviour
{
	public new Collider collider { get; private set; }
	public virtual float hp { get; set; }

	protected virtual void Awake()
	{
		LevelInstance.levelInstance.hpableCharacters.Add(collider, this);
	}

	// 해당 캐릭터에게 대미지를 입힙니다.
	/// - damageCauser : 피해를 가하는 캐릭터
	/// - damage : 가하는 피해
	public void ApplyDamage(HpableCharacter damageCauser, Component componentCauser, float damage)
	{
		OnTakeDamage(damageCauser, componentCauser, damage);
	}

	// 대미지를 입었을 경우 호출됩니다.
	protected virtual void OnTakeDamage(HpableCharacter damageCauser, Component componentCauser, float damage)
	{
		// 피해량만큼 체력을 감소시킵니다.
		hp -= damage;

		// 체력이 0 이하가 된다면 사망시킵니다.
		if (hp <= 0.0f)
			OnCharacterDie();
	}

	protected virtual void OnCharacterDie()
	{

	}


}
